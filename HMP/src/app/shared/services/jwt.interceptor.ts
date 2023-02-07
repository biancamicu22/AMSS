import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserService } from './user.service';
import { AppSettings } from 'src/app/app.settings';


@Injectable()
export class JwtInterceptor implements HttpInterceptor {
    constructor(private authenticationService: UserService, private appsettings: AppSettings) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // add auth header with jwt if user is logged in and request is to api url
        const currentUser = this.authenticationService.currentUserValue;
        const isLoggedIn = currentUser && currentUser.jwt;
        const isApiUrl = request.url.startsWith(this.appsettings.baseApiUrl);
        if (isLoggedIn && isApiUrl) {
            request = request.clone({
                setHeaders: {
                    Authorization: `Bearer ${currentUser.jwt}`,
                },
                
            });
            request.headers.append("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
        }

        return next.handle(request);
    }
}