import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from "rxjs";

import { FriendList } from "./friendRequest";

@Injectable
({
    providedIn: 'root'
})
  
export class FriendService 
{
    //DB Strings
    //private connection: string = "http://localhost:3000"; //Mocked DB
    //private connection: string = "http://localhost:44250; //Whatever our real backend is
    private connection: string = "/api";
    //private connection: string = "https://whatsthatsong.azurewebsites.net";

    constructor(private http: HttpClient){}

    //HTTP Methods
    getFriendList(id: number): Observable<FriendList[]>
	{
        const headers = new HttpHeaders().append('Content-Type', 'application/json');
        const params = new HttpParams().append('id', "" + id);
		return this.http.get<FriendList[]>(this.connection + "/user/GetFriends", {headers, params});
    }

    checkFriends(idA: number, idB: number): Observable<boolean>
    {
        const headers = new HttpHeaders().append('Content-Type', 'application/json');
        const params = new HttpParams().append('userId', "" + idA).append('friendId', "" + idB);
        return this.http.get<boolean>(this.connection + "/user/AreWeFriends", {headers, params}); 
    }

    requestFriend(fr: FriendList): Observable<FriendList>
    {
        return this.http.post<FriendList>(this.connection + "/user/RequestFriend", fr);
    }

    deleteFriend(idA: number, idB: number): Observable<any>
    {
        return this.http.delete<any>(this.connection + "/user/DeleteFriend?LoggedInUserId=" + idA + "&friendToDeleteId=" + idB);
    }

    getAllFriendRequests(uId: number): Observable<FriendList[]>
    {
        const headers = new HttpHeaders().append('Content-Type', 'application/json');
        const params = new HttpParams().append('UserId', "" + uId);
        return this.http.get<any>(this.connection + "/user/DisplayAllFriendRequests", {headers, params}); 
    }

    editFriendRequest(fr: FriendList): Observable<FriendList>
    {
        return this.http.put<FriendList>(this.connection + "/user/EditFriendStatus", fr);
    }

    //Helper Methods
    setPending(fr: FriendList): void {fr.status = "pending";}
    setAccepted(fr: FriendList): void {fr.status = "accept";}
    setRejected(fr: FriendList): void {fr.status = "rejected";}
    setDeleted(fr: FriendList): void {fr.status = "deleted";}
    isPending(fr: FriendList): boolean {return fr.status == "pending";}
    isAccepted(fr: FriendList): boolean {return fr.status == "accept";}
    isRejected(fr: FriendList): boolean {return fr.status == "rejected";}
    isDeleted(fr: FriendList): boolean {return fr.status == "deleted";}
}