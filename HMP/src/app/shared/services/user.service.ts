import { Injectable } from '@angular/core';
import { AppSettings } from 'src/app/app.settings';
import { Observable, BehaviorSubject } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators'
import { JwtHelperService } from '@auth0/angular-jwt';
import { LoginResponse } from '../models/loginResponse';
import { UserModel } from '../models/userModel';
import { Roles } from '../models/roles';
@Injectable()
export class UserService {
    public contractLinkCode:string = null; 
    public contractLinkEmail: string = null;
    public currentUserSubject: BehaviorSubject<LoginResponse>;
    public currentUser: Observable<LoginResponse>;
    public isAdmin: boolean = false;

    public checkLinkEmail(): any{
        if(this.contractLinkEmail == null || this.contractLinkEmail == '') return null;
        return this.contractLinkEmail;
      }

    public get currentUserValue(): LoginResponse {
      return this.currentUserSubject.value;
    }

    constructor(private appSettings: AppSettings, private http: HttpClient, private jwtHelper: JwtHelperService){
        const uData = this.GetUserData();
        if(uData && uData.jwt && jwtHelper.isTokenExpired(uData.jwt) == false){
            this.currentUserSubject = new BehaviorSubject<LoginResponse>(uData);
        }
        else{
            this.currentUserSubject = new BehaviorSubject<LoginResponse>(null);
        }
        this.contractLinkEmail = null;
        this.currentUser = this.currentUserSubject.asObservable();
    }

    public updateProfile(userData: UserModel): Observable<LoginResponse>{
        return this.http.put<LoginResponse>(this.appSettings.baseApiUrl + "user/me", userData).pipe(map(user => {
            this.currentUserSubject.next(user);
            this.SaveUserData(user);
            return user;
        }));
    }

    public updateUserPassword(passwordRequest: any): Observable<LoginResponse>{
        return this.http.put<LoginResponse>(this.appSettings.baseApiUrl + "user/me/password", passwordRequest).pipe(map(user => {
            this.currentUserSubject.next(user);
            this.SaveUserData(user);
            return user;
        }));
    }

    public getUserProfile(): Observable<UserModel>{
        const uData = this.GetUserData();
        return this.http.get<UserModel>(this.appSettings.baseApiUrl + "user/me");
    }

    public IsUserAdmin(): boolean{
        if(this.currentUserValue != null){
            const decoded = this.jwtHelper.decodeToken(this.currentUserValue.jwt);
            return decoded.ROLE == Roles.Admin;
        }
        return false;
    }

    public GetUserRole(): string{
        if(this.currentUserValue != null){
            const decoded = this.jwtHelper.decodeToken(this.currentUserValue.jwt);
            return decoded.ROLE;
        }
        return null;
    }

    public LoginUser(user: UserModel): Observable<LoginResponse>{

        return this.http.post<LoginResponse>(this.appSettings.baseApiUrl + "user/session", user).pipe(map(user => {
            this.currentUserSubject.next(user);
            this.SaveUserData(user);
            return user;
        }));
    }

    public CreateUser(user: UserModel): Observable<LoginResponse>{
        return this.http.post<LoginResponse>(this.appSettings.baseApiUrl + "user/create", user).pipe(map(user => {
            this.currentUserSubject.next(user);
            this.SaveUserData(user);
            return user;
        }));
    }

    public SaveUserData(userData: LoginResponse){
        localStorage.setItem(this.appSettings.lsUserDataKey, JSON.stringify(userData));
    }

    public GetUserData(): LoginResponse{
        try{
            return JSON.parse(localStorage.getItem(this.appSettings.lsUserDataKey));
        }
        catch{
            return null;
        }
    }

    public DeleteUserData(){
        try{
            localStorage.removeItem(this.appSettings.lsUserDataKey);
        }
        catch{
            return null;
        }
    }

    public LogoutUser(): Observable<LoginResponse>{
        const uData = this.GetUserData();
        this.DeleteUserData();
        this.currentUserSubject.next(null);
        return this.http.post<LoginResponse>(this.appSettings.baseApiUrl + "user/logout", {}).pipe(map(user => {
            return user;
        }));
    }

    public userList: UserModel[] = []; 
    public getAllUsers(): Observable<UserModel[]>{
        return this.http.get<UserModel[]>(this.appSettings.baseApiUrl + "user/all").pipe(map(list => {
            this.userList = list;
            return list;
        }));
    }
}