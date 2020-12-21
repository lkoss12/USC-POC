import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AppService } from 'src/services/app.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private appService: AppService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (this.appService.User != null) {
        request = request.clone({
            setHeaders: {
                Authorization: 'Bearer ' + this.appService.User.Token
            }
        });
        if (request.url.endsWith('refresh')) {
            request = request.clone({
                withCredentials: true
            });
        } else if (this.appService.User.ImpersonatedUser != null) {
            request = request.clone({
                setHeaders: {
                    'X-USC-IMPERSONATION': this.appService.User.ImpersonatedUser.Token
                }
            });    
        }
    }
    return next.handle(request);
  }
}
