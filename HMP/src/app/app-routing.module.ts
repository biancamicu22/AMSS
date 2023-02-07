import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthenticationComponent } from './authentication/authentication.component';
import { Roles } from './shared/models/roles';
import { AuthGuard } from './shared/services/auth.guard';
import { SprintComponent } from './sprint/sprint.component';
import { TaskUpdateComponent } from './task/task-update/task-update.component';
import { TaskComponent } from './task/task.component';


const routes: Routes = [
  {
    path: '',
    redirectTo: "/auth",
    pathMatch: 'full'
  },
  {
    path: 'sprints', 
    component: SprintComponent,
    canActivate: [AuthGuard],
    data: { role: [Roles.User], requireLogin: true },
  },
  {
    path: 'tasks', 
    component: TaskComponent,
    canActivate: [AuthGuard],
    data: { role: [Roles.User], requireLogin: true },
  },
  {
    path: 'tasks/:id', 
    component: TaskUpdateComponent,
    canActivate: [AuthGuard],
    data: { role: [Roles.User], requireLogin: true },
  },
  {
    path: 'auth', 
    component: AuthenticationComponent,
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }