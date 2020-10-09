using System;
using System.Collections.Generic;
using System.Linq;
using Models.Client;
using Models.Server;
using Server.Exceptions;
using Server.Logic.ChangeTracking;
using Server.Models.ServerModels.SuccessModels;

namespace Server.Logic.ModelStateChangers
{
    public class ModelChanger
    {
        private PlayerTokenMapHandler mapHandler;
        private FactoryOfferPhaseChanger factoryOfferPhaseChanger;
        private RoundEndHandler roundEndHandler;
        private WallTiler wallTiler;
        private ScoreChangeCalculator scoringCalculator;
        private ChangeTracker tracker;
        public ModelChanger(
            PlayerTokenMapHandler mapHandler,
            FactoryOfferPhaseChanger factoryOfferPhaseChanger,
            RoundEndHandler roundEndHandler,
            ScoreChangeCalculator scoringCalculator,
            WallTiler wallTiler,
            ChangeTracker tracker
        )
        {
            this.mapHandler=mapHandler;
            this.factoryOfferPhaseChanger=factoryOfferPhaseChanger;
            this.roundEndHandler=roundEndHandler;
            this.scoringCalculator=scoringCalculator;
            this.wallTiler=wallTiler;
            this.tracker=tracker;
        }
        public ResponseModel ProcessRequest(ClientRequestModel request,GameStateModel state){

            var action = new GameActionModel(request.Action, state, request.GameStateHash);
            ResponseModel response=new ResponseModel();
            var oldState=action.GameState.DeepCopy();
            factoryOfferPhaseChanger.ProcessClientChanges(action);
            var changesThisStep=tracker.FindChanges(oldState,action.GameState);
            response.FactoryOfferPhaseTileChanges=changesThisStep.TileChanges;
            if(roundEndHandler.ActionEndsRound(action))
            {
                oldState=action.GameState.DeepCopy();
                wallTiler.MovePatternLineTilesToWalls(action);
                changesThisStep=tracker.FindChanges(oldState,action.GameState);
                response.WallTileMoves=new WallMovePhaseModel{
                    WallChanges= changesThisStep.WallChanges,
                    Discards=changesThisStep.TileChanges
                };
                oldState=action.GameState.DeepCopy();
                scoringCalculator.UpdateScores(action,response.WallTileMoves.WallChanges);
                response.GameOver=roundEndHandler.GameHasEnded(action);
                if(response.GameOver)
                {
                    scoringCalculator.AddFinalBonusesToScore(action);
                }
                else{
                    roundEndHandler.SetupNextRound(action);
                }
                changesThisStep=tracker.FindChanges(oldState,action.GameState);
                response.ScoreChanges=changesThisStep.ScoreChanges;
                response.NextRoundSetupChanges=changesThisStep.TileChanges;
                UpdateLeadingPlayer(response,action.GameState.PlayerData);
            }
            response.NewGameStateHash=action.GameState.GetHashCode();
            return response;
        }
        private void UpdateLeadingPlayer(ResponseModel respone, List<PlayerDataModel> playerData){
            respone.LeadingPlayerIndex=0;
            for(int i=0;i<playerData.Count;i++){
                if(playerData[i].Score>playerData[respone.LeadingPlayerIndex].Score){
                    respone.LeadingPlayerIndex=i;
                }
            }
        }
    }
}

