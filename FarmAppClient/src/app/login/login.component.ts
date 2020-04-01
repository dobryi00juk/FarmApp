import { Router } from '@angular/router';
import { MainService } from '../shared/main.service';
import { Component, OnInit } from '@angular/core';
import { User } from '../models/allmodel';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})

export class LoginComponent implements OnInit {
  constructor(private router: Router, private service: MainService) {}

  loginModel: User = new User();

  ngOnInit(): void {
    if (localStorage.getItem('token') != null) {
      this.router.navigateByUrl('/home');
    }
  }

  onSubmit() {
    this.service.loginUser(this.loginModel);
  }

}
