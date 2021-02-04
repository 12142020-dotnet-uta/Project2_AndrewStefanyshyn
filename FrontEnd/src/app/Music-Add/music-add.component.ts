import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormControl, Validators, ValidationErrors, ValidatorFn, AbstractControl} from '@angular/forms';

import { User } from '../user';
import { Song } from '../song';
import { LoginService } from '../login.service';
import { SongService } from '../song.service';

@Component({
  selector: 'app-music-add',
  templateUrl: './music-add.component.html',
  styleUrls: ['../app.component.css', './music-add.component.css']
})

export class MusicAddComponent implements OnInit 
{
    public user: User;
	public formdata: FormGroup;// = null;

    constructor(public loginService: LoginService, private songService: SongService, private router: Router) 
    { 
        this.user = loginService.loggedInUser;
    }

	ngOnInit(): void 
	{
        this.formdata = new FormGroup
        (
            {
                title: new FormControl("", Validators.required),
                album: new FormControl("", Validators.required),
                genre: new FormControl("", Validators.required),
                urlPath: new FormControl("", Validators.required),
                lyrics: new FormControl("")
            }
        );	
	}
	
	upload(s: Song): void
	{
        s.artistName = this.user.userName;
        s.isOriginal = true;
        s.albumUrl = "https://edm.com/.image/t_share/MTU5NDY5Nzk2NTUzOTI1OTA1/soundcloud.png";
        //s.year = 2021;
        this.songService.uploadSong(s).subscribe
        (
            () =>
            {
                alert("Song uploaded successfully");
                this.router.navigate(["/music/" + this.user.id]);
            },
        );
    }

    //Quick access properties for the forms
    get title() { return this.formdata.get('title'); }
    get album() { return this.formdata.get('album'); }
    get genre() { return this.formdata.get('genre'); }
    get urlPath() { return this.formdata.get('urlPath'); }
    get lyrics() { return this.formdata.get('lyrics'); }
}
