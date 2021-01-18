import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'client';
  appUser : any
  constructor(private _http : HttpClient){

  }

  ngOnInit(){
this._http.get('https://localhost:5001/api/users').subscribe(response => {
  this.appUser = response
},error => { console.log("Error is produced");
})
console.log(this.appUser)
  }



}