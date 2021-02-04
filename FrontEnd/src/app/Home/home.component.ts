import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { filter } from 'rxjs/operators';
import { DOCUMENT } from '@angular/common';

import { Song } from '../song';
import { LoginService } from '../login.service';
import { SongService } from '../song.service';
import { FriendService } from '../friend.service';
import { GeniusService } from '../genius.service';
import { SpotifyService } from '../spotify.service';

@Component
({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['../app.component.css', './home.component.css']
})

export class HomeComponent implements OnInit 
{
    public homepageSong: Song;
    public query: string;// = "";
    public userquery: string;// = "";

    
    public songIn: Array<Song> = new Array<Song>();
    public songSelected: boolean;
    public selectedSong: Song;
    public selectedSongIndex: number; //For the Banner

    //Spotify stuff
    private authCode: string = null;
    
    constructor(public loginService: LoginService, public songService: SongService, public friendService: FriendService, 
                    public spotifyService: SpotifyService, public geniusService: GeniusService,
                    private route: ActivatedRoute, private router: Router, 
                    @Inject(DOCUMENT) private document: Document)
    {
        route.queryParams.pipe(filter(params => params.code)).subscribe
        (
            params => 
            {
                this.authCode = params.code;
                console.log(this.authCode);
                this.testSpotify22();
            }
        );

        songService.getHomepageSongs().subscribe
        (
            (data) => 
            {
                this.songIn = data; 
                this.displaySong(0);
                //this.displaySong(Math.floor(Math.random() * 5)); //5 = # of top songs displayed
            },
            () => alert("Error getting Favourites")
        );
    

        //Parse Friend Requests
        //As it currently is, will require another subscription for getting user name.
        //We should fix this in db.
       if(loginService.loggedIn)
       {
            friendService.getAllFriendRequests(loginService.loggedInUser.id).subscribe
            (
                (data) => 
                {
                    for(let x = 0; x < data.length; x++)
                    {
                        if(friendService.isPending(data[x]))
                        {
                            if(confirm("Would you like to become friends with " + data[x].fromUsername + "?"))
                            {
                                friendService.setAccepted(data[x]);
                                friendService.editFriendRequest(data[x]).subscribe
                                (
                                    () => alert("You are now friends"),
                                    () => alert("Accept Error")
                                );
                            }
                            else
                            {
                                friendService.setRejected(data[x]);
                                friendService.editFriendRequest(data[x]).subscribe(() => {}, () => alert("Error Rejecting"));
                            }
                        }
                        //Keeps displaying. If we have time we can add another state = "Accepted Viewed"
                        /*
                        else if(friendService.isAccepted(data[x]))
                        {
                            alert(data[x].toUsername + " accepted your friend request.");
                            //friendService.deleteFriendRequest(data[x].id).subscribe();
                        }
                        */
                       //This is not working. SHows up for wrong user. Needs to be refactored
                       /*
                        else if(friendService.isRejected(data[x]))
                        {
                            alert(data[x].toUsername + " rejected your friend request.");
                            friendService.deleteFriend(data[x].friendId, data[x].requestedFriendId).subscribe(() => {}, () => alert("Error Rection"));
                            //friendService.deleteFriendRequest(data[x].id).subscribe();
                        }
                        else if(friendService.isDeleted(data[x]))
                        {
                            alert(data[x].fromUsername + " is no longer your friend.");
                            friendService.deleteFriend(data[x].friendId, data[x].requestedFriendId).subscribe(() => {}, () => alert("Error Deletion"));
                            //friendService.deleteFriendRequest(data[x].id).subscribe();
                        }
                        */
                    }
                },
                (error) => alert(error)
            );
        }
    }
  
    ngOnInit(): void 
    {
        //this.displaySong(0);
        //this.displaySong(Math.floor(Math.random() * 5)); //5 = # of top songs displayed

        //Test songs
        //this.homepageSong = new Song("Moonlight Sonata 3", "Viossy", "Viossy", "Rock", 2010, "https://soundcloud.com/xabcxyzx/drviossy-moonlight-sonata-beethoven-metal-version", false);
        //this.homepageSong = new Song("Joel's Song", "Joel", "Joel's Album", "Rock", 2000, "https://soundcloud.com/00joel/dgnj", true);
        //this.homepageSong = new Song("St. Matthew's Passion", "Bach", "Work", "Orchestral", 2021, "track/5PGo11SpjSti3e6qi3CItZ", false);
        //this.homepageSong.lyrics = "Instrumental";
    }

    search(): void
    {
        this.router.navigate(['/search'], { queryParams: { query: this.query } });
    }

    userSearch(): void
    {
        this.router.navigate(['/usersearch'], { queryParams: { query: this.userquery } });
    }
    
    displaySong(x: number)
    {
        this.selectedSong = this.songIn[x];
        this.songSelected = true;
        this.selectedSongIndex = x;
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

    
    test(): void
    {
        alert("Testing.");
        this.loginService.getValidUsers().subscribe
        (
            (data) => alert("SUCCESS"),
            () => alert("ERROR")
        );
    }

    testSpotify2(): void
    {
        this.document.location.href = this.spotifyService.getAuthUrl();
    }

    testSpotify22(): void
    {
        if(!this.authCode)
        {
            console.log("Your authCode is not set. Try doing Step 1 again.");
            return;
        }
        this.spotifyService.authStepTwo(this.authCode).subscribe
        (
            (data) => 
            {
                console.log(data["access_token"]);
                this.spotifyService.access_token = data["access_token"];
                this.spotifyService.loggedIn = true;
            },
            (error) => 
            {
                console.log(error["error"]);
                console.log(error["error_description"]);
            }
        );
    }

    /*
    testGenius(): void
    {
        this.document.location.href = this.geniusService.getAuthUrl();
    }

    testGenius2(): void
    {
        if(!this.authCode)
        {
            console.log("Your authCode is not set. Try doing Step 1 again.");
            return;
        }
        this.geniusService.authStepTwo(this.authCode).subscribe
        (
            (data) => 
            {
                console.log(data["access_token"]);
                this.spotifyService.access_token = data["access_token"];
            },
            (error) => 
            {
                console.log(error["error"]);
                console.log(error["error_description"]);
            }
        );
    }
    */
}
