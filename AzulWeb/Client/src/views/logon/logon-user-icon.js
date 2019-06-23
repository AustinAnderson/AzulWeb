import {LitElement,html} from 'lit-element';

class LogonUserIcon extends LitElement
{
    static TagName(){return "logon-user-icon";}
    static get properties(){
        return {
            userId: {type:String},
            iconSrc: {type:String},
            hidden: {type:Boolean}
        }
    }
    constructor()
    {
        super();
        //this.hidden=true;
    }
    render()
    {
        return html`
            <style>
div.card {
    font-size:60px;
    width: 8em;
    height: 1em;
    box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
    display: flex;
    flex-direction:col;
    justify-content: left;
    align-items: center;
    transform: scale(.85)
}
div.cardInner{
    display:flex;
    flex-direction:row;
    height:90%;
}
img{
    border-right: 2px solid lightblue;
    height:85%;
    padding-right:5px;
    padding-left:5px;
}
.text{
    font-family: 'Roboto', 'Noto', sans-serif;
    display:flex;
    align-items: center;
    color: #555;
    border-left: 10px solid transparent;
    font-size:.6em;
}
            </style>
            <div class="card" ?hidden="${this.hidden}">
                <img src="${this.iconSrc}" />
                <div class="cardInner">
                    <span class="text">${this.userId}</span>
                </div>
            <div>
        `;
    }
}
customElements.define(LogonUserIcon.TagName(),LogonUserIcon);