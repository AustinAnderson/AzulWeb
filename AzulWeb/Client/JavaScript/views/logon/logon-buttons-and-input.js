import {LitElement,html} from 'lit-element';
import '@polymer/paper-button/paper-button.js';
import '@polymer/paper-button/paper-input.js';

class LogonPage extends LitElement
{
    static TagName(){ return "longon-page"; }
    static AppendNewTo(domParent) {
        var tag=document.createElement(LogonPage.TagName());
        domParent.appendChild(tag);
        return tag;
    }
    constructor(){
        this.hidden=true;
        var private={
            gameId: "",
            userId: "",
            gameIdEditable: true,
            userIdEditable: true,
            buttonContext="Host Game",
            buttonDisabled=true,
            cancelGameButtonHidden=true,
            RoomCodeEnteredAction:()=>{},
            RoomCodeClearedAction:()=>{},
            UserNameEnteredAction:()=>{},
            UserNameClearedAction:()=>{},
            ContextAwareButtonAction:()=>{},
            RemoveAllUsers(){

            },
            RoomCodeChanged(){
                if(gameId==""){ private.RoomCodeClearedAction();}
                else { private.RoomCodeEnteredAction(); }
            },
            UserNameChanged(){
                if(userId==""){ private.UserNameClearedAction();}
                else { private.UserNameEnteredAction(); }
            },
            CancelGameButtonClicked(){
                OnGameCancelled(private.gameId);
                RoomTornDown();
            },
            contextButtonHandlers={
                HostNewGameClicked(){
                    private.gameId=OnHostGameClicked();
                    private.become.WaitForMorePlayers();
                },
                JoinGameClicked(){
                    private.become.WaitForHostToStart();
                },
                LeaveLobby(){
                    private.become.ReadyToJoin();
                },
                StartGameClicked(){
                    OnGameStart();
                }
            },
            become={
                NeedNameAndCode(){
                    private.cancelGameButtonHidden=true;
                    private.buttonContext="Host Game";
                    private.buttonDisabled=true;
                    private.gameIdEditable=true;
                    private.userIdEditable=true;
                    private.ContextAwareButtonAction=()=>{};
                    private.RoomCodeClearedAction=()=>{};
                    private.UserNameClearedAction=()=>{};
                    private.RoomCodeEnteredAction=private.become.NeedNameToJoin;
                    private.UserNameEnteredAction=private.become.ReadyToHost;
                },
                NeedNameToJoin(){
                    private.cancelGameButtonHidden=true;
                    private.buttonContext="Join Game";
                    private.buttonDisabled=true;
                    private.ContextAwareButtonAction=()=>{};
                    private.gameIdEditable=true;
                    private.userIdEditable=true;
                    private.RoomCodeClearedAction=private.become.NeedNameAndCode;
                    private.RoomCodeEnteredAction=()=>{};
                    private.UserNameClearedAction=()=>{};
                    private.UserNameEnteredAction=private.become.ReadyToJoin;
                },
                ReadyToHost(){
                    private.cancelGameButtonHidden=true;
                    private.buttonContext="Host Game";
                    private.buttonDisabled=false;
                    private.ContextAwareButtonAction=private.contextButtonHandlers.HostNewGameClicked;
                    private.gameIdEditable=true;
                    private.userIdEditable=true;
                    private.RoomCodeClearedAction=()=>{};
                    private.RoomCodeEnteredAction=private.become.NeedNameToJoin;
                    private.UserNameClearedAction=private.become.NeedNameAndCode;
                    private.UserNameEnteredAction=()=>{};
                },
                ReadyToJoin(){
                    private.cancelGameButtonHidden=true;
                    private.buttonContext="Join Game";
                    private.buttonDisabled=false;
                    private.ContextAwareButtonAction=private.contextButtonHandlers.JoinGameClicked;
                    private.gameIdEditable=true;
                    private.userIdEditable=true;
                    private.RoomCodeClearedAction=private.become.ReadyToHost;
                    private.RoomCodeEnteredAction=()=>{};
                    private.UserNameClearedAction=private.become.NeedNameToJoin;
                    private.UserNameEnteredAction=()=>{};
                },
                WaitForMorePlayers()
                {
                    private.cancelGameButtonHidden=false;
                    private.buttonContext="Start Game";
                    private.buttonDisabled=true;
                    private.ContextAwareButtonAction=()=>{};
                    private.gameIdEditable=false;
                    private.userIdEditable=false;
                    private.RoomCodeClearedAction=()=>{};
                    private.RoomCodeEnteredAction=()=>{};
                    private.UserNameClearedAction=()=>{};
                    private.UserNameEnteredAction=()=>{};
                },
                ReadyToStart()
                {
                    private.cancelGameButtonHidden=false;
                    private.buttonContext="Start Game";
                    private.buttonDisabled=false;
                    private.ContextAwareButtonAction=private.contextButtonHandlers.StartGameClicked;
                    private.gameIdEditable=false;
                    private.userIdEditable=false;
                    private.RoomCodeClearedAction=()=>{};
                    private.RoomCodeEnteredAction=()=>{};
                    private.UserNameClearedAction=()=>{};
                    private.UserNameEnteredAction=()=>{};
                },
                WaitForHostToStart()
                {
                    private.cancelGameButtonHidden=true;
                    private.buttonContext="Leave Lobby";
                    private.buttonDisabled=false;
                    private.ContextAwareButtonAction=private.contextButtonHandlers.LeaveLobby;
                    private.gameIdEditable=false;
                    private.userIdEditable=false;
                    private.RoomCodeClearedAction=()=>{};
                    private.RoomCodeEnteredAction=()=>{};
                    private.UserNameClearedAction=()=>{};
                    private.UserNameEnteredAction=()=>{};
                }
            }
        }
    }

    UserAdded(connectionId,userId,playerIconSrc)
    {
        document.querySelector("div")
        if(playerCount>=2){
            private.become.ReadyToStart();
        }
    }
    UserRemoved(userId){

    }
    /**
     * to be called by the user when a game session is cancelled by the host
     * @memberof LogonButtonsAndInput
     */
    RoomTornDown(){
        private.RemoveAllUsers();
        private.gameId="";//will this fire onchange?
        private.become.ReadyToHost();
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
    render(){
        html`
            <div hidden={{Hidden}}>
                <papper-input label="room code" 
                              disabled={{private.gameIdEditable}} 
                              value={{private.gameId}}
                              >
                </papper-input>

                <papper-input label="user name" 
                              disabled={{private.userIdEditable}}
                              value={{private.userId}}
                              >
                </papper-input>

                <papper-button hidden={{private.cancelGameButtonHidden}}
                               on-click={{private.CancelGameButtonClicked}}
                               >
                    "Cancel"
                </papper-button>

                <papper-button disabled={{private.buttonDisabled}}
                               on-click={{private.ContextAwareButtonAction}}
                               >
                    {{private.buttonContext}}
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