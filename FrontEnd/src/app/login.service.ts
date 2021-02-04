import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from "rxjs";

import { User } from "./user";

@Injectable
({
    providedIn: 'root'
})
  
export class LoginService 
{
    public loggedIn: boolean = false;
    public loggedInUser: User = null;

    //DB Strings
    //private connection: string = "http://localhost:3000"; //Mocked DB
    //private connection: string = "http://localhost:65418"; //Whatever our real backend is
    private connection: string = "/api";
    //private connection: string = "https://whatsthatsong.azurewebsites.net";

    constructor(private http: HttpClient)
    {
        if(!this.loggedIn)
        {
            let json = localStorage.getItem('loggedInUser');
            if(json != null)
            {
                this.loggedInUser = JSON.parse(json);
                this.loggedIn = true;
            }
        }
    }

    loginLocal(u: User)
    {
        this.loggedInUser = u;
        this.loggedIn = true;
        localStorage.setItem('loggedInUser', JSON.stringify(u))
    }

    logoutLocal()
    {
        this.loggedIn = false;
        localStorage.removeItem('loggedInUser');
    }


    //HTTP Methods
    getUser(id: number): Observable<User>
	{
        const headers = new HttpHeaders().append('Content-Type', 'application/json');
        const params = new HttpParams().append('id', "" + id);
		return this.http.get<User>(this.connection + "/user/getUserByIdaAync", {headers, params});
    }

    getUserByUsername(name: string): Observable<User>
    {
        const headers = new HttpHeaders().append('Content-Type', 'application/json');
        const params = new HttpParams().append('userName', "" + name);
		return this.http.get<User>(this.connection + "/user/GetUserByName", {headers, params});
    }

    getValidUsers(): Observable<User[]>
	{
		return this.http.get<User[]>(this.connection + "/user/getAllUsers");
    }
    
    register(user: User): Observable<User>
	{
		return this.http.post<User>(this.connection + "/user/CreateUser", user);
    }

    login(user: User): Observable<User>
    {
        const headers = new HttpHeaders().append('Content-Type', 'application/json');
        const params = new HttpParams().append('username', user.userName).append('password', user.password);
        return this.http.get<User>(this.connection + "/user/login", {headers, params});
    }
    
    editUser(id: number, user: User): Observable<User[]>
    {
        return this.http.put<User[]>(this.connection + "/user/SaveEdit", user);
    }

    searchUsers(searchString: string): Observable<User[]>
    {
        const headers = new HttpHeaders().append('Content-Type', 'application/json');
        const params = new HttpParams().append('searchString', searchString);
        return this.http.get<User[]>(this.connection + "/user/SearchForUsers", {headers, params});
    }

    getFriends(id: number): Observable<User[]>
    {
        const headers = new HttpHeaders().append('Content-Type', 'application/json');
        const params = new HttpParams().append('id', "" + id);
        return this.http.get<User[]>(this.connection + "/user/GetFriendsAsUsers", {headers, params});
    }

    //Not Working
    test(): Observable<any>
    {
        return this.http.get<any>(this.connection + "/user/test");
    }
}