import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { POCLibraryModule } from 'projects/poclibrary/src/public-api';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    POCLibraryModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
