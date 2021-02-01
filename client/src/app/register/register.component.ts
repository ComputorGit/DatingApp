import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model : any ={};

  @Input() userFromHomeComponent : any
  @Output() cancelRegister = new EventEmitter(); //make sure it is getting import from angular/core

  constructor(private _accountService : AccountService, private _toastr : ToastrService ) { }


  ngOnInit(): void {
  }

  register(){
    this._accountService.register(this.model).subscribe(response => {
      console.log(response)
    },error => {
      console.log(error)
      this._toastr.error(error.error);
    })
  
  }

  cancel(){
    console.log('cancelled')
    this.cancelRegister.emit(false);
  }

}
