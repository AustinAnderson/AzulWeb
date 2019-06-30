import {LitElement,html} from 'lit-element';
import '@polymer/paper-button/paper-button.js';
import '@polymer/paper-input/paper-input.js';
import './logon-user-icon.js'
import {playerIndexIconMap} from '../playerIndexIconMap.js'

class LogonPage extends LitElement
{
    static TagName(){ return "logon-page"; }
    static get properties(){
        return {
            gameIdErrorMessage: {type:String},
            userIdErrorMessage: {type:String},
            gameIdInvalid: {type:Boolean},
            userIdInvalid: {type:Boolean},
            gameId: {type:String},
            userId: {type:String},
            gameIdEditable: {type:Boolean},
            userIdEditable: {type:Boolean},
            buttonContext: {type:String},
            buttonDisabled: {type:Boolean},
            cancelGameButtonHidden: {type:Boolean},
            /*
            [{
                connectionId,
                userId,
                playerIndex
            }]
            */
            playerList: {type:Array}
        };
    }
    constructor(){
        super();
        this.gameIdErrorMessage="";
        this.userIdErrorMessage="";
        this.gameIdInvalid=false;
        this.userIdInvalid=false;
        this.gameId= "";
        this.userId= "";
        this.gameIdEditable= true;
        this.userIdEditable= true;
        this.buttonContext="Host Game";
        this.buttonDisabled=true;
        this.cancelGameButtonHidden=true;
        this.playerList=[];
        console.log(playerIndexIconMap[0])
        //intended to be overwritten with a call back for making the server cancel the game
        this.OnGameCancelled=(cancelledGameId) =>{
            alert("LogonPage.OnGameCancelled never assigned!");
        }
        //intended to be overwritten with a call back for making the server start the game
        //and the outer class to switch the view to the main game view
        this.OnGameStart=()=>{ alert("LogonPage.OnGameStart never assigned!"); }
         
        /** 
          intended to be overwritten with a call back for retrieving the new
          game code from the server
          takes a function to set the gameid as an argument
        */
        this.OnHostGameClicked=(setGameIdCallBack)=>{
            alert("LogonPage.OnHostGameClicked never assigned!");
            setGameIdCallBack("someCodeFromServer");
        }
        
        var privvar=(function(that){ return {
            playerCount:0,
            RoomCodeChanged(newValue){
                that.gameId=newValue;
                if(that.gameId==""){ this.RoomCodeClearedAction();}
                else { this.RoomCodeEnteredAction(); }
                that.gameIdInvalid=false;
                that.gameIdErrorMessage="";
            },
            UserNameChanged(newValue){
                that.userId=newValue;
                if(that.userId==""){ this.UserNameClearedAction();}
                else { this.UserNameEnteredAction(); }
            },
            RoomCodeEnteredAction:()=>{},
            RoomCodeClearedAction:()=>{},
            UserNameEnteredAction:()=>{},
            UserNameClearedAction:()=>{},
            ContextAwareButtonAction:()=>{},
            CancelGameButtonClicked(){
                that.OnGameCancelled(that.gameId);
                that.RoomTornDown();
            },
            contextButtonHandlers:{
                HostNewGameClicked(){
                    console.log(that);
                    that.OnHostGameClicked(x=>{
                        console.log("got game id from server: "+x);
                        that.gameId=x
                    });
                    privvar.become.WaitForMorePlayers();
                },
                JoinGameClicked(){
                    privvar.become.WaitForHostToStart();
                },
                LeaveLobby(){
                    privvar.become.ReadyToJoin();
                },
                StartGameClicked(){
                    that.OnGameStart();
                }
            },
            become:{
                NeedNameAndCode(){
                    console.log("need name and code");
                    that.cancelGameButtonHidden=true;
                    that.buttonContext="Host Game";
                    that.buttonDisabled=true;
                    that.gameIdEditable=true;
                    that.userIdEditable=true;
                    privvar.ContextAwareButtonAction=()=>{};
                    privvar.RoomCodeClearedAction=()=>{};
                    privvar.UserNameClearedAction=()=>{};
                    privvar.RoomCodeEnteredAction=privvar.become.NeedNameToJoin;
                    privvar.UserNameEnteredAction=privvar.become.ReadyToHost;
                },
                NeedNameToJoin(){
                    console.log("need name to join");
                    that.cancelGameButtonHidden=true;
                    that.buttonContext="Join Game";
                    that.buttonDisabled=true;
                    that.gameIdEditable=true;
                    that.userIdEditable=true;
                    privvar.ContextAwareButtonAction=()=>{};
                    privvar.RoomCodeClearedAction=privvar.become.NeedNameAndCode;
                    privvar.RoomCodeEnteredAction=()=>{};
                    privvar.UserNameClearedAction=()=>{};
                    privvar.UserNameEnteredAction=privvar.become.ReadyToJoin;
                },
                ReadyToHost(){
                    console.log("ready to host");
                    that.cancelGameButtonHidden=true;
                    that.buttonContext="Host Game";
                    that.buttonDisabled=false;
                    that.gameIdEditable=true;
                    that.userIdEditable=true;
                    privvar.ContextAwareButtonAction=privvar.contextButtonHandlers.HostNewGameClicked;
                    privvar.RoomCodeClearedAction=()=>{};
                    privvar.RoomCodeEnteredAction=privvar.become.ReadyToJoin;
                    privvar.UserNameClearedAction=privvar.become.NeedNameAndCode;
                    privvar.UserNameEnteredAction=()=>{};
                },
                ReadyToJoin(){
                    console.log("ready to join");
                    that.cancelGameButtonHidden=true;
                    that.buttonContext="Join Game";
                    that.buttonDisabled=false;
                    that.gameIdEditable=true;
                    that.userIdEditable=true;
                    privvar.ContextAwareButtonAction=privvar.contextButtonHandlers.JoinGameClicked;
                    privvar.RoomCodeClearedAction=privvar.become.ReadyToHost;
                    privvar.RoomCodeEnteredAction=()=>{};
                    privvar.UserNameClearedAction=privvar.become.NeedNameToJoin;
                    privvar.UserNameEnteredAction=()=>{};
                },
                WaitForMorePlayers()
                {
                    console.log("wait for more players");
                    that.cancelGameButtonHidden=false;
                    that.buttonContext="Start Game";
                    that.buttonDisabled=true;
                    that.gameIdEditable=false;
                    that.userIdEditable=false;
                    privvar.ContextAwareButtonAction=()=>{};
                    privvar.RoomCodeClearedAction=()=>{};
                    privvar.RoomCodeEnteredAction=()=>{};
                    privvar.UserNameClearedAction=()=>{};
                    privvar.UserNameEnteredAction=()=>{};
                },
                ReadyToStart()
                {
                    console.log("ready to start");
                    that.cancelGameButtonHidden=false;
                    that.buttonContext="Start Game";
                    that.buttonDisabled=false;
                    that.gameIdEditable=false;
                    that.userIdEditable=false;
                    privvar.ContextAwareButtonAction=privvar.contextButtonHandlers.StartGameClicked;
                    privvar.RoomCodeClearedAction=()=>{};
                    privvar.RoomCodeEnteredAction=()=>{};
                    privvar.UserNameClearedAction=()=>{};
                    privvar.UserNameEnteredAction=()=>{};
                },
                WaitForHostToStart()
                {
                    console.log("Wait for host to start");
                    that.cancelGameButtonHidden=true;
                    that.buttonContext="Leave Lobby";
                    that.buttonDisabled=false;
                    that.gameIdEditable=false;
                    that.userIdEditable=false;
                    privvar.ContextAwareButtonAction=privvar.contextButtonHandlers.LeaveLobby;
                    privvar.RoomCodeClearedAction=()=>{};
                    privvar.RoomCodeEnteredAction=()=>{};
                    privvar.UserNameClearedAction=()=>{};
                    privvar.UserNameEnteredAction=()=>{};
                }
            }
        }})(this)
        this.__private=privvar;
        this.__private.become.NeedNameAndCode();
    }

    SetPlayerList(newPlayerList)
    {
        this.playerList=newPlayerList;
        if(this.playerList.length>=2){
            this.__private.become.ReadyToStart();
        }
    }
    /**
     * to be called by the user when a game session is cancelled by the host
     * @memberof LogonButtonsAndInput
     */
    RoomTornDown(){
        this.playerList=[];
        this.gameId="";//will this fire onchange?
        this.__private.become.ReadyToHost();
    }

    
    render(){
        return html`
            <style>
.parentDiv{
    display:flex;
}
.users{
    display:block;
}
.card {
    width: 250px;
    box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
    text-align:center;
}
.cardInner{
    display:inline-block;
    text-align:left;
    width: 95%;
}
        </style>
        <div class="parentDiv">
            <div class="card">
                <div class="cardInner">
                <paper-input id="roomInputId" label="room code" 
                              ?disabled="${!this.gameIdEditable}"
                              value="${this.gameId}"
                              @keyup="${e=>this.__private.RoomCodeChanged(e.target.value)}"
                              ?invalid="${this.gameIdInvalid}"
                              error-message="${this.gameIdErrorMessage}"
                              >
                </paper-input>

                <paper-input id="userInputId" label="user name" 
                              ?disabled="${!this.userIdEditable}"
                              value="${this.userId}"
                              @keyup="${e=>this.__private.UserNameChanged(e.target.value)}"
                              ?invalid="${this.userIdInvalid}"
                              error-message="${this.userIdErrorMessage}"
                              >
                </paper-input>

                <paper-button ?hidden="${this.cancelGameButtonHidden}"
                               @click=${this.__private.CancelGameButtonClicked}
                               >
                    Cancel
                </paper-button>

                <paper-button ?disabled=${this.buttonDisabled}
                               @click=${this.__private.ContextAwareButtonAction}
                               >
                    ${this.buttonContext}
                </paper-button>
                </div>
            </div>
            <div class="users">
                ${this.playerList.map(player=>html`
                    <logon-user-icon
                        userId="${player.userId}"
                        iconSrc="${playerIndexIconMap[player.playerIndex]}"
                    >
                    </logon-user-icon>
                `)}
            </div>
        </div>
        `;
    }
}
customElements.define(LogonPage.TagName(),LogonPage);