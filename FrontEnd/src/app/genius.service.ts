import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable
({
    providedIn: 'root'
})
  
export class GeniusService 
{    
    /*
    //Connection Constants
    private authConnection: string = "https://api.genius.com/oauth"
    private clientId: string = "PtPJ6dadTTUzZ-kQTxs5EBhD4hHaA61CvXlmHmHPnYv3uk5-0-KcPf5A7fryJv_d";
    private clientSecret: string = "HolAM9P87DOTzdBVZ0lpCZs4WUJsEmx8iIUtwkV8mV_kgE-wemA_0GAMcZ2kp7_ycOgsErQMKLPu0gaeA4ELzQ";
    private redirectURL: string = "https://inharmony.azurewebsites.net";
*/
    private connection: string = "https://api.genius.com";

    //User Info
    public loggedIn: boolean;
    public access_token: string = "mE2NSmIrqPTDAtUM7SyyG6s2vHe172ei_UkAUxK17kLoVwOTigrz3y8IFSyKd_oT";

    constructor(private http: HttpClient)
    {
        this.loggedIn = false;
    }

    search(query: string): Observable<any>
    {
        const headers = new HttpHeaders().append('Authorization', 'Bearer ' + this.access_token);
        return this.http.get<any>(this.connection + "/search?q=" + encodeURIComponent(query), {headers: headers});
    }

/*
    getAuthUrl(): string
    { 
        return this.authConnection + "/authorize?client_id=" + this.clientId + "&redirect_uri=" + this.redirectURL 
                                                        + "&scope=me&state=A1" + "&response_type=code";
    }
    authStepTwo(code: string): Observable<any>
    {
        console.log(code);

        //const headers = new HttpHeaders().append('Authorization', 'Basic ' + this.authorizationString)
        //                                    .append('Content-Type', 'application/x-www-form-urlencoded');
                                            
        const payload = new HttpParams().set('code', code).set("client_id", this.clientId).set("client_secret", this.clientSecret)
                                            .append('redirect_uri', this.redirectURL).append("response_type", "code")
                                            .set('grant_type', "authorization_code");
        return this.http.post<any>(this.authConnection + "/api/token", payload);
    }
    */
}