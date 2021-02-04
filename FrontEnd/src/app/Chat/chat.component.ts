import { Component, ElementRef, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';

import { User } from '../user';
import { Message } from '../message';
import { MessageService } from '../message.service';
import { LoginService } from '../login.service';


@Component
({
    selector: 'app-chat',
    templateUrl: './chat.component.html',
    styleUrls: ['../app.component.css', './chat.component.css']
})

export class ChatComponent implements OnInit 
{
    public aId: number;
    public bId: number;
    public messages: Array<Message> = new Array<Message>();
    public formdata: FormGroup;

    constructor(public loginService: LoginService, public messageService: MessageService, private route: ActivatedRoute, private elementRef: ElementRef)
    {
        //ONE OF THE USERS SHOULD BE LOGGEDINUSER
        route.params.subscribe
        (
            params => 
            {
                    if(params.aId)  this.aId = params["aId"];
                    else            alert("Error");
                    if(params.bId)  this.bId = params["bId"];
                    else            alert("Error");
            }
        );

        messageService.getChatMessages(this.aId, this.bId).subscribe
        (
            (data) => 
            {
                this.messages = data["messages"]; 
                let host = this.elementRef.nativeElement.querySelector('.host');
                
                for(let x = 0; x < data["messages"].length; x++)
                {
                    if((data["messages"][x].toUserId == this.aId && data["messages"][x].fromUserId == this.bId))
                        host.insertAdjacentHTML("beforeend", "<p style='text-align: left; margin: 10px;'> " 
                                                    + data["messages"][x].fromUserName + " says: " + data["messages"][x].content + " </p>");
                    else if((data["messages"][x].toUserId == this.bId) && (data["messages"][x].fromUserId == this.aId ))
                        host.insertAdjacentHTML("beforeend", "<p style='text-align: right; margin: 10px;'> " 
                                                    + data["messages"][x].fromUserName + " says: " + data["messages"][x].content + " </p>");
                }
            },
            () => alert("Error")
        );
    }
  
    ngOnInit(): void 
    {
        this.formdata = new FormGroup({ message: new FormControl("", Validators.required) });	
    }

    sendMessage(message: string): void
    {
        if(this.loginService.loggedIn)
        {
            this.messageService.sendMessage(new Message(this.aId, this.bId, message["message"], this.loginService.loggedInUser.userName)).subscribe
            (
                () => 
                {
                    let host = this.elementRef.nativeElement.querySelector('.host');
                    host.insertAdjacentHTML("beforeend", "<p> " + message["message"] + " </p>");
                    this.formdata = new FormGroup({ message: new FormControl("", Validators.required) });	
                },
                (error) => alert(error)
            );
        }
        else    alert("You must be logged in to message!");

    }
}
