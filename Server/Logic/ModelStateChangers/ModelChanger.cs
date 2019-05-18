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
        public ResponseModel ProcessRequest(ClientRequestModel request){
            ResponseModel response=new ResponseModel();
            var oldState=request.GameState.DeepCopy();
            factoryOfferPhaseChanger.ProcessClientChanges(request);
            var changesThisStep=tracker.FindChanges(oldState,request.GameState);
            response.FactoryOfferPhaseTileChanges=changesThisStep.TileChanges;

            if(roundEndHandler.ActionEndsRound(request))
            {
                oldState=request.GameState.DeepCopy();
                wallTiler.MovePatternLineTilesToWalls(request);
                changesThisStep=tracker.FindChanges(oldState,request.GameState);
                response.WallTileMoves=new WallMovePhaseModel{
                    WallChanges= changesThisStep.WallChanges,
                    Discards=changesThisStep.TileChanges
                };
                oldState=request.GameState.DeepCopy();
                scoringCalculator.UpdateScores(request,response.WallTileMoves.WallChanges);
                roundEndHandler.SetupNextRound(request);
                changesThisStep=tracker.FindChanges(oldState,request.GameState);
            }
            response.ScoreChanges=changesThisStep.ScoreChanges;
            response.NextRoundSetupChanges=changesThisStep.TileChanges;
            UpdateLeadingPlayer(response,request.GameState.PlayerData);
            response.NewGameStateHash=request.GameState.GetHashCode();
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

