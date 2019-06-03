import {HubConnectionBuilder,LogLevel} from '@aspnet/signalr'

export class SignalRClient
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
        var hub=new HubConnectionBuilder()
                    .withUrl("/hub")
                    .configureLogging(LogLevel.Information)
                    .build();
        hub.on("GameStart",gameStartCallBack);
        hub.on("Update",UpdateCallBack);
        hub.on("SetPlayerTokenAndIndex",SetPlayerTokenAndIndexCallBack);
        hub.on("JoinedGame",JoinedGameCallBack);
    }

    /**
     * @param {string} userId
     * @param {string} gameId
     * @returns {Promise<any>}
     * @memberof SignalRClient
     */
    NotifyServerGameJoined(userId,gameId)
    {
        return hub.invoke("RebroadCastJoinedGame",[userId,gameId]);
    }
}