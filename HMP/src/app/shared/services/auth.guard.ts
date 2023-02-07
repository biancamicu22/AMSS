import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { UserService } from './user.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Roles } from '../models/roles';


@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
    constructor(
        private router: Router,
        private authenticationService: UserService,
        private jwtHelper: JwtHelperService,
    ) { }

    private performRedirect(route: ActivatedRouteSnapshot){
        if(route.data.role == Roles.Admin){
            this.router.navigate(['/admin-login'], { queryParams: {  } });
        }
        else{
            this.router.navigate(['/auth'], { queryParams: {  } });
        }
    }
    
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        const currentUser = this.authenticationService.currentUserValue;
        if(route.data.requireLogin == false || route.data.requireLogin == undefined){
            return true;
        }
        if(route.data.role){
            if (currentUser != null) {
                if(this.jwtHelper.isTokenExpired(currentUser.jwt)){
                    this.authenticationService.LogoutUser().subscribe(x => {
                        this.performRedirect(route);
                        return false;
                    }, e => {
                        this.performRedirect(route);
                        return false;
                    });
                }
                const uRole = this.authenticationService.GetUserRole();
                if((route.data.role as []).find(role => role == uRole) == undefined){
                    this.performRedirect(route);
                    return false;
                }
                return true;
            }
            else if(currentUser == null){
                this.performRedirect(route);
                return false;
            }
        }
    }
}