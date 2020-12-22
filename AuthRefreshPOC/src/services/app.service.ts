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

    public get Claims(): string[] {
        if (this._user === null) {
            return [];
        }
        var returnClaims = [...this._user.Claims];
        if (this._user.ImpersonatedUser !== null) {
            returnClaims = [...new Set([...returnClaims ,...this._user.ImpersonatedUser.Claims])];
        }
        return returnClaims;
    }
    public get ImpersonationChanged$(): Observable<User> {
        return this.impersonationChanged.asObservable();
    }
    public get AuthChanged$(): Observable<User> {
        return this.authChanged.asObservable();
    }


    constructor() {
        this._user = null;
        
    }
    Impersonate(token: string) {
        const decodedToken: Token = this.jwtHelperService.decodeToken(token);
        this._user.ImpersonatedUser = {
            Token: token,
            UscId: decodedToken.nameid,
            Claims: JSON.parse(decodedToken.claims)
        };
        this.impersonationChanged.next(this._user.ImpersonatedUser);
    }
    Login(token: string) {
        const decodedToken: Token = this.jwtHelperService.decodeToken(token);
        this._user = {
            Token: token,
            UscId: decodedToken.sub,
            Claims: JSON.parse(decodedToken.claims),
            ImpersonatedUser: null
        };
        this.authChanged.next(this._user);
    }
    RefreshToken(token: string) {
        this._user.Token = token;
    }


}