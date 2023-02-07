import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';
import { UserModel } from '../models/userModel';
import { LoginResponse } from '../models/loginResponse';
import { CustomLoaderService } from '../services/customLoader.service';

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
})
export class CreateComponent implements OnInit {
  registerForm: FormGroup;
  public otherErrorsDiv = null;
  constructor(private formBuilder: FormBuilder, public userService: UserService, private router: Router,
    private customService: CustomLoaderService) {}
  ngOnInit() {
    this.registerForm = this.formBuilder.group({
      userFirstName: ['', [Validators.required]],
      userLastName: ['', [Validators.required]],
      username: ['', [Validators.required]],
      userBirthDate: ['', [Validators.required]],
      userEmail: ['', [Validators.required, Validators.email]],
      userPassword: ['', [Validators.required]]
    });
  }

  get f() { return this.registerForm.controls; }
  checkInput(input: any){
    return input.invalid && (input.dirty || input.touched);
  }

  public activateResponse: LoginResponse = null;
  public downloadedPk = false;
  onSubmit(){ 
    console.log("Aici")
    if (this.registerForm.invalid) {
      return;
    }
    this.customService.start();
    this.userService.CreateUser(<UserModel>{
      username: this.f.username.value,
      email: this.f.userEmail.value,
      birthdate: this.f.userBirthDate.value,
      lastName: this.f.userLastName.value,
      firstName: this.f.userFirstName.value,
      password: this.f.userPassword.value,
    }).subscribe(response => {
      this.customService.success('Account was created!!', 'Success');
      this.router.navigateByUrl("/sprints");
      this.customService.stop();
    },
    this.customService.errorFromResp)
  }

}
