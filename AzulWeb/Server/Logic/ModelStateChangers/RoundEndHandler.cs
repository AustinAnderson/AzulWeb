using System;
using System.Collections.Generic;
using System.Linq;
using Models.Client;
using Models.Server;

namespace Server.Logic.ModelStateChangers
{
    public class RoundEndHandler
    {
        private readonly Random rng=new Random(DateTime.Now.GetHashCode());
        public bool ActionEndsRound(GameActionModel request)
        {
            //if center of table is empty and all factories are empty, round is done
            bool phaseDone=request.GameState.SharedData.CenterOfTable.Count==0;
            for(int i=0;phaseDone&&i<request.GameState.SharedData.Factories.Length;i++)
            {
                if(!request.GameState.SharedData.Factories[i].IsEmpty) phaseDone=false;
            }
            return phaseDone;
        }
        public bool GameHasEnded(GameActionModel request){
            bool gameOver=false;
            var playerData=request.GameState.PlayerData;
            for(int i=0;i<playerData.Count;i++){
                for(int r=0;r<playerData[i].Wall.Length;r++){
                    bool rowFull=true;
                    for(int c=0;c<playerData[i].Wall[r].Length;c++){
                        if(playerData[i].Wall[r][c]==null) rowFull=false;
                    }
                    if(rowFull) gameOver=true;
                }
            }
            return gameOver;
        }

        public void SetupNextRound(GameActionModel request)
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
        private void DiscardTableCenter(GameActionModel request){
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
        private TileModel DiscardFloorLinesAndPluckFirstPlayerMarker(GameActionModel request){
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

