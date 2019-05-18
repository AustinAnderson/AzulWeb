using System;
using System.Collections.Generic;
using System.Linq;
using Models.Client;

namespace Server.Logic.ModelStateChangers
{
    public class RoundEndHandler
    {
        private readonly Random rng=new Random(DateTime.Now.GetHashCode());
        public bool ActionEndsRound(ClientRequestModel request)
        {
            //if center of table is empty and all factories are empty, round is done
            bool phaseDone=request.GameState.SharedData.CenterOfTable.Count==0;
            for(int i=0;phaseDone&&i<request.GameState.SharedData.Factories.Length;i++)
            {
                if(!request.GameState.SharedData.Factories[i].IsEmpty) phaseDone=false;
            }
            return phaseDone;
        }

        public void SetupNextRound(ClientRequestModel request)
        {
            DiscardTableCenter(request);
            var firstPlayerMarker=DiscardFloorLinesAndPluckFirstPlayerMarker(request);
            request.GameState.SharedData.CenterOfTable.Add(firstPlayerMarker);
            foreach(var factory in request.GameState.SharedData.Factories){
                for(int i=0;i<factory.IndexLimit;i++){
                    if(factory[i]==null){
                        var tile=RandTileFromBagOrNull(request.GameState.SharedData.Bag);
                        if(tile==null) 
                            MoveDiscardIntoBag(request.GameState.SharedData);
                        tile=RandTileFromBagOrNull(request.GameState.SharedData.Bag);
                        factory[i]=tile;
                    }
                }
            }
        }
        private void DiscardTableCenter(ClientRequestModel request){
            foreach(var tile in request.GameState.SharedData.CenterOfTable){
                request.GameState.SharedData.DiscardPile.Add(tile);
            }
            request.GameState.SharedData.CenterOfTable=new HashSet<TileModel>();
        }
        private void MoveDiscardIntoBag(SharedDataModel state){
            state.Bag=state.DiscardPile;
            state.DiscardPile=new HashSet<TileModel>();
        }
        private TileModel RandTileFromBagOrNull(ISet<TileModel> bag)
        {
            TileModel drawn=null;
            if(bag.Count>0){
                drawn=bag.Skip(rng.Next(0,bag.Count)-1).First();
                bag.Remove(drawn);
            }
            return drawn;
        }
        private TileModel DiscardFloorLinesAndPluckFirstPlayerMarker(ClientRequestModel request){
            TileModel firstPlayerMarker=null;
            foreach(var playerData in request.GameState.PlayerData)
            {
                while(!playerData.FloorLine.IsEmpty){
                    var popped=playerData.FloorLine.PopOrNull();
                    if(popped.Type==TileType.FirstPlayerMarker){
                        firstPlayerMarker=popped;
                    } 
                    else{
                        request.GameState.SharedData.DiscardPile.Add(popped);
                    }
                }
            }
            if(firstPlayerMarker==null) {
                throw new ArgumentException(
                    "in next round setup, first player marker was never added to anyone's floorline"
                );
            }
            return firstPlayerMarker;
        }
    }
}

