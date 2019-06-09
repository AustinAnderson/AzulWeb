import {LitElement,html} from 'lit-element';
import '@polymer/paper-button/paper-button.js';
import '@polymer/paper-input/paper-input.js';

export class LogonPage extends LitElement
{
    static TagName(){ return "longon-page"; }
    static get properties(){
        return {
            _private: {type:Object},
            OnGameCancelled: {type: Object },
            OnGameStart: {type: Object },
            OnHostGameClicked: {type: Object},
            gameId: {type:String},
            userId: {type:String},
            gameIdEditable: {type:Boolean},
            userIdEditable: {type:Boolean},
            buttonContext: {type:String},
            buttonDisabled: {type:Boolean},
            cancelGameButtonHidden: {type:Boolean}
        };
    }
    RoomCodeChanged(){
        if(gameId==""){ this._private.RoomCodeClearedAction();}
        else { this._private.RoomCodeEnteredAction(); }
    }
    UserNameChanged(){
        if(userId==""){ this._private.UserNameClearedAction();}
        else { this._private.UserNameEnteredAction(); }
    }
    constructor(){
        super();
        this.gameId= "";
        this.userId= "";
        this.gameIdEditable= true;
        this.userIdEditable= true;
        this.buttonContext="Host Game";
        this.buttonDisabled=true;
        this.cancelGameButtonHidden=true;
        //intended to be overwritten with a call back for making the server cancel the game
        this.OnGameCancelled=(cancelledGameId) =>{
            alert(LogonPage.name+"."+OnGameCancelled.name+" never assigned!");
        }
        //intended to be overwritten with a call back for making the server start the game
        //and the outer class to switch the view to the main game view
        this.OnGameStart=()=>{ alert(LogonPage.name+"."+OnGameStart.name+" never assigned!"); }
        /**
         * intended to be overwritten with a call back for retrieving the new
         * game code from the server
         * @returns {string} server created game code
         * @memberof LogonButtonsAndInput
         */
        this.OnHostGameClicked=()=>{
            alert(LogonPage.name+"."+OnHostGameClicked.name+" never assigned!");
            return "";
        }
        
        this._private={
            RoomCodeEnteredAction:()=>{},
            RoomCodeClearedAction:()=>{},
            UserNameEnteredAction:()=>{},
            UserNameClearedAction:()=>{},
            ContextAwareButtonAction:()=>{},
            RemoveAllUsers(){

            },
            CancelGameButtonClicked(){
                OnGameCancelled(this._private.gameId);
                RoomTornDown();
            },
            contextButtonHandlers:{
                HostNewGameClicked(){
                    this._private.gameId=OnHostGameClicked();
                    this._private.become.WaitForMorePlayers();
                },
                JoinGameClicked(){
                    this._private.become.WaitForHostToStart();
                },
                LeaveLobby(){
                    this._private.become.ReadyToJoin();
                },
                StartGameClicked(){
                    OnGameStart();
                }
            },
            become:{
                NeedNameAndCode(){
                    this.cancelGameButtonHidden=true;
                    this.buttonContext="Host Game";
                    this.buttonDisabled=true;
                    this.gameIdEditable=true;
                    this.userIdEditable=true;
                    this._private.ContextAwareButtonAction=()=>{};
                    this._private.RoomCodeClearedAction=()=>{};
                    this._private.UserNameClearedAction=()=>{};
                    this._private.RoomCodeEnteredAction=this._private.become.NeedNameToJoin;
                    this._private.UserNameEnteredAction=this._private.become.ReadyToHost;
                },
                NeedNameToJoin(){
                    this.cancelGameButtonHidden=true;
                    this.buttonContext="Join Game";
                    this.buttonDisabled=true;
                    this.gameIdEditable=true;
                    this.userIdEditable=true;
                    this._private.ContextAwareButtonAction=()=>{};
                    this._private.RoomCodeClearedAction=this._private.become.NeedNameAndCode;
                    this._private.RoomCodeEnteredAction=()=>{};
                    this._private.UserNameClearedAction=()=>{};
                    this._private.UserNameEnteredAction=this._private.become.ReadyToJoin;
                },
                ReadyToHost(){
                    this.cancelGameButtonHidden=true;
                    this.buttonContext="Host Game";
                    this.buttonDisabled=false;
                    this.gameIdEditable=true;
                    this.userIdEditable=true;
                    this._private.ContextAwareButtonAction=this._private.contextButtonHandlers.HostNewGameClicked;
                    this._private.RoomCodeClearedAction=()=>{};
                    this._private.RoomCodeEnteredAction=this._private.become.NeedNameToJoin;
                    this._private.UserNameClearedAction=this._private.become.NeedNameAndCode;
                    this._private.UserNameEnteredAction=()=>{};
                },
                ReadyToJoin(){
                    this.cancelGameButtonHidden=true;
                    this.buttonContext="Join Game";
                    this.buttonDisabled=false;
                    this.gameIdEditable=true;
                    this.userIdEditable=true;
                    this._private.ContextAwareButtonAction=this._private.contextButtonHandlers.JoinGameClicked;
                    this._private.RoomCodeClearedAction=this._private.become.ReadyToHost;
                    this._private.RoomCodeEnteredAction=()=>{};
                    this._private.UserNameClearedAction=this._private.become.NeedNameToJoin;
                    this._private.UserNameEnteredAction=()=>{};
                },
                WaitForMorePlayers()
                {
                    this.cancelGameButtonHidden=false;
                    this.buttonContext="Start Game";
                    this.buttonDisabled=true;
                    this.gameIdEditable=false;
                    this.userIdEditable=false;
                    this._private.ContextAwareButtonAction=()=>{};
                    this._private.RoomCodeClearedAction=()=>{};
                    this._private.RoomCodeEnteredAction=()=>{};
                    this._private.UserNameClearedAction=()=>{};
                    this._private.UserNameEnteredAction=()=>{};
                },
                ReadyToStart()
                {
                    this.cancelGameButtonHidden=false;
                    this.buttonContext="Start Game";
                    this.buttonDisabled=false;
                    this.gameIdEditable=false;
                    this.userIdEditable=false;
                    this._private.ContextAwareButtonAction=this._private.contextButtonHandlers.StartGameClicked;
                    this._private.RoomCodeClearedAction=()=>{};
                    this._private.RoomCodeEnteredAction=()=>{};
                    this._private.UserNameClearedAction=()=>{};
                    this._private.UserNameEnteredAction=()=>{};
                },
                WaitForHostToStart()
                {
                    this.cancelGameButtonHidden=true;
                    this.buttonContext="Leave Lobby";
                    this.buttonDisabled=false;
                    this.gameIdEditable=false;
                    this.userIdEditable=false;
                    this._private.ContextAwareButtonAction=this._private.contextButtonHandlers.LeaveLobby;
                    this._private.RoomCodeClearedAction=()=>{};
                    this._private.RoomCodeEnteredAction=()=>{};
                    this._private.UserNameClearedAction=()=>{};
                    this._private.UserNameEnteredAction=()=>{};
                }
            }
        }
    }

    UserAdded(connectionId,userId,playerIconSrc)
    {
        document.querySelector("div")
        if(playerCount>=2){
            this._private.become.ReadyToStart();
        }
    }
    UserRemoved(userId){

    }
    /**
     * to be called by the user when a game session is cancelled by the host
     * @memberof LogonButtonsAndInput
     */
    RoomTornDown(){
        this._private.RemoveAllUsers();
        this.gameId="";//will this fire onchange?
        this._private.become.ReadyToHost();
    }

    
    render(){
        return html`
            <div>
                <papper-input label="room code" 
                              disabled="${this.gameIdEditable}"
                              value="${this.gameId}"
                              @change="${this.RoomCodeChanged}"
                              >
                </papper-input>

                <papper-input label="user name" 
                              disabled="${this.userIdEditable}"
                              value="${this.userId}"
                              @change="${this.UserNameChanged}"
                              >
                </papper-input>

                <papper-button ?hidden="${this.cancelGameButtonHidden}"
                               @click="${this.CancelGameButtonClicked}"
                               >
                    "Cancel"
                </papper-button>

                <papper-button disabled="${this.buttonDisabled}"
                               @click="${this._private.ContextAwareButtonAction}"
                               >
                    ${this.buttonContext}
                </papper-button>

                <logon-user-icon></logon-user-icon>
                <logon-user-icon></logon-user-icon>
                <logon-user-icon></logon-user-icon>
                <logon-user-icon></logon-user-icon>
            </div>
        `;
    }
}
customElements.define(LogonPage.TagName(),LogonPage);