import { Injectable } from "@angular/core";
import { CanActivate } from "@angular/router";
import { LoginService } from "./login.service";

@Injectable()
export class GuardService implements CanActivate
{
    constructor(private loginService: LoginService) {}

    canActivate() : boolean
    {
        return this.loginService.loggedIn;
    }
}