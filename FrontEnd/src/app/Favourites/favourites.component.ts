import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Song } from '../song';
import { LoginService } from '../login.service';
import { SongService } from '../song.service';

@Component
({
    selector: 'app-favourites',
    templateUrl: './favourites.component.html',
    styleUrls: ['../app.component.css', './favourites.component.css']
})

export class FavouritesComponent implements OnInit 
{
    public homeUser: boolean;
    public songIn: Array<Song> = new Array<Song>();
    public songSelected: boolean;
    public selectedSong: Song;
    public selectedSongIndex: number;

    constructor(public loginService: LoginService, public songService: SongService, private route: ActivatedRoute)
    {
        let id: number;
        route.params.subscribe
        (
            params => 
            {
                if(params.id)  id = params['id'];
                else           id = -1;
            }
        );
                
        if(id == -1 || this.loginService.loggedInUser.id == id)  this.homeUser = true;
        else    this.homeUser = false;

        //Just in case
        if(id == -1 && loginService.loggedIn)  id = loginService.loggedInUser.id;

        songService.getFavourites(id).subscribe
        (
            (data) => this.songIn = data,
            () => alert("Error getting Favourites")
        );
    }
  
    ngOnInit(): void 
    {
        this.songSelected = false;
    }

    displaySong(x: number)
    {
        this.selectedSong = this.songIn[x];
        this.songSelected = true;
        this.selectedSongIndex = x;
    }  

    delete(x: number)
    {
        this.deleteFromFavourites(this.songIn[x], x);
    }

    deleteFromFavourites(s: Song, index: number)
    {
        if(this.homeUser)
        {
            this.songService.deleteFavourite(s.id, this.loginService.loggedInUser.id).subscribe
            (
                () =>
                {
                    this.songIn.splice(index, 1);
                },
                () => alert("Error Deleting from favourites")
            );
        }
        else    alert("You do not have permission to delete these items!");
    }
    
    //Banner Methods
    getNextBannerSong(): void
    {
        if(this.songIn.length <= this.selectedSongIndex + 1)
            this.displaySong(0);
        else
            this.displaySong(this.selectedSongIndex + 1);
    }

    getPrevBannerSong(): void
    {
        if(this.selectedSongIndex <= 0)
            this.displaySong(this.songIn.length - 1);
        else
            this.displaySong(this.selectedSongIndex - 1);
    }
}
