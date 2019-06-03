import {LitElement,html} from 'lit-element';

class LogonUserIcon extends LitElement
{
    static TagName(){return "logon-user-icon";}
    constructor()
    {
        super();
        var _private={
            hidden=true,
            playerIconSource="",
            userId=""
        }
    }
    isAdded(){return !private.hidden;}
    addUser(playerIconSrc,userId)
    {
        _private.playerIconSource=playerIconSrc;
        _private.userId=userId;
        _private.hidden=false;
    }
    removeUser(){
        _private.hidden=true;
        _private.playerIconSource="";
        _private.userId="";
    }
    render()
    {
        return html`
            <div class="${TagName()}" hidden={{private.hidden}}>
                <img src={{private.playerIconSource}} />
                <p>{{private.userId}}</p>
            <div>
        `;
    }
}
customElements.define(LogonUserIcon.TagName(),LogonUserIcon);