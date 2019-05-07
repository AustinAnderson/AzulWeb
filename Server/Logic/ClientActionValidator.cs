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
        public ActionValidationFailedModel ValidateRequest(ClientRequestModel request)
        {
            if(request.GameStateHash!=request.GameState.GetHashCode()){
                throw new BadRequestException("client is out of sync with server");
                //maybe think of some way to boot the player from the game, idk
            }
            ActionValidationFailedModel validationError=null;
            var shared=request.GameState.SharedData;
            if(request.Action.FromFactory)
            {
                if(request.Action.FactoryIndex >= shared.Factories.Length||
                   request.Action.FactoryIndex < 0
                )
                {
                    throw new BadRequestException($"invalid factory index {request.Action.FactoryIndex},"+
                    $" index must be positive and less than {shared.Factories.Length}");
                }
                validationError=CheckChosenFactoryNotEmpty(request.Action,request.GameState.SharedData);
                validationError=CheckTypeExistsInFactory(request.Action,request.GameState.SharedData);
            }
            else
            {
                validationError=CheckCenterOfTableNotEmpty(request.Action,request.GameState.SharedData);
                validationError=CheckCenterOfTableHasType(request.Action,request.GameState.SharedData);
            }
            return validationError;

        }
        private ActionValidationFailedModel CheckCenterOfTableHasType(ActionDescriptionModel action,SharedDataModel data)
        {
            ActionValidationFailedModel error=null;
            if(!data.CenterOfTable.Any(x=>x.Type==action.tileType))
            {
                error=new ActionValidationFailedModel{
                    ErrorOnFactory=false,
                    Message=ClientFacingMessages.InvalidTileTypeForCenterOfTable(""+action.tileType)
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
                    ErrorOnFactory=false,
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
                    ErrorOnFactory=true,
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
                if(Factory[i].Type==action.tileType){
                    matchingTile=true; 
                    break;
                }
            }
            if(!matchingTile)
            {
                error=new ActionValidationFailedModel
                {
                    ErrorOnFactory=true,
                    FactoryIndex=action.FactoryIndex,
                    Message=ClientFacingMessages.InvalidTileTypeForFactory(""+action.tileType)
                };
            }
            return error;
        }
    }
}

