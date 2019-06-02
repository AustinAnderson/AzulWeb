import $ from 'jquery'

class ApiAccess {
    async requestNewGame(){
        return await $.get('/api/MatchMaking/requestNewGame');
    }
    async startNewGame(gameId,connectionIdList){
        return await $.post('/api/MatchMakin/startNewGame/'+gameId,connectionIdList);
    }
}