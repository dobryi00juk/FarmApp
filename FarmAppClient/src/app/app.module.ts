import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './auth/auth.guard';


import { AppComponent } from './app.component';
import { MainService } from './shared/main.service';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { AuthInterceptor } from './auth/auth.interceptor';

import { CommonModule } from '@angular/common';
import * as Material from '@angular/material';

import { LayoutModule } from '@angular/cdk/layout';
import { BlockUIModule } from 'ng-block-ui';
import { MatConfirmDialogComponent } from './mat-confirm-dialog/mat-confirm-dialog.component';
import { UserComponent } from './home/users/user/user.component';
import { UsersComponent } from './home/users/users.component';
import { UserListComponent } from './home/users/user-list/user-list.component';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard],
    children: [
      { path: 'users', component: UsersComponent }
    ] }
];

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    MatConfirmDialogComponent,
    UserComponent,
    UsersComponent,
    UserListComponent,
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({ progressBar: true }),
    [RouterModule.forRoot(routes)],
    BlockUIModule.forRoot(),
    FormsModule,
    CommonModule,
    LayoutModule,
    Material.MatToolbarModule,
    Material.MatGridListModule,
    Material.MatFormFieldModule,
    Material.MatInputModule,
    Material.MatButtonModule,
    Material.MatCardModule,
    Material.MatIconModule,
    Material.MatSidenavModule,
    Material.MatSelectModule,
    Material.MatListModule,
    Material.MatDialogModule,
    Material.MatRadioModule,
    Material.MatCheckboxModule,
    Material.MatDatepickerModule,
    Material.MatNativeDateModule,
    Material.MatSnackBarModule,
    Material.MatTableModule,
    Material.MatPaginatorModule,
    Material.MatSortModule,
  ],
  exports: [
    [RouterModule],
    Material.MatToolbarModule,
    Material.MatGridListModule,
    Material.MatFormFieldModule,
    Material.MatInputModule,
    Material.MatButtonModule,
    Material.MatCardModule,
    Material.MatIconModule,
    Material.MatSidenavModule,
    Material.MatSelectModule,
    Material.MatListModule,
    Material.MatDialogModule,
    Material.MatRadioModule,
    Material.MatCheckboxModule,
    Material.MatDatepickerModule,
    Material.MatNativeDateModule,
    Material.MatSnackBarModule,
    Material.MatTableModule,
    Material.MatPaginatorModule,
    Material.MatSortModule,
  ],
  providers: [MainService, {
    provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
