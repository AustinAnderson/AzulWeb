import {LitElement,html} from 'lit-element';
import '@polymer/paper-button/paper-button.js';
import '@polymer/paper-input/paper-input.js';

export class LogonPage extends LitElement
{
    static TagName(){ return "longon-page"; }
    static get properties(){
        return {
            this._private: {type:Object},
        };
    }
    /**
     * intended to be overwritten with a call back for making the server cancel the game
     * @param {string} cancelledGameId the id of the game that was cancelled
     * 
     * @memberof LogonButtonsAndInput
     */
    OnGameCancelled=(cancelledGameId) =>{
        alert(LogonPage.name+"."+OnGameCancelled.name+" never assigned!");
    }
    /**
     * intended to be overwritten with a call back for making the server start the game
     * and the outer class to switch the view to the main game view
     * 
     * @memberof LogonButtonsAndInput
     */
    OnGameStart=()=>{
        alert(LogonPage.name+"."+OnGameStart.name+" never assigned!");
    }
    /**
     * intended to be overwritten with a call back for retrieving the new
     * game code from the server
     * @returns {string} server created game code
     * @memberof LogonButtonsAndInput
     */
    OnHostGameClicked=()=>{
        alert(LogonPage.name+"."+OnHostGameClicked.name+" never assigned!");
        return "";
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
        
        this._private={
            gameId: "",
            userId: "",
            gameIdEditable: true,
            userIdEditable: true,
            buttonContext:"Host Game",
            buttonDisabled:true,
            cancelGameButtonHidden:true,
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
                    this._private.cancelGameButtonHidden=true;
                    this._private.buttonContext="Host Game";
                    this._private.buttonDisabled=true;
                    this._private.gameIdEditable=true;
                    this._private.userIdEditable=true;
                    this._private.ContextAwareButtonAction=()=>{};
                    this._private.RoomCodeClearedAction=()=>{};
                    this._private.UserNameClearedAction=()=>{};
                    this._private.RoomCodeEnteredAction=this._private.become.NeedNameToJoin;
                    this._private.UserNameEnteredAction=this._private.become.ReadyToHost;
                },
                NeedNameToJoin(){
                    this._private.cancelGameButtonHidden=true;
                    this._private.buttonContext="Join Game";
                    this._private.buttonDisabled=true;
                    this._private.ContextAwareButtonAction=()=>{};
                    this._private.gameIdEditable=true;
                    this._private.userIdEditable=true;
                    this._private.RoomCodeClearedAction=this._private.become.NeedNameAndCode;
                    this._private.RoomCodeEnteredAction=()=>{};
                    this._private.UserNameClearedAction=()=>{};
                    this._private.UserNameEnteredAction=this._private.become.ReadyToJoin;
                },
                ReadyToHost(){
                    this._private.cancelGameButtonHidden=true;
                    this._private.buttonContext="Host Game";
                    this._private.buttonDisabled=false;
                    this._private.ContextAwareButtonAction=this._private.contextButtonHandlers.HostNewGameClicked;
                    this._private.gameIdEditable=true;
                    this._private.userIdEditable=true;
                    this._private.RoomCodeClearedAction=()=>{};
                    this._private.RoomCodeEnteredAction=this._private.become.NeedNameToJoin;
                    this._private.UserNameClearedAction=this._private.become.NeedNameAndCode;
                    this._private.UserNameEnteredAction=()=>{};
                },
                ReadyToJoin(){
                    this._private.cancelGameButtonHidden=true;
                    this._private.buttonContext="Join Game";
                    this._private.buttonDisabled=false;
                    this._private.ContextAwareButtonAction=this._private.contextButtonHandlers.JoinGameClicked;
                    this._private.gameIdEditable=true;
                    this._private.userIdEditable=true;
                    this._private.RoomCodeClearedAction=this._private.become.ReadyToHost;
                    this._private.RoomCodeEnteredAction=()=>{};
                    this._private.UserNameClearedAction=this._private.become.NeedNameToJoin;
                    this._private.UserNameEnteredAction=()=>{};
                },
                WaitForMorePlayers()
                {
                    this._private.cancelGameButtonHidden=false;
                    this._private.buttonContext="Start Game";
                    this._private.buttonDisabled=true;
                    this._private.ContextAwareButtonAction=()=>{};
                    this._private.gameIdEditable=false;
                    this._private.userIdEditable=false;
                    this._private.RoomCodeClearedAction=()=>{};
                    this._private.RoomCodeEnteredAction=()=>{};
                    this._private.UserNameClearedAction=()=>{};
                    this._private.UserNameEnteredAction=()=>{};
                },
                ReadyToStart()
                {
                    this._private.cancelGameButtonHidden=false;
                    this._private.buttonContext="Start Game";
                    this._private.buttonDisabled=false;
                    this._private.ContextAwareButtonAction=this._private.contextButtonHandlers.StartGameClicked;
                    this._private.gameIdEditable=false;
                    this._private.userIdEditable=false;
                    this._private.RoomCodeClearedAction=()=>{};
                    this._private.RoomCodeEnteredAction=()=>{};
                    this._private.UserNameClearedAction=()=>{};
                    this._private.UserNameEnteredAction=()=>{};
                },
                WaitForHostToStart()
                {
                    this._private.cancelGameButtonHidden=true;
                    this._private.buttonContext="Leave Lobby";
                    this._private.buttonDisabled=false;
                    this._private.ContextAwareButtonAction=this._private.contextButtonHandlers.LeaveLobby;
                    this._private.gameIdEditable=false;
                    this._private.userIdEditable=false;
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
        this._private.gameId="";//will this fire onchange?
        this._private.become.ReadyToHost();
    }

    
    render(){
        return html`
            <div>
                <papper-input label="room code" 
                              disabled="${this._private.gameIdEditable}"
                              value="${this._private.gameId}"
                              @change="${this.RoomCodeChanged}"
                              >
                </papper-input>

                <papper-input label="user name" 
                              disabled="${this._private.userIdEditable}"
                              value="${this._private.userId}"
                              @change="${this.UserNameChanged}"
                              >
                </papper-input>

                <papper-button ?hidden="${this._private.cancelGameButtonHidden}"
                               @click="${this._private.CancelGameButtonClicked}"
                               >
                    "Cancel"
                </papper-button>

                <papper-button disabled="${this._private.buttonDisabled}"
                               @click="${this._private.ContextAwareButtonAction}"
                               >
                    ${this._private.buttonContext}
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