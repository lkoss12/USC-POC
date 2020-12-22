import { NgModule } from '@angular/core';
import { POCLibraryComponent } from './poclibrary.component';
import {MatButtonModule} from '@angular/material/button';
import {MatInputModule} from '@angular/material/input';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [POCLibraryComponent],
  imports: [
    FormsModule,
    CommonModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatInputModule
  ],
  exports: [POCLibraryComponent]
})
export class POCLibraryModule { }
