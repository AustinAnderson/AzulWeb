import $ from 'jquery';
import SignalRClient from 'signalrClient.js'

class Main
{
    constructor(){
        this.gameState={};
        this.gameHash="";
        this.playerToken="";
        this.playerIndex=0;
        this.signalrClient=new SignalRClient(
            initialState=>{
                console.log(x);
                this.gameState=initialState.initialState;
                this.gameHash=initialState.initialHash;
            },
            stateChanges=>console.log(stateChanges),
            playerTokenAndIndex=>{
                console.log(playerTokenAndIndex);
                this.playerToken=playerTokenAndIndex.playerToken;
                this.playerIndex=playerTokenAndIndex.playerIndex;
            },
            userId=>this.signalrClient.NotifyServerGameJoined(userId,this.gameState.gameId)
        );
    }
}