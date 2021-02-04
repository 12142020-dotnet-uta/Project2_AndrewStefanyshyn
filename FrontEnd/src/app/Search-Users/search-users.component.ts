import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { filter } from 'rxjs/operators';

import { User } from '../user';
import { LoginService } from '../login.service';

@Component
({
    selector: 'app-search-users',
    templateUrl: './search-users.component.html',
    styleUrls: ['../app.component.css', './search-users.component.css']
})

export class SearchUsersComponent implements OnInit 
{
    public query: string;
    public users: Array<User>;

    constructor(public loginService: LoginService, private route: ActivatedRoute, private router: Router){}
  
    ngOnInit(): void 
    {
        this.route.queryParams.pipe(filter(params => params.query)).subscribe
        ( 
            params => 
            { 
                this.query = params.query; 
            } 
        );
        
        this.router.routeReuseStrategy.shouldReuseRoute = function () 
        {
            return false;
        };

        //Initialize Search Results
        this.users = new Array<User>();
        this.loginService.searchUsers(this.query).subscribe
        (
            (data) => this.users = data,
            () => alert("No users found")
        );
    }

    search()
    {
        this.router.navigate(['/usersearch'], { queryParams: { query: this.query } });
    }

    
    displayProfile(u: User): void
    {
        this.router.navigate(['/profile', u.id]);
    }
}
