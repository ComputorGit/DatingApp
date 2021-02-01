import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { ObserveOnSubscriber } from 'rxjs/internal/operators/observeOn';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  isNavbarCollapsed=true;
  currentUser$ : Observable<User>

  model:any ={}
  constructor(public _accountService : AccountService,private _router : Router,
              private _toastr : ToastrService) { }

  ngOnInit(): void {
    //this.getCurrentUser();
    this.currentUser$ = this._accountService.currentUser$;
  }

  login(){
    this._accountService.login(this.model).subscribe(response => {
      this._router.navigateByUrl('/members');
    }, error =>{
      console.log(error)
      this._toastr.error(error.error);
    })
  }

  logout(){
    this._accountService.logout();
    this._router.navigateByUrl('');
  }

  // getCurrentUser(){
  //   this._accountService.currentUser$.subscribe(user => {
  //     this.loggedIn = !!user; //!! turn object into the boolean value based on the null property
  //   })
  // }

}
