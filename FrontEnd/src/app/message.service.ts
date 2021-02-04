import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

import {Message } from "./message";

@Injectable
({
    providedIn: 'root'
})
  
export class MessageService 
{    
    //DB Strings
    //private connection: string = "http://localhost:3000"; //Mocked DB
    //private connection: string = "http://localhost:44250; //Whatever our real backend is
    private connection: string = "/api";
    //private connection: string = "https://whatsthatsong.azurewebsites.net";

    constructor(private http: HttpClient){}

    getMessage(id: number) : Observable<Message>
    {
        return this.http.get<Message>(this.connection + "/message/" + id);
    }

    getAllMessages(): Observable<Message[]>
    {
        return this.http.get<Message[]>(this.connection + "/message");
    }

    sendMessage(m: Message): Observable<Message>
    {
        return this.http.post<Message>(this.connection + "/user/sendMessage", m);
    }

    getChatMessages(aId: number, bId: number): Observable<any>
    {
        const headers = new HttpHeaders().append('Content-Type', 'application/json');
        const params = new HttpParams().append('loggedInUser', "" + aId).append('UserToMessageId', "" + bId);
        return this.http.get<any>(this.connection + "/user/GoToChat", {headers, params}); 
    }
}