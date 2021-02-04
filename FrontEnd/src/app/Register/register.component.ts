import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormControl, Validators, ValidationErrors, ValidatorFn, AbstractControl} from '@angular/forms';

import { User } from '../user';
import { LoginService } from '../login.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['../app.component.css', './register.component.css']
})

export class RegisterComponent implements OnInit 
{
	public formdata: FormGroup;// = null;

	constructor(public loginService: LoginService, private router: Router) { }

	ngOnInit(): void 
	{
        this.formdata = new FormGroup
        (
            {
                email: new FormControl("", [Validators.required, Validators.email]),
                userName: new FormControl("", Validators.required),
                password: new FormControl("", Validators.required),
                rePassword: new FormControl("", Validators.required)
            },
            {
            validators: this.PasswordValidator
            }
        );	
	}
	
	register(user: User): void
	{
        //const u = new User(user.userName, user.password, user.firstName, user.lastName, user.email);
        const u = new User(user.userName, user.password, user.email);

        this.loginService.register(u).subscribe
        (
            (data) => 
            {
                if(!data) 
                    alert("User Already Exists!");
                else
                {
                    this.loginService.loginLocal(data);
                    this.router.navigate(['/']);
                }
            },
            () => alert("There was an error!")
        );
    }

    private PasswordValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => 
        {
            if(control.get('password').value != control.get('rePassword').value)
                return { rePassword: "Does not match" };
            else
                return null;
        };

    //Quick access properties for the forms
    get email() { return this.formdata.get('email'); }
    get userName() { return this.formdata.get('userName'); }
    get password() { return this.formdata.get('password'); }
    get rePassword() { return this.formdata.get('rePassword'); }
}
