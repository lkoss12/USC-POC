import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AppService } from 'src/services/app.service';
import { AuthService } from 'src/services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'AuthRefreshPOC';
  uscId: string;
  impersonationId: string;

  isAuthorized: boolean;
  authChangedSubscription: Subscription;
  impersonationChangedSubscription: Subscription;

  public get AppService(): AppService {
    return this.appService;
  }
  public get AuthService(): AuthService {
    return this.authService;
  }
  constructor(private authService: AuthService,
              private appService: AppService) {
    this.isAuthorized = false;
  }
  ngOnDestroy(): void {
    this.authChangedSubscription.unsubscribe();
    this.impersonationChangedSubscription.unsubscribe();
  }
  ngOnInit(): void {
    this.authChangedSubscription = this.appService
      .AuthChanged$
      .subscribe((uscId) => {
        this.isAuthorized = true;
      });

      this.impersonationChangedSubscription = this.appService
        .ImpersonationChanged$
        .subscribe((uscId) => {

      });
  }
  impersonate() {
    this.authService.ImpersonateUser(this.impersonationId);
  }
  login() {
    this.authService.GetToken(this.uscId);
  }
}
