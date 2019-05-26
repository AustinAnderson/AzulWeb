using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Models.Client;
using Server.Models.ServerModels.ChangeTrackingModels;

namespace Server.SignalRHubs
{
    public class GameContentHub:Hub
    {
        public Task SendGameStart(GameStateModel state)
            => Clients.Group(state.GameId).SendAsync(nameof(SendGameStart),state);
        public Task SendUpdate(string gameId,GameStateChangesModel stateChanges)
            => Clients.Group(gameId).SendAsync(nameof(SendUpdate),stateChanges);
        public async Task SendJoinedGame(string userId,string gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId,gameId);
            await Clients.Group(gameId).SendAsync(nameof(SendJoinedGame),userId);
        }
    }
}

