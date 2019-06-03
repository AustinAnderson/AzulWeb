import {LitElement,html} from 'lit-element';
import '@polymer/paper-button/paper-button.js';
import '@polymer/paper-input/paper-input.js';

export class LogonPage extends LitElement
{
    static TagName(){ return "longon-page"; }
    static AppendNewTo(domParent) {
        console.log(domParent);
        var tag=document.createElement(LogonPage.TagName());
        tag.hidden=true;
        domParent.appendChild(tag);
        return tag;
    }
    constructor(){
        console.log("logon constructor run");
        /**
         * intended to be overwritten with a call back for making the server cancel the game
         * @param {string} cancelledGameId the id of the game that was cancelled
         * 
         * @memberof LogonButtonsAndInput
         */
        this.OnGameCancelled=(cancelledGameId) =>{
            alert(LogonPage.name+"."+OnGameCancelled.name+" never assigned!");
        }
        /**
         * intended to be overwritten with a call back for making the server start the game
         * and the outer class to switch the view to the main game view
         * 
         * @memberof LogonButtonsAndInput
         */
        this.OnGameStart=()=>{
            alert(LogonPage.name+"."+OnGameStart.name+" never assigned!");
        }
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
        var _private={
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
            RoomCodeChanged(){
                if(gameId==""){ _private.RoomCodeClearedAction();}
                else { _private.RoomCodeEnteredAction(); }
            },
            UserNameChanged(){
                if(userId==""){ _private.UserNameClearedAction();}
                else { _private.UserNameEnteredAction(); }
            },
            CancelGameButtonClicked(){
                OnGameCancelled(_private.gameId);
                RoomTornDown();
            },
            contextButtonHandlers:{
                HostNewGameClicked(){
                    _private.gameId=OnHostGameClicked();
                    _private.become.WaitForMorePlayers();
                },
                JoinGameClicked(){
                    _private.become.WaitForHostToStart();
                },
                LeaveLobby(){
                    _private.become.ReadyToJoin();
                },
                StartGameClicked(){
                    OnGameStart();
                }
            },
            become:{
                NeedNameAndCode(){
                    _private.cancelGameButtonHidden=true;
                    _private.buttonContext="Host Game";
                    _private.buttonDisabled=true;
                    _private.gameIdEditable=true;
                    _private.userIdEditable=true;
                    _private.ContextAwareButtonAction=()=>{};
                    _private.RoomCodeClearedAction=()=>{};
                    _private.UserNameClearedAction=()=>{};
                    _private.RoomCodeEnteredAction=_private.become.NeedNameToJoin;
                    _private.UserNameEnteredAction=_private.become.ReadyToHost;
                },
                NeedNameToJoin(){
                    _private.cancelGameButtonHidden=true;
                    _private.buttonContext="Join Game";
                    _private.buttonDisabled=true;
                    _private.ContextAwareButtonAction=()=>{};
                    _private.gameIdEditable=true;
                    _private.userIdEditable=true;
                    _private.RoomCodeClearedAction=_private.become.NeedNameAndCode;
                    _private.RoomCodeEnteredAction=()=>{};
                    _private.UserNameClearedAction=()=>{};
                    _private.UserNameEnteredAction=_private.become.ReadyToJoin;
                },
                ReadyToHost(){
                    _private.cancelGameButtonHidden=true;
                    _private.buttonContext="Host Game";
                    _private.buttonDisabled=false;
                    _private.ContextAwareButtonAction=_private.contextButtonHandlers.HostNewGameClicked;
                    _private.gameIdEditable=true;
                    _private.userIdEditable=true;
                    _private.RoomCodeClearedAction=()=>{};
                    _private.RoomCodeEnteredAction=_private.become.NeedNameToJoin;
                    _private.UserNameClearedAction=_private.become.NeedNameAndCode;
                    _private.UserNameEnteredAction=()=>{};
                },
                ReadyToJoin(){
                    _private.cancelGameButtonHidden=true;
                    _private.buttonContext="Join Game";
                    _private.buttonDisabled=false;
                    _private.ContextAwareButtonAction=_private.contextButtonHandlers.JoinGameClicked;
                    _private.gameIdEditable=true;
                    _private.userIdEditable=true;
                    _private.RoomCodeClearedAction=_private.become.ReadyToHost;
                    _private.RoomCodeEnteredAction=()=>{};
                    _private.UserNameClearedAction=_private.become.NeedNameToJoin;
                    _private.UserNameEnteredAction=()=>{};
                },
                WaitForMorePlayers()
                {
                    _private.cancelGameButtonHidden=false;
                    _private.buttonContext="Start Game";
                    _private.buttonDisabled=true;
                    _private.ContextAwareButtonAction=()=>{};
                    _private.gameIdEditable=false;
                    _private.userIdEditable=false;
                    _private.RoomCodeClearedAction=()=>{};
                    _private.RoomCodeEnteredAction=()=>{};
                    _private.UserNameClearedAction=()=>{};
                    _private.UserNameEnteredAction=()=>{};
                },
                ReadyToStart()
                {
                    _private.cancelGameButtonHidden=false;
                    _private.buttonContext="Start Game";
                    _private.buttonDisabled=false;
                    _private.ContextAwareButtonAction=_private.contextButtonHandlers.StartGameClicked;
                    _private.gameIdEditable=false;
                    _private.userIdEditable=false;
                    _private.RoomCodeClearedAction=()=>{};
                    _private.RoomCodeEnteredAction=()=>{};
                    _private.UserNameClearedAction=()=>{};
                    _private.UserNameEnteredAction=()=>{};
                },
                WaitForHostToStart()
                {
                    _private.cancelGameButtonHidden=true;
                    _private.buttonContext="Leave Lobby";
                    _private.buttonDisabled=false;
                    _private.ContextAwareButtonAction=_private.contextButtonHandlers.LeaveLobby;
                    _private.gameIdEditable=false;
                    _private.userIdEditable=false;
                    _private.RoomCodeClearedAction=()=>{};
                    _private.RoomCodeEnteredAction=()=>{};
                    _private.UserNameClearedAction=()=>{};
                    _private.UserNameEnteredAction=()=>{};
                }
            }
        }
    }

    UserAdded(connectionId,userId,playerIconSrc)
    {
        document.querySelector("div")
        if(playerCount>=2){
            _private.become.ReadyToStart();
        }
    }
    UserRemoved(userId){

    }
    /**
     * to be called by the user when a game session is cancelled by the host
     * @memberof LogonButtonsAndInput
     */
    RoomTornDown(){
        _private.RemoveAllUsers();
        _private.gameId="";//will this fire onchange?
        _private.become.ReadyToHost();
    }

    
    render(){
        return html`
            <div>
                <papper-input label="room code" 
                              disabled={{_private.gameIdEditable}} 
                              value={{_private.gameId}}
                              >
                </papper-input>

                <papper-input label="user name" 
                              disabled={{_private.userIdEditable}}
                              value={{_private.userId}}
                              >
                </papper-input>

                <papper-button hidden={{_private.cancelGameButtonHidden}}
                               on-click={{_private.CancelGameButtonClicked}}
                               >
                    "Cancel"
                </papper-button>

                <papper-button disabled={{_private.buttonDisabled}}
                               on-click={{_private.ContextAwareButtonAction}}
                               >
                    {{_private.buttonContext}}
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