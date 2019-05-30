using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Models.Client;
using Server.Models.ServerModels.ChangeTrackingModels;
using Server.Models.ServerModels.SetupModels;

namespace Server.SignalRHubs
{
    //public are called by the client
    public class GameContentHub:Hub
    {
        internal async Task SendGameStart(InitialStateModel state)
        {
            await ReinsertToGroup(
                state.InitialState.GameId,
                state.InitialState.PlayerData.Select(p=>p.ConnectionId)
            );
            await Clients.Group(state.InitialState.GameId).SendAsync("GameStart",state);
        }
        internal async Task SendUpdate(
            string gameId,GameStateChangesModel stateChanges, IEnumerable<string> connectionIds
        )
        {
            await ReinsertToGroup(gameId,connectionIds);
            await Clients.Group(gameId).SendAsync("Update",stateChanges);
        }
        private async Task ReinsertToGroup(string gameId,IEnumerable<string> connectionIds)
        {
            foreach(var id in connectionIds)//not sure what happens when dups
            {
                await Groups.AddToGroupAsync(id,gameId);
            }
        }
        //client: connect.done(()=>server.RebroadcastJoinedGame)
        //sends broadcast with username and connecting id,
        //game making client holds those connection ids, and 
        //
        public async Task RebroadCastJoinedGame(string userId,string gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId,gameId);
            await Clients.Group(gameId).SendAsync("JoinedGame",userId);
        }
    }
}

