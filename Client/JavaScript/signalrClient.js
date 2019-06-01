import {HubConnection} from '@aspnet/signalr'

export class SignalRClient extends HubConnection
{
    /**
     *Creates an instance of SignalRClient.
     * @param {*} gameStartCallBack initialStateModel=>{}
     * @param {*} UpdateCallBack stateChanges=>{} 
     * @param {*} SetPlayerTokenAndIndexCallBack playerTokenAndIndex=>{} 
     * @param {*} JoinedGameCallBack userId=>{} 
     * @memberof SignalRClient
     */
    constructor(
        gameStartCallBack,
        UpdateCallBack,
        SetPlayerTokenAndIndexCallBack,
        JoinedGameCallBack
    ){
        super("/hub");
        this.on("GameStart",gameStartCallBack);
        this.on("Update",UpdateCallBack);
        this.on("SetPlayerTokenAndIndex",SetPlayerTokenAndIndexCallBack);
        this.on("JoinedGame",JoinedGameCallBack);
    }

    /**
     * @param {string} userId
     * @param {string} gameId
     * @returns {Promise<any>}
     * @memberof SignalRClient
     */
    NotifyServerGameJoined(userId,gameId)
    {
        return this.invoke("RebroadCastJoinedGame",[userId,gameId]);
    }
}