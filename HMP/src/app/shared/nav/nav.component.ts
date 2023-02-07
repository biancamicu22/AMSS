import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';
import { CustomLoaderService } from '../services/customLoader.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
})
export class NavComponent implements OnInit{

  constructor(public userService: UserService, private router: Router, 
    private customService: CustomLoaderService) {
    
  }
  userLoggedIn: boolean = false;
  username:string = '';
  ngOnInit(): void {
    if(this.userService.currentUserValue != null){
      this.userLoggedIn = true;
      this.username = this.userService.currentUserValue.username;
    }
    this.userService.currentUser.subscribe(user => {
      if(user == null){
        this.username = '';
        this.userLoggedIn = false;
      }
      else{
        this.username = user.username;
        this.userLoggedIn = true;
      }
    });
  }
  
  signOut(){
    this.router.navigateByUrl("/auth");
    this.customService.success('V-ati deconectat cu succes!', 'Success');
    this.userService.LogoutUser().subscribe(response => {
    });
  }
}
