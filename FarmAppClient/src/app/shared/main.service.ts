import { User, ResponseBody } from './../models/allmodel';
import { Injectable, OnInit } from '@angular/core';
import { HttpClient, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { RequestBody } from '../models/allmodel';
import { timeout, finalize } from 'rxjs/operators';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BlockUI, NgBlockUI } from 'ng-block-ui';

@Injectable({
  providedIn: 'root'
})

export class MainService {

  constructor(private http: HttpClient, private router: Router, private toast: ToastrService) { }

  @BlockUI() blockUI: NgBlockUI;
  private getToken = 'http://localhost:5000/gettoken';


  loginUser(user: User) {
    this.blockUI.start('Авторизация...');
    const login: RequestBody  = new RequestBody();
    login.methodRoute = 'GetToken';
    login.param = JSON.stringify(user);
    console.log(login);
    return this.http.post(this.getToken, login, { observe: 'response' })
      .pipe(
        timeout(10000000),
        finalize(() => {
          this.blockUI.stop();
        })).subscribe(
          (res: HttpResponse<ResponseBody>) => {
            console.log(res);
            localStorage.setItem('token', res.body.result);
            this.router.navigateByUrl('/home');
            this.toast.success('Успешно', 'Аутентификация');
          },
          (err: HttpErrorResponse) => {
            console.log(err);
            if (err === null || err.status === 0) {
              this.toast.error('Сервер недоступен!', 'Ошибка!');
            } else {
              if (err.status === 400 || err.status === 404 || err.status === 500) {
                this.toast.error(err.error.result, err.error.header);
              } else if (err.statusText === 'TimeoutError' ) {
                this.toast.error('Превышен лимит ожидания ответа от сервера!', 'Ошибка!');
              } else {
                console.log(err);
              }
            }
          }
        );
  }

  getUser() {
    return this.http.get('http://localhost:5000/getuser');
  }

  getUsers() {
    return this.http.get('http://localhost:5000/getusers');
  }


  replacer(key: any, value: string) {
    key.value = key.value[0].toLowerCase();
    console.log(key);
    if (typeof key === 'string') {
      return value.toUpperCase();
    }
    return value;
  }

  // getUsers() {
  //   return this.http.get(this.url);
  // }

  // createUser(user: User) {
  //   return this.http.post(this.url, user);
  // }

  // updateUser(id: number, user: User) {
  //   const urlParams = new HttpParams().set('id', id.toString());
  //   return this.http.put(this.url, user, { params: urlParams});
  // }

  // deleteUser(id: number) {
  //     return this.http.delete(this.url + '/' + id);
  // }
}
