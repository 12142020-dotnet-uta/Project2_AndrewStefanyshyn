import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { LoginService } from './login.service';

@Component
({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})

export class AppComponent 
{
    private title = 'WhatsThatSong';

    constructor(public loginService: LoginService, private router: Router){}

    logout()
    {
        //this.loginService.loggedIn = false;
        //this.router.navigate(['/']);
        this.loginService.logoutLocal();
    }
}
