import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'client';
  appUser : any
  // constructor(private _http : HttpClient, private _accountService : AccountService){
    constructor(private _accountService : AccountService){

  }

  ngOnInit(){


      // this._http.get('https://localhost:5001/api/users').subscribe(response => {
      //   this.appUser = response
      // },error => { console.log("Error is produced");
      // })
      // console.log(this.appUser)

      this.setCurrentUser();
      }




  setCurrentUser(){
    const user : User = JSON.parse(localStorage.getItem('user'))
    this._accountService.setCurrentUser(user);
  }


}