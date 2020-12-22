import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DataComponent } from './views/data/data.component';

const routes: Routes = [
  {
    path: 'data',
    component: DataComponent
  },
  {
    path: '**',
    redirectTo: 'data',
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
