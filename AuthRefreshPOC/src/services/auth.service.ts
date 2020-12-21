import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { HttpClient } from '@angular/common/http';
import { AppService } from './app.service';
import { Token } from 'src/app/Interfaces/token.interface';
import { environment } from 'src/environments/environment';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private jwtHelperService: JwtHelperService = new JwtHelperService();
    private _expirationDate: Date;

    public get ExpirationDate(): Date {
        return this._expirationDate;
    }
    constructor(private httpClient: HttpClient,
                private appService: AppService)
    {
    }
    public ImpersonateUser(uscid: string)
    {
        const sub = this.httpClient
            .get(environment.authUrl + "/auth/impersonate?uscid=" + uscid, {
                responseType: 'text'
            })
            .subscribe(
                (token) => {
                    this.appService.Impersonate(token);
                },
                (error) => {
                    console.log('Error caught');
                    console.log(error);
                }
            );

    }
    public GetToken(uscid: string)
    {
        const sub = this.httpClient
            .get(environment.authUrl + "/auth/token?uscid=" + uscid, {
                responseType: 'text'
            })
            .subscribe(
                (token) => {
                    this.appService.Login(token);
                    this.SetupRefreshTimer(token);
                },
                (error) => {
                    console.log('Error caught');
                    console.log(error);
                }
            );
    }
    private RefreshToken() {
        const sub = this.httpClient
            .post(environment.authUrl + "/auth/refresh", 
                {}, 
                {
                    responseType: 'text'
                }
            )
            .subscribe(
                (token) => {
                    this.appService.RefreshToken(token);
                    this.SetupRefreshTimer(token);
                },
                (error) => {
                    console.log('Error caught');
                    console.log(error);
                }
            );
    }
    private SetupRefreshTimer(token) {
        this._expirationDate = this.jwtHelperService.getTokenExpirationDate(
          token
        );
        const expireTimeDelay =
            this._expirationDate.getTime() - new Date().getTime() - 60000;
        setTimeout(() => {
          this.RefreshToken();
        }, expireTimeDelay);
      }
    
}
