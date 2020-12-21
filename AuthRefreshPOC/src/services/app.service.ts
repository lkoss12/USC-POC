import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable, Subject } from 'rxjs';
import { AppUser, User } from 'src/app/Interfaces/appuser.interface';
import { Token } from 'src/app/Interfaces/token.interface';

@Injectable({
    providedIn: 'root'
})
export class AppService {
    private jwtHelperService: JwtHelperService = new JwtHelperService();

    private _user: AppUser;
    
    public get User(): AppUser {
        return this._user;
    }

    private authChanged = new Subject<User>();
    private impersonationChanged = new Subject<User>();

    public get ImpersonationChanged$(): Observable<User> {
        return this.impersonationChanged.asObservable();
    }
    public get AuthChanged$(): Observable<User> {
        return this.authChanged.asObservable();
    }


    constructor() {
        
    }
    Impersonate(token: string) {
        const decodedToken: Token = this.jwtHelperService.decodeToken(token);
        this._user.ImpersonatedUser = {
            Token: token,
            UscId: decodedToken.nameid
        };
        this.impersonationChanged.next(this._user.ImpersonatedUser);
    }
    Login(token: string) {
        const decodedToken: Token = this.jwtHelperService.decodeToken(token);
        this._user = {
            Token: token,
            UscId: decodedToken.sub
        };
        this.authChanged.next(this._user);
    }
    RefreshToken(token: string) {
        this._user.Token = token;
    }


}