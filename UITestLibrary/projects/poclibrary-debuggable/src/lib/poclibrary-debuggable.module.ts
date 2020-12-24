import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { PoclibraryDebuggableComponent } from './poclibrary-debuggable.component';
import {MatButtonModule} from '@angular/material/button';
import {MatInputModule} from '@angular/material/input';



@NgModule({
  declarations: [PoclibraryDebuggableComponent],
  imports: [
    FormsModule,
    CommonModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatInputModule
  ],
  exports: [PoclibraryDebuggableComponent]
})
export class PoclibraryDebuggableModule { }
