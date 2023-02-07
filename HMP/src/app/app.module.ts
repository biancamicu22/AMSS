import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MDBBootstrapModule } from 'angular-bootstrap-md';
import { SharedModule } from './shared/shared.module';
import { AuthenticationComponent } from './authentication/authentication.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { AppSettings } from './app.settings';
import { UserService } from './shared/services/user.service';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { JwtInterceptor } from './shared/services/jwt.interceptor';
import { JwtModule } from '@auth0/angular-jwt';
import { ToastrModule } from 'ngx-toastr';
import { NgxUiLoaderModule } from 'ngx-ui-loader';
import { CustomLoaderService } from './shared/services/customLoader.service';
import { SprintComponent } from './sprint/sprint.component';
import { SprintListComponent } from './sprint/sprint-list/sprint-list.component';
import { SprintCreateComponent } from './sprint/sprint-create/sprint-create.component';
import { SprintService } from './shared/services/sprint.service';
import { TaskService } from './shared/services/task.service';
import { TaskComponent } from './task/task.component';
import { TaskListComponent } from './task/task-list/task-list.component';
import { TaskCreateComponent } from './task/task-create/task-create.component';
import { TaskUpdateComponent } from './task/task-update/task-update.component';

@NgModule({
  declarations: [
    AppComponent,
    AuthenticationComponent,
    SprintComponent,
    SprintListComponent,
    SprintCreateComponent,
    TaskComponent,
    TaskListComponent,
    TaskCreateComponent,
    TaskUpdateComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    ToastrModule.forRoot(),
    NgxUiLoaderModule,
    MDBBootstrapModule.forRoot(),
    JwtModule.forRoot({
      config: {
        blacklistedRoutes: []
      }
    })
  ],
  providers:  [
    AppSettings,
    CustomLoaderService,
    UserService,
    SprintService,
    TaskService,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }