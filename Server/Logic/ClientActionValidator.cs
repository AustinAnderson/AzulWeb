using System;
using System.Collections.Generic;
using System.Linq;
using Models.Client;
using Server.Exceptions;
using Server.Models.ServerModels.ErrorModels;

namespace Server.Logic
{
    public class ClientActionValidator
    {
        PlayerTokenMapHandler mapHandler;
        public ClientActionValidator(PlayerTokenMapHandler mapHandler){
            this.mapHandler=mapHandler;
        }
        public ActionValidationFailedModel ValidateRequest(ClientRequestModel request)
        {
            int hash=request.GameState.GetHashCode();
            if(request.GameStateHash!=request.GameState.GetHashCode()){
                throw new BadRequestException(
                    ClientFacingMessages.BadRequestMessages.ClientServerMismatchHash(
                        ""+request.GameStateHash,""+hash
                    ),
                    bootable:true
                );
                //maybe think of some way to boot the player from the game, idk
            }
            CheckPlayerToken(request.Action,request.GameState);
            ActionValidationFailedModel validationError=null;
            var shared=request.GameState.SharedData;
            if(request.Action.FromFactory)
            {
                if(request.Action.FactoryIndex >= shared.Factories.Length||
                   request.Action.FactoryIndex < 0
                )
                {
                    throw new BadRequestException($"invalid factory index {request.Action.FactoryIndex},"+
                    $" index must be positive and less than {shared.Factories.Length}",false);
                }
                validationError=CheckChosenFactoryNotEmpty(request.Action,shared);
                if(validationError==null){
                    validationError=CheckTypeExistsInFactory(request.Action,shared);
                }
            }
            else
            {
                validationError=CheckCenterOfTableNotEmpty(request.Action,shared);
                if(validationError==null){
                    validationError=CheckCenterOfTableHasType(request.Action,shared);
                }
            }
            if(validationError==null){
                validationError=CheckChosenPatternLineMatchesType(request.Action,request.GameState);
            }
            return validationError;
        }


        private void CheckPlayerToken(ActionDescriptionModel action,GameStateModel data){
            var playerMap=mapHandler.DecryptMap(data.PlayerTokenMapEnc);
            if(playerMap.TryGetValue(action.PlayerToken,out int currentPlayersIndex))
            {
                if(data.SharedData.CurrentTurnsPlayersIndex!=currentPlayersIndex){
                    throw new BadRequestException(
                        ClientFacingMessages.BadRequestMessages.PlayerPlayedOutOfTurn(
                            ""+data.SharedData.CurrentTurnsPlayersIndex,""+currentPlayersIndex
                        ),
                        false
                    );
                }
            }else{
                throw new BadRequestException(
                    ClientFacingMessages.BadRequestMessages.PlayerTokenInvalid(""+action.PlayerToken),
                    false
                );
            }
        }
        private ActionValidationFailedModel CheckChosenPatternLineMatchesType(ActionDescriptionModel action, GameStateModel data)
        {
            ActionValidationFailedModel error=null;
            var patternLines=data.PlayerData[data.SharedData.CurrentTurnsPlayersIndex].PatternLines;
            var patternLine=patternLines[action.PatternLineIndex];
            for(int i=0;i<patternLine.Count;i++){
                if(patternLine[i]!=null&& patternLine[i].Type!=action.TileType)
                {
                    error=new ActionValidationFailedModel{
                        ErrorOnCenterOfTable=false,
                        PatternLineIndex=action.PatternLineIndex,
                        Message=ClientFacingMessages.PatternLineContainsDifferentType(
                            ""+action.TileType,""+patternLine[i].Type,""+action.PatternLineIndex+1
                        )
                    };
                    break;
                }
            }
            return error;
        }
        private ActionValidationFailedModel CheckCenterOfTableHasType(ActionDescriptionModel action,SharedDataModel data)
        {
            ActionValidationFailedModel error=null;
            if(!data.CenterOfTable.Any(x=>x.Type==action.TileType))
            {
                error=new ActionValidationFailedModel{
                    ErrorOnCenterOfTable=true,
                    Message=ClientFacingMessages.InvalidTileTypeForCenterOfTable(""+action.TileType)
                };
            }
            return error;
        }
        private ActionValidationFailedModel CheckCenterOfTableNotEmpty(ActionDescriptionModel action,SharedDataModel data)
        {
            ActionValidationFailedModel error=null;
            if(data.CenterOfTable.Count==0)
            {
                error=new ActionValidationFailedModel{
                    ErrorOnCenterOfTable=true,
                    Message=ClientFacingMessages.NoDrawingFromEmptyCenterOfTable()
                };
            }
            return error;
        }
        
        private ActionValidationFailedModel CheckChosenFactoryNotEmpty(ActionDescriptionModel action,SharedDataModel data)
        {
            ActionValidationFailedModel error=null;
            if(data.Factories[action.FactoryIndex].IsEmpty)
            {
                error=new ActionValidationFailedModel
                {
                    FactoryIndex=action.FactoryIndex,
                    Message=ClientFacingMessages.NoDrawingFromEmptyFactory()
                };
            }
            return error;
        }
        private ActionValidationFailedModel CheckTypeExistsInFactory(ActionDescriptionModel action,SharedDataModel data)
        {
            ActionValidationFailedModel error=null;
            bool matchingTile=false;
            var Factory=data.Factories[action.FactoryIndex];
            for(int i=0;i<Factory.IndexLimit;i++)
            {
                if(Factory[i]?.Type==action.TileType){
                    matchingTile=true; 
                    break;
                }
            }
            if(!matchingTile)
            {
                error=new ActionValidationFailedModel
                {
                    FactoryIndex=action.FactoryIndex,
                    Message=ClientFacingMessages.InvalidTileTypeForFactory(""+action.TileType)
                };
            }
            return error;
        }
    }
}

