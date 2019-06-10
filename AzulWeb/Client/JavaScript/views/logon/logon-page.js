import {LitElement,html} from 'lit-element';
import '@polymer/paper-button/paper-button.js';
import '@polymer/paper-input/paper-input.js';

class LogonPage extends LitElement
{
    static TagName(){ return "logon-page"; }
    static get properties(){
        return {
            gameId: {type:String},
            userId: {type:String},
            gameIdEditable: {type:Boolean},
            userIdEditable: {type:Boolean},
            buttonContext: {type:String},
            buttonDisabled: {type:Boolean},
            cancelGameButtonHidden: {type:Boolean}
        };
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
        
        this.__private={
            RoomCodeChanged(newValue){
                gameId=newValue;
                if(gameId==""){ this.__private.RoomCodeClearedAction();}
                else { this.__private.RoomCodeEnteredAction(); }
            },
            UserNameChanged(newValue){
                userId=newValue;
                if(userId==""){ this.__private.UserNameClearedAction();}
                else { this.__private.UserNameEnteredAction(); }
            },
            RoomCodeEnteredAction:()=>{},
            RoomCodeClearedAction:()=>{},
            UserNameEnteredAction:()=>{},
            UserNameClearedAction:()=>{},
            ContextAwareButtonAction:()=>{},
            RemoveAllUsers(){

            },
            CancelGameButtonClicked(){
                OnGameCancelled(this.__private.gameId);
                RoomTornDown();
            },
            contextButtonHandlers:{
                HostNewGameClicked(){
                    this.__private.gameId=OnHostGameClicked();
                    this.__private.become.WaitForMorePlayers();
                },
                JoinGameClicked(){
                    this.__private.become.WaitForHostToStart();
                },
                LeaveLobby(){
                    this.__private.become.ReadyToJoin();
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
                    this.__private.ContextAwareButtonAction=()=>{};
                    this.__private.RoomCodeClearedAction=()=>{};
                    this.__private.UserNameClearedAction=()=>{};
                    this.__private.RoomCodeEnteredAction=this.__private.become.NeedNameToJoin;
                    this.__private.UserNameEnteredAction=this.__private.become.ReadyToHost;
                },
                NeedNameToJoin(){
                    this.cancelGameButtonHidden=true;
                    this.buttonContext="Join Game";
                    this.buttonDisabled=true;
                    this.gameIdEditable=true;
                    this.userIdEditable=true;
                    this.__private.ContextAwareButtonAction=()=>{};
                    this.__private.RoomCodeClearedAction=this.__private.become.NeedNameAndCode;
                    this.__private.RoomCodeEnteredAction=()=>{};
                    this.__private.UserNameClearedAction=()=>{};
                    this.__private.UserNameEnteredAction=this.__private.become.ReadyToJoin;
                },
                ReadyToHost(){
                    this.cancelGameButtonHidden=true;
                    this.buttonContext="Host Game";
                    this.buttonDisabled=false;
                    this.gameIdEditable=true;
                    this.userIdEditable=true;
                    this.__private.ContextAwareButtonAction=this.__private.contextButtonHandlers.HostNewGameClicked;
                    this.__private.RoomCodeClearedAction=()=>{};
                    this.__private.RoomCodeEnteredAction=this.__private.become.NeedNameToJoin;
                    this.__private.UserNameClearedAction=this.__private.become.NeedNameAndCode;
                    this.__private.UserNameEnteredAction=()=>{};
                },
                ReadyToJoin(){
                    this.cancelGameButtonHidden=true;
                    this.buttonContext="Join Game";
                    this.buttonDisabled=false;
                    this.gameIdEditable=true;
                    this.userIdEditable=true;
                    this.__private.ContextAwareButtonAction=this.__private.contextButtonHandlers.JoinGameClicked;
                    this.__private.RoomCodeClearedAction=this.__private.become.ReadyToHost;
                    this.__private.RoomCodeEnteredAction=()=>{};
                    this.__private.UserNameClearedAction=this.__private.become.NeedNameToJoin;
                    this.__private.UserNameEnteredAction=()=>{};
                },
                WaitForMorePlayers()
                {
                    this.cancelGameButtonHidden=false;
                    this.buttonContext="Start Game";
                    this.buttonDisabled=true;
                    this.gameIdEditable=false;
                    this.userIdEditable=false;
                    this.__private.ContextAwareButtonAction=()=>{};
                    this.__private.RoomCodeClearedAction=()=>{};
                    this.__private.RoomCodeEnteredAction=()=>{};
                    this.__private.UserNameClearedAction=()=>{};
                    this.__private.UserNameEnteredAction=()=>{};
                },
                ReadyToStart()
                {
                    this.cancelGameButtonHidden=false;
                    this.buttonContext="Start Game";
                    this.buttonDisabled=false;
                    this.gameIdEditable=false;
                    this.userIdEditable=false;
                    this.__private.ContextAwareButtonAction=this.__private.contextButtonHandlers.StartGameClicked;
                    this.__private.RoomCodeClearedAction=()=>{};
                    this.__private.RoomCodeEnteredAction=()=>{};
                    this.__private.UserNameClearedAction=()=>{};
                    this.__private.UserNameEnteredAction=()=>{};
                },
                WaitForHostToStart()
                {
                    this.cancelGameButtonHidden=true;
                    this.buttonContext="Leave Lobby";
                    this.buttonDisabled=false;
                    this.gameIdEditable=false;
                    this.userIdEditable=false;
                    this.__private.ContextAwareButtonAction=this.__private.contextButtonHandlers.LeaveLobby;
                    this.__private.RoomCodeClearedAction=()=>{};
                    this.__private.RoomCodeEnteredAction=()=>{};
                    this.__private.UserNameClearedAction=()=>{};
                    this.__private.UserNameEnteredAction=()=>{};
                }
            }
        }
    }

    UserAdded(connectionId,userId,playerIconSrc)
    {
        document.querySelector("div")
        if(playerCount>=2){
            this.__private.become.ReadyToStart();
        }
    }
    UserRemoved(userId){

    }
    /**
     * to be called by the user when a game session is cancelled by the host
     * @memberof LogonButtonsAndInput
     */
    RoomTornDown(){
        this.__private.RemoveAllUsers();
        this.gameId="";//will this fire onchange?
        this.__private.become.ReadyToHost();
    }

    
    render(){
        return html`
            <div>
                <paper-input label="room code" 
                              ?disabled="${this.gameIdEditable}"
                              value="${this.gameId}"
                              @keyup="${e=>this.__private.RoomCodeChanged(e.target.value)}"
                              >
                </paper-input>

                <paper-input label="user name" 
                              ?disabled="${this.userIdEditable}"
                              value="${this.userId}"
                              @keyup="${e=>this.__private.UserNameChanged(e.target.value)}"
                              >
                </paer-input>

                <paper-button ?hidden="${this.cancelGameButtonHidden}"
                               @click="${this.CancelGameButtonClicked}"
                               >
                    "Cancel"
                </paper-button>

                <paper-button ?disabled="${this.buttonDisabled}"
                               @click="${this.__private.ContextAwareButtonAction}"
                               >
                    ${this.buttonContext}
                </paper-button>

                <logon-user-icon></logon-user-icon>
                <logon-user-icon></logon-user-icon>
                <logon-user-icon></logon-user-icon>
                <logon-user-icon></logon-user-icon>
            </div>
        `;
    }
}
customElements.define(LogonPage.TagName(),LogonPage);