import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";

import { LoginService } from "../login.service";
import { User } from "../user";

@Component
({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['../app.component.css', './login.component.css']
})
  
export class LoginComponent implements OnInit
{
    public formdata: FormGroup;// = null;

    constructor(public loginService: LoginService, private router: Router) {}

    ngOnInit(): void
    {
        this.formdata = new FormGroup
        (
            {
                userName: new FormControl("", Validators.required),
                password: new FormControl("", Validators.required)
            }
        );
    }

    login(user: User): void
    {
        const u: User = new User(user.userName, user.password, "");
        this.loginService.login(u).subscribe
        (
            (data) => 
            {
                if(data)
                {
                    this.loginService.loginLocal(data);
                    this.router.navigate(['/']);
                }
                else
                {
                    alert("User Does Not Exist");
                }
            },
            () => alert("Error!")
        );

    }

    //Quick access properties for the forms
    get userName() { return this.formdata.get('userName'); }
    get password() { return this.formdata.get('password'); }
}