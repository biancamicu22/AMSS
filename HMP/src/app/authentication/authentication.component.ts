import { Component } from '@angular/core';
import { UserService } from '../shared/services/user.service';
import { Router, ActivatedRoute } from '@angular/router';
import { AppSettings } from '../app.settings';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.scss']
})
export class AuthenticationComponent {


  constructor(private userService: UserService, private appSettings: AppSettings, private route: ActivatedRoute) {
    this.route.queryParams.subscribe(params => {
      var code = params['code'];
      var email = params['email'];
      if (code != null && email != null) {
        this.linkMessage = true;
        this.userService.contractLinkCode = code;
        this.userService.contractLinkEmail = atob(email);
      }
    });

  }

  public linkMessage = false;
  public hideLogin = false;
  public hideCreate = true;
  public activateLogin = (event: Event): void => {
    this.hideLogin = false;
    this.hideCreate = true;
  }
  public activateCreate = (event: Event): void => {
    this.hideLogin = true;
    this.hideCreate = false;
  }
}
