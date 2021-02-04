import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

import { Song } from "./song";

@Injectable
({
    providedIn: 'root'
})
  
export class SongService 
{    
    //DB Strings
    //private connection: string = "http://localhost:3000"; //Mocked DB
    //private connection: string = "http://localhost:44250; //Whatever our real backend is
    private connection: string = "/api";
    //private connection: string = "https://whatsthatsong.azurewebsites.net";

    constructor(private http: HttpClient){}

    getSong(id: number) : Observable<Song>
    {
        const headers = new HttpHeaders().append('Content-Type', 'application/json');
        const params = new HttpParams().append('id', "" + id);
        return this.http.get<Song>(this.connection + "/Song/getSong", {headers, params});
    }

    getHomepageSongs(): Observable<Song[]>
    {
        return this.http.get<Song[]>(this.connection + "/Song/getTop5Originals");
    }

    uploadSong(s: Song): Observable<void>
    {
        return this.http.post<void>(this.connection + "/Song/uploadSong", s);
    }

    getUserSongs(uId: number): Observable<Song[]>
    {
        const headers = new HttpHeaders().append('Content-Type', 'application/json');
        const params = new HttpParams().append('id', "" + uId);
        return this.http.get<Song[]>(this.connection + "/Song/GetAllSongsByACertainUser", {headers, params});
    }

    deleteMySong(id: number): Observable<void>
    {
        return this.http.delete<void>(this.connection + "/Song/DeleteUploadedSong?songid=" + id);
    }

    incrementPlays(id: number): Observable<void>
    {
        const headers = new HttpHeaders().append('Content-Type', 'application/json');
        const params = new HttpParams().append('songid', "" + id);
        return this.http.get<void>(this.connection + "/Song/incrementNumPlays", {headers, params});   
    }

    addToFavourites(sId: number, uId: number): Observable<void>
    {
        const headers = new HttpHeaders().append('Content-Type', 'application/json');
        const params = new HttpParams().append('songid', "" + sId).append('userId', "" + uId);
        return this.http.get<void>(this.connection + "/Song/addSongToFavorites", {headers, params});   
    }

    getFavourites(id: number): Observable<Song[]>
    {
        const headers = new HttpHeaders().append('Content-Type', 'application/json');
        const params = new HttpParams().append('Id', "" + id);
        return this.http.get<Song[]>(this.connection + "/Song/GetUsersFavoriteSongs", {headers, params});  
    }

    getTopFavourites(id: number): Observable<Song[]>
    {
        const headers = new HttpHeaders().append('Content-Type', 'application/json');
        const params = new HttpParams().append('Id', "" + id);
        return this.http.get<Song[]>(this.connection + "/Song/Get5FavoriteSongsForUser", {headers, params});  
    }

    isFavourite(sId: number, uId: number): Observable<boolean>
    {
        const headers = new HttpHeaders().append('Content-Type', 'application/json');
        const params = new HttpParams().append('songId', "" + sId).append('userId', "" + uId);
        return this.http.get<boolean>(this.connection + "/Song/isSongAlreadyAFavorite", {headers, params});   
    }

    deleteFavourite(sId: number, uId: number): Observable<void>
    {
       return this.http.delete<void>(this.connection + "/Song/DeleteSongFromFavorites?userId=" + uId + "&songid=" + sId);
    }

    searchOriginalsByLyrics(query: string): Observable<Song[]>
    {
        const headers = new HttpHeaders().append('Content-Type', 'application/json');
        const params = new HttpParams().append('phrase', query);
        return this.http.get<Song[]>(this.connection + "/Song/getOriginalsongsByLyrics", {headers, params});   
    }

    songExists(title: string, artist: string): Observable<Song>
    {
        const headers = new HttpHeaders().append('Content-Type', 'application/json');
        const params = new HttpParams().append('artistName', artist).append('title', title);
        return this.http.get<Song>(this.connection + "/Song/IsSongInDb", {headers, params});   
    }
}