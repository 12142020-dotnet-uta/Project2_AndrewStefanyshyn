import { Song } from "./song";

export class User
{
    public userName: string;
    public password: string;
    public firstName: string;
    public lastName: string;
    public email: string;
    public description: string = "";

    //Social
    public favourites: Array<Song>;
    public friends: Array<User>;
    //public playlists: Array<Playlist>;
    //public songs: Array<UserSong>;

    public id: number;

    constructor(u: string, p: string, e: string)
    {
        this.userName = u;
        this.password = p;
        this.firstName = "";
        this.lastName = "";
        this.email = e;
        this.favourites = new Array<Song>();
        this.friends = new Array<User>();
        //this.playlists = new Array<Playlist>();
        //this.songs = new Array<UserSong>();
    }
}