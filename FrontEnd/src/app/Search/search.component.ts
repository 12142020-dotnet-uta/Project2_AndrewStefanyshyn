import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { filter } from 'rxjs/operators';

import { Song } from '../song';
import { SongService } from '../song.service';
import { GeniusService } from '../genius.service';
import { SpotifyService } from '../spotify.service';

@Component
({
    selector: 'app-search',
    templateUrl: './search.component.html',
    styleUrls: ['../app.component.css', './search.component.css']
})

export class SearchComponent implements OnInit 
{
    public query: string;

    public songIn: Array<Song> = new Array<Song>();
    public songSelected: boolean;
    public selectedSong: Song;
    public selectedSongIndex: number; //For the Banner

    constructor(public songService: SongService, public geniusService: GeniusService, public spotifyService: SpotifyService,
                        private route: ActivatedRoute, private router: Router)
    {
        route.queryParams.pipe(filter(params => params.query)).subscribe
        (
            params => 
            {
                this.query = params.query;
            }
        );

        if(this.query.length > 2)
        {
            songService.searchOriginalsByLyrics(this.query).subscribe
            (
                (data) => 
                {
                    for(let x = 0; x < data.length; x++)
                        this.songIn.push(data[x]);
                },
                () => alert("Error Searching")
            );

            geniusService.search(this.query).subscribe
            (
                (data) =>
                {
                    const results = data["response"]["hits"];
                    for(let x = 0; x < results.length; x++)
                    {
                        const result = results[x]["result"];

                        const title = result["title"];
                        const artistName = result["primary_artist"]["name"];
                        const lyrics = result["path"];
                        let s = new Song(result["title"], result["primary_artist"]["name"], "", "", 2021, "", false);
                        s.lyrics = result["path"];

                        //Get Spotify Info
                        this.spotifyService.searchTracks(title).subscribe
                        (
                            (data) =>
                            {
                                const results = data["tracks"]["items"];
                                for(let x = 0; x < results.length; x++)
                                {
                                    const result = results[x];
                                    if(result["artists"][0]["name"].toLowerCase() == artistName.toLowerCase())
                                    {
                                        s = new Song(title, artistName, result["album"]["name"], "", 
                                                            parseInt(result["album"]["release_date"].slice(0, 4)), 
                                                            result["external_urls"]["spotify"].substring(25), false);
                                        s.albumUrl = result["album"]["images"][0]["url"];
                                        s.lyrics = lyrics;
                                        s.numberOfPlays = Math.floor(Math.random() * 100);
                                        break;
                                    }
                                }
                                this.songIn.push(s);
                            },
                            () => 
                            {
                                console.log("Error with Spotify API"); 
                                this.songIn.push(s);
                            }
                        );
                    }
                    console.log("Success");
                },
                () => console.log("Error with Genius API")
            );
        }

        router.routeReuseStrategy.shouldReuseRoute = function () 
        {
            return false;
        };
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

    search()
    {
        this.router.navigate(['/search'], { queryParams: { query: this.query } });
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

/*
    testGenius(): void
    {
        this.geniusService.search("The sin I bring").subscribe
        (
            (data) =>
            {
                const results = data["response"]["hits"];
                for(let x = 0; x < results.length; x++)
                {
                    const result = results[x]["result"];
                    let s = new Song(result["title"], result["primary_artist"]["name"], "", "", 2021, "", false);
                    this.songIn.push(s);
                    console.log(result["title"]);
                }
            },
            () => alert("Error with Genius API")
        );
    }

    testSpotify(): void
    {
        const title: string = "Rosenrot";
        const artistName: string = "Rammstein";
        this.spotifyService.searchTracks(title).subscribe
        (
            (data) =>
            {
                const results = data["tracks"]["items"];
                for(let x = 0; x < results.length; x++)
                {
                    const result = results[x];//["album"];
                    if(result["artists"][0]["name"].toLowerCase() == artistName.toLowerCase())
                    {
                        let s = new Song(result["name"], result["artists"][0]["name"], result["album"]["name"], "", 
                                            parseInt(result["album"]["release_date"].slice(0, 4)), 
                                            result["external_urls"]["spotify"].substring(25), false);
                        this.songIn.push(s);
                        console.log(result["name"]);
                    }
                }
            },
            () => alert("Error with Spotify API")
        );
    }
    */
}
