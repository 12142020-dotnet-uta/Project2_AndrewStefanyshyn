import { Component, Input, OnInit } from '@angular/core';
import { DomSanitizer, SafeResourceUrl, SafeUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { LoginService } from '../login.service';
import { Song } from '../song';
import { SongService } from '../song.service';
import { SpotifyService } from '../spotify.service';

@Component
({
    selector: 'app-song-summary',
    templateUrl: './song-summary.component.html',
    styleUrls: ['../app.component.css', './song-summary.component.css']
})

export class SongSummaryComponent implements OnInit 
{
    @Input() song: Song;
    public lyrics: boolean;
    public isFavourite: boolean = false;

    constructor(public loginService: LoginService, public songService: SongService, public spotifyService: SpotifyService,
                    private router: Router, public sanitizer: DomSanitizer){}
  
    ngOnInit(): void 
    {
        if(this.song)
        {
            this.checkFavourite();
            this.saveToDb();
        }
    }

    ngOnChanges(): void
    {
        this.lyrics = false;
        //this.checkFavourite();
        
        if(this.loginService.loggedIn && this.song && this.song.id)
            this.songService.incrementPlays(this.song.id).subscribe
            (
                () => this.song.numberOfPlays++, 
                () => alert("Error Incrementing Plays")
            );
    }

    getURL(): SafeResourceUrl
    {
        return this.sanitizer.bypassSecurityTrustResourceUrl("https://w.soundcloud.com/player/?url=" + this.song.urlPath);
    }

    getSpotifyURL(): SafeResourceUrl
    {
        return this.sanitizer.bypassSecurityTrustResourceUrl("https://open.spotify.com/embed/" + this.song.urlPath);
    }

    getGeniusLyrics(): SafeResourceUrl
    {
        return this.sanitizer.bypassSecurityTrustResourceUrl('https://genius.com' + this.song.lyrics);
    }


    showLyrics(): void
    {
        if(this.lyrics)  this.lyrics = false;
        else             this.lyrics = true;
    }

    addToFavourites(): void
    {
        this.songService.addToFavourites(this.song.id, this.loginService.loggedInUser.id).subscribe
        (
            () => 
            {
                alert("Added to Favourites");
                this.isFavourite = true;
            },
            () => alert("Error adding to favourites")
        );
    }

    checkFavourite(): void
    {
        this.isFavourite = false;
        
        if(this.loginService.loggedIn && this.song.id != null && this.song.id > 0)
            this.songService.isFavourite(this.song.id, this.loginService.loggedInUser.id).subscribe
            (
                (data) => this.isFavourite = data,
                () => alert("Error checking favourite")
            );
    }

    goToProfile(): void
    {
        this.loginService.getUserByUsername(this.song.artistName).subscribe
        (
            (data) => 
            {
                if(data)    this.router.navigate(['/profile/' + data.id]);
                else        alert("User profile does not exist!");
            },
            () => alert("Error finding user profile")
        );
    }

    saveToDb(): void
    {
        //First we check if it is already in our database. If not, then we upload.
        //We only check unoriginal songs, because logically an original song must already be in db
        
        //NOTE CHANGE SONGEXISTS TO SONG INSTEAD OF BOOL, THAT WAY WE HAVE ID FOR ADDING SONG TO FAVOURITES
        if(!this.song.isOriginal)
        {
            this.songService.songExists(this.song.title, this.song.artistName).subscribe
            (
                (data) =>
                {
                    if(!data)
                    {
                        this.song.genre="Rock";//TEMP needs to be fixed
                        this.songService.uploadSong(this.song).subscribe
                        (
                            () => console.log(this.song.title + " uploaded to db"),
                            () => alert("Error uploading song")
                        );
                    }
                    else //Song exists
                    {
                        this.song = data;
                        this.songService.incrementPlays(this.song.id).subscribe
                        (
                            () => this.song.numberOfPlays++, 
                            () => alert("Error Incrementing Plays")
                        );
                    }
                },
                () => alert("Error checking if song exists in db")
            );
        }
    }
}
