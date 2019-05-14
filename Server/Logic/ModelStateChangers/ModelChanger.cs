using System;
using System.Collections.Generic;
using System.Linq;
using Models.Client;
using Models.Server;
using Server.Exceptions;

namespace Server.Logic.ModelStateChangers
{
    public class ModelChanger
    {
        private PlayerTokenMapHandler mapHandler;
        private FactoryOfferPhaseChnager factoryOfferPhaseChanger;
        private RoundEndHandler roundEndHandler;
        private WallTiler wallTiler;
        private ScoreChangeCalculator scoringCalculator;
        public ModelChanger(
            PlayerTokenMapHandler mapHandler,
            FactoryOfferPhaseChnager factoryOfferPhaseChanger,
            RoundEndHandler roundEndHandler,
            ScoreChangeCalculator scoringCalculator,
            WallTiler wallTiler 
        )
        {
            this.mapHandler=mapHandler;
            this.factoryOfferPhaseChanger=factoryOfferPhaseChanger;
            this.roundEndHandler=roundEndHandler;
            this.scoringCalculator=scoringCalculator;
            this.wallTiler=wallTiler;
        }
        public ResponseModel ProcessRequest(ClientRequestModel request){
            ResponseModel response=new ResponseModel();
            response.FactoryOfferPhaseTileChanges=factoryOfferPhaseChanger.ProcessClientChanges(request);
            if(roundEndHandler.ActionEndsRound(request)){
                response.WallTileMoves=wallTiler.MovePatternLineTilesToWalls(request);
            }
            response.NewGameStateHash=request.GameState.GetHashCode();
        }
    }
}

