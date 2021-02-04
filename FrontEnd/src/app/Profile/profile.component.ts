import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Song } from '../song';
import { User } from '../user';
import { FriendList } from '../friendRequest';
import { LoginService } from '../login.service';
import { SongService } from '../song.service';
import { FriendService } from '../friend.service';

@Component
({
    selector: 'app-profile',
    templateUrl: './profile.component.html',
    styleUrls: ['../app.component.css', './profile.component.css']
})

export class ProfileComponent implements OnInit 
{
    public user: User;// = null;
    public homeUser: boolean;// = false;
    public isFriend: boolean = false;

    //Track List Info
    public songIn: Array<Song> = new Array<Song>();
    public selectedSong: Song;// = null;
    public songSelected: boolean;// = false;
    public selectedSongIndex: number;

    constructor(public loginService: LoginService, public songService: SongService, public friendService: FriendService, private route: ActivatedRoute, private router: Router)
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
        
        if(id == -1 || (this.loginService.loggedIn && this.loginService.loggedInUser.id == id))  this.homeUser = true;
        else
        {
            this.homeUser = false;
            this.loginService.getUser(id).subscribe
            (
                (data) => 
                {
                    this.user = data;

                    //Then we get friends
                    this.friendService.checkFriends(this.loginService.loggedInUser.id, data.id).subscribe
                    (
                        (data) => 
                        {
                            if(data)    this.isFriend = true;
                            else        this.isFriend = false;
                        },
                        () => 
                        {
                            this.isFriend = false;
                            alert("Error");
                        }
                    );

                    //Then we get favourites
                    songService.getTopFavourites(id).subscribe
                    (
                        (data) => this.songIn = data,
                        () => alert("Error getting Favourites")
                    );
                },
                () => this.homeUser = true
            );
        }

        if(this.homeUser)
        {
            this.user = loginService.loggedInUser;

            //Set the favourite songs
            songService.getTopFavourites(this.user.id).subscribe
            (
                (data) => this.songIn = data,
                () => alert("Error getting Favourites")
            );
        }
    }
  
    ngOnInit(): void 
    {
    }

    
    displaySong(x: number)
    {
        this.selectedSong = this.songIn[x];
        this.songSelected = true;
        this.selectedSongIndex = x;
    }  

    makeFriend(): void
    {
        if(!this.homeUser)  //We shouldn't get this option otherwise
        {
            let fl: FriendList = new FriendList(this.loginService.loggedInUser.id, this.user.id);
            fl.toUsername = this.user.userName;
            fl.fromUsername = this.loginService.loggedInUser.userName;
            this.friendService.requestFriend(fl).subscribe
            (
                () => alert("Sent a friend request to " + this.user.userName),
                () => alert("Request Error")
            );
        }
    }

    removeFriend(): void
    {
        if(!this.homeUser) //We shouldn't get this option otherwise
        {
            let fl = new FriendList(this.loginService.loggedInUser.id, this.user.id);
            fl.toUsername = this.user.userName;
            fl.fromUsername = this.loginService.loggedInUser.userName;
            this.friendService.setDeleted(fl);
            this.friendService.editFriendRequest(fl).subscribe
            (
                () =>
                {
                    this.isFriend = false;
                    alert("You are no longer friends with " + this.user.userName);
                },
                () => alert("Failure")
            );
        }
    }
    
    findFriend(u: User): number
    {
        let index = -1;
        for(let x = 0; x < this.loginService.loggedInUser.friends.length; x++)
            if(u.userName == this.loginService.loggedInUser.friends[x].userName)
            {
                index = x;
                break;
            }
    
        return index;
    }


    //Router Methods
    goToChat(): void
    {
        this.router.navigate(['/chat/' + this.user.id + '/' + this.loginService.loggedInUser.id]);
    }

    goToEdit(): void
    {
        this.router.navigate(['/editProfile/' + this.loginService.loggedInUser.id]);
    }

    goToFavourites(): void
    {
        this.router.navigate(['/favourites/' + this.user.id]);
    }

    goToOriginalMusic(): void
    {
        this.router.navigate(['/music/' + this.user.id]);
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
