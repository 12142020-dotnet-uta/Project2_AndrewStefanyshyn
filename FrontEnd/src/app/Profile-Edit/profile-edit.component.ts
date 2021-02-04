import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormControl, Validators, ValidationErrors, ValidatorFn, AbstractControl} from '@angular/forms';

import { User } from '../user';
import { LoginService } from '../login.service';

@Component({
  selector: 'app-profile-edit',
  templateUrl: './profile-edit.component.html',
  styleUrls: ['../app.component.css', './profile-edit.component.css']
})

export class ProfileEditComponent implements OnInit 
{
    public user: User;
    public formdataDescription: FormGroup;// = null;
    public formdataFirstName: FormGroup;// = null;
    public formdataLastName: FormGroup;// = null;

    constructor(public loginService: LoginService, private route: ActivatedRoute, private router: Router) 
    {
        let routeId: number;
        route.params.subscribe
        (
            params => 
            {
                if(params.id)  routeId = params['id'];
                else           routeId = 0;
            }
        );

        loginService.getUser(routeId).subscribe
        (
            (data) => this.user = data,
            (error) => alert(error)
        );
    }
    
	ngOnInit(): void 
	{
        this.formdataDescription = new FormGroup({ description: new FormControl("") });	
        this.formdataFirstName = new FormGroup({ firstName: new FormControl("") });	
        this.formdataLastName = new FormGroup({ lastName: new FormControl("") });	
	}
	
	updateDescription(description: string): void
	{
        this.user.description = description["description"];

        this.loginService.editUser(this.user.id, this.user).subscribe
        (
            () => 
            {
                this.loginService.loggedInUser = this.user;
                this.router.navigate(['/profile/' + this.user.id]);
            },
            () => alert("There was an error!")
        );
    }

    updateFName(firstName: string): void
	{
        this.user.firstName = firstName["firstName"];

        this.loginService.editUser(this.user.id, this.user).subscribe
        (
            () => 
            {
                this.loginService.loggedInUser = this.user;
                this.router.navigate(['/profile/' + this.user.id]);
            },
            () => alert("There was an error!")
        );
    }

    updateLName(lastName: string): void
	{
        this.user.lastName = lastName["lastName"];

        this.loginService.editUser(this.user.id, this.user).subscribe
        (
            () => 
            {
                this.loginService.loggedInUser = this.user;
                this.router.navigate(['/profile/' + this.user.id]);
            },
            () => alert("There was an error!")
        );
    }

    
    //Quick access properties for the forms
    get description() { return this.formdataDescription.get('description'); }
    get firstName() { return this.formdataFirstName.get('firstName'); }
    get lastName() { return this.formdataLastName.get('lastName'); }
}
