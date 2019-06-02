import {LitElement,html} from 'lit-element';

class LogonUserIcon extends LitElement
{
    static TagName(){return "logon-user-icon";}
    constructor()
    {
        var private={
            hidden=true,
            playerIconSource="",
            userId=""
        }
    }
    isAdded(){return !private.hidden;}
    addUser(playerIconSrc,userId)
    {
        private.playerIconSource=playerIconSrc;
        private.userId=userId;
        private.hidden=false;
    }
    removeUser(){
        private.hidden=true;
        private.playerIconSource="";
        private.userId="";
    }
    render()
    {
        html`
            <div class="${TagName()}" hidden={{private.hidden}}>
                <img src={{private.playerIconSource}} />
                <p>{{private.userId}}</p>
            <div>
        `;
    }
}
customElements.define(LogonUserIcon.TagName(),LogonUserIcon);