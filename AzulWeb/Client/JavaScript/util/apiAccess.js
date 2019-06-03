import $ from 'jquery'

export class ApiAccess {
    constructor(){}
    requestNewGame(){
        return $.get('/api/MatchMaking/requestNewGame');
    }
    startNewGame(gameId,connectionIdList){
        return $.post('/api/MatchMakin/startNewGame/'+gameId,connectionIdList);
    }
}