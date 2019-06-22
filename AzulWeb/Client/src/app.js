import $ from 'jquery';
import {SignalRClient} from './util/signalrClient.js'
import {ApiAccess} from './util/apiAccess.js'
import './views/logon/logon-page.js'

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
            this.updateGame,
            playerTokenAndIndex=>{
                console.log(playerTokenAndIndex);
                this.playerToken=playerTokenAndIndex.playerToken;
                this.playerIndex=playerTokenAndIndex.playerIndex;
            },
            userId=>this.signalrClient.NotifyServerGameJoined(userId,this.gameState.gameId)
        );
        this.restClient=new ApiAccess();
        var logonPage=document.querySelector("logon-page");
        logonPage.OnHostGameClicked=(setGameCallback)=>{ 
            var gameId="";
            console.log("call api request new game");
            this.restClient.requestNewGame()
                      .done(res=>setGameCallback(res))
                      .fail(()=>alert("unable to get new game from server"));
        }
        //logonPage.OnGameCancelled=
        //logonPage.OnGameStart=
        logonPage.hidden=false;
    }
    updateGame(stateChanges){
        console.log(stateChanges);
    }

}
new Main();