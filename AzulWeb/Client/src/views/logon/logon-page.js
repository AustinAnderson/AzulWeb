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
        
        var privvar={
            RoomCodeChanged(newValue){
                this.gameId=newValue;
                if(this.gameId==""){ this.RoomCodeClearedAction();}
                else { this.RoomCodeEnteredAction(); }
            },
            UserNameChanged(newValue){
                this.userId=newValue;
                if(this.userId==""){ this.UserNameClearedAction();}
                else { this.UserNameEnteredAction(); }
            },
            RoomCodeEnteredAction:()=>{},
            RoomCodeClearedAction:()=>{},
            UserNameEnteredAction:()=>{},
            UserNameClearedAction:()=>{},
            ContextAwareButtonAction:()=>{},
            RemoveAllUsers(){

            },
            CancelGameButtonClicked(){
                OnGameCancelled(this.gameId);
                RoomTornDown();
            },
            contextButtonHandlers:{
                HostNewGameClicked(){
                    this.gameId=OnHostGameClicked();
                    privvar.become.WaitForMorePlayers();
                },
                JoinGameClicked(){
                    privvar.become.WaitForHostToStart();
                },
                LeaveLobby(){
                    privvar.become.ReadyToJoin();
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
                    privvar.ContextAwareButtonAction=()=>{};
                    privvar.RoomCodeClearedAction=()=>{};
                    privvar.UserNameClearedAction=()=>{};
                    privvar.RoomCodeEnteredAction=privvar.become.NeedNameToJoin;
                    privvar.UserNameEnteredAction=privvar.become.ReadyToHost;
                },
                NeedNameToJoin(){
                    this.cancelGameButtonHidden=true;
                    this.buttonContext="Join Game";
                    this.buttonDisabled=true;
                    this.gameIdEditable=true;
                    this.userIdEditable=true;
                    privvar.ContextAwareButtonAction=()=>{};
                    privvar.RoomCodeClearedAction=privvar.become.NeedNameAndCode;
                    privvar.RoomCodeEnteredAction=()=>{};
                    privvar.UserNameClearedAction=()=>{};
                    privvar.UserNameEnteredAction=privvar.become.ReadyToJoin;
                },
                ReadyToHost(){
                    this.cancelGameButtonHidden=true;
                    this.buttonContext="Host Game";
                    this.buttonDisabled=false;
                    this.gameIdEditable=true;
                    this.userIdEditable=true;
                    privvar.ContextAwareButtonAction=privvar.contextButtonHandlers.HostNewGameClicked;
                    privvar.RoomCodeClearedAction=()=>{};
                    privvar.RoomCodeEnteredAction=privvar.become.NeedNameToJoin;
                    privvar.UserNameClearedAction=privvar.become.NeedNameAndCode;
                    privvar.UserNameEnteredAction=()=>{};
                },
                ReadyToJoin(){
                    this.cancelGameButtonHidden=true;
                    this.buttonContext="Join Game";
                    this.buttonDisabled=false;
                    this.gameIdEditable=true;
                    this.userIdEditable=true;
                    privvar.ContextAwareButtonAction=privvar.contextButtonHandlers.JoinGameClicked;
                    privvar.RoomCodeClearedAction=privvar.become.ReadyToHost;
                    privvar.RoomCodeEnteredAction=()=>{};
                    privvar.UserNameClearedAction=privvar.become.NeedNameToJoin;
                    privvar.UserNameEnteredAction=()=>{};
                },
                WaitForMorePlayers()
                {
                    this.cancelGameButtonHidden=false;
                    this.buttonContext="Start Game";
                    this.buttonDisabled=true;
                    this.gameIdEditable=false;
                    this.userIdEditable=false;
                    privvar.ContextAwareButtonAction=()=>{};
                    privvar.RoomCodeClearedAction=()=>{};
                    privvar.RoomCodeEnteredAction=()=>{};
                    privvar.UserNameClearedAction=()=>{};
                    privvar.UserNameEnteredAction=()=>{};
                },
                ReadyToStart()
                {
                    this.cancelGameButtonHidden=false;
                    this.buttonContext="Start Game";
                    this.buttonDisabled=false;
                    this.gameIdEditable=false;
                    this.userIdEditable=false;
                    privvar.ContextAwareButtonAction=privvar.contextButtonHandlers.StartGameClicked;
                    privvar.RoomCodeClearedAction=()=>{};
                    privvar.RoomCodeEnteredAction=()=>{};
                    privvar.UserNameClearedAction=()=>{};
                    privvar.UserNameEnteredAction=()=>{};
                },
                WaitForHostToStart()
                {
                    this.cancelGameButtonHidden=true;
                    this.buttonContext="Leave Lobby";
                    this.buttonDisabled=false;
                    this.gameIdEditable=false;
                    this.userIdEditable=false;
                    privvar.ContextAwareButtonAction=privvar.contextButtonHandlers.LeaveLobby;
                    privvar.RoomCodeClearedAction=()=>{};
                    privvar.RoomCodeEnteredAction=()=>{};
                    privvar.UserNameClearedAction=()=>{};
                    privvar.UserNameEnteredAction=()=>{};
                }
            }
        }
        this.__private=privvar;
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
                              ?disabled="${!this.gameIdEditable}"
                              value="${this.gameId}"
                              @keyup="${e=>this.__private.RoomCodeChanged(e.target.value)}"
                              >
                </paper-input>

                <paper-input label="user name" 
                              ?disabled="${!this.userIdEditable}"
                              value="${this.userId}"
                              @keyup="${e=>this.__private.UserNameChanged(e.target.value)}"
                              >
                </paer-input>

                <paper-button ?hidden="${this.cancelGameButtonHidden}"
                               @click=${this.CancelGameButtonClicked}
                               >
                    "Cancel"
                </paper-button>

                <paper-button ?disabled=${this.buttonDisabled}
                               @click=${this.__private.ContextAwareButtonAction}
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