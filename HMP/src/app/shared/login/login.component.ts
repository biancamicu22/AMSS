import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../services/user.service';
import { UserModel } from '../models/userModel';
import { Router } from '@angular/router';
import { CustomLoaderService } from '../services/customLoader.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  @Input('isAdmin') public isAdmin: boolean = false;
  public otherErrorsDiv = null;
  constructor(private formBuilder: FormBuilder, public userService: UserService, private router: Router,
     private customService: CustomLoaderService) { }

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      username: ['', [Validators.required]],
      userPassword: ['', [Validators.required]]
    });
    if(this.userService.contractLinkEmail != null) this.f.username.disable();
  }

  get f() { return this.loginForm.controls; }

  checkInput(input: any){
    return input.invalid && (input.dirty || input.touched);
  }

  onSubmit(){
    this.otherErrorsDiv = null;
    this.loginForm.markAllAsTouched(); 
    let shouldReturn = false;
    if (this.loginForm.invalid) {
      shouldReturn = true
    }

    if(shouldReturn) return;

    this.customService.start();
    if(!this.isAdmin){
      this.userService.LoginUser(<UserModel>{
        username: this.f.username.value,
        password: this.f.userPassword.value
      }).subscribe(loginResponse => {
            this.router.navigateByUrl("/sprints");
            this.customService.success('Bine ai revenit, ' + loginResponse.username + '!', 'Success');
            this.customService.stop();
      }, this.customService.errorFromResp);
    }
  }
}
