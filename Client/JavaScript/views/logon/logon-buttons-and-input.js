import {LitElement,html} from 'lit-element';
import '@polymer/paper-button/paper-button.js';
import '@polymer/paper-button/paper-input.js';

class LogonButtonsAndInput extends LitElement
{
    constructor(){
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

            RoomCodeChanged(){
                if(gameId==""){ private.RoomCodeClearedAction();}
                else { private.RoomCodeEnteredAction(); }
            },
            UserNameChanged(){
                if(userId==""){ private.UserNameClearedAction();}
                else { private.UserNameEnteredAction(); }
            },
            RemoveAllUsers(){

            },
            CancelGameButtonClicked(){

            },
            contextButtonHandlers={
                HostNewGameClicked(){
                    private.gameId=OnHostGameClicked();
                    private.become.WaitForMorePlayers();
                },
                JoinGameClicked(){
                    private.become.WaitForHostToStart();
                },
                StartGameClicked(){

                },
                LeaveLobby(){
                    private.become.ReadyToJoin();
                }
            },
            become={
                NeedNameAndCode(){
                    RemoveAllUsers();
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
                    RemoveAllUsers();
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
                    RemoveAllUsers();
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
                    RemoveAllUsers();
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

    UserAdded(userId,playerIconSrc)
    {
        document.querySelector("div")
    }
    UserRemoved(userId){

    }

    /**
     * intented to be overwritten with a call back for retrieving the new
     * game code from the server
     * @returns {string} server created game code
     * @memberof LogonButtonsAndInput
     */
    OnHostGameClicked=()=>{
        alert("LogonButtonsAndInput.OnHostGameClicked never assigned!");
        return "";
    }
    render(){
        html`
            <div>
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
customElements.define("logon-buttons-and-input",LogonButtonsAndInput);