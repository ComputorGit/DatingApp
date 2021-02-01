import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  userAtHomeComponent : any;
  constructor(private _http:HttpClient) { }

  ngOnInit(): void {
    this.getUsers();
  }

  registerToggle(){
    this.registerMode = !this.registerMode
  }

  getUsers(){

    this._http.get('https://localhost:5001/api/users').subscribe(userResponse => this.userAtHomeComponent = userResponse);
    
    //  this._http.get('https://localhost:5001/api/users').subscribe(response => {
    //    console.log(response)
    //     this.userAtHomeComponent = response
    //     console.log(this.userAtHomeComponent)
    //   },error => { console.log("Error is produced");
    //   })
    //   console.log(this.userAtHomeComponent)

  }

  cancelRegisterMode(event:boolean){
    this.registerMode = event;
  }

}
