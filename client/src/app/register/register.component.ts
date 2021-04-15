import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
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

  registerForm : FormGroup;

  constructor(private _accountService : AccountService, private _toastr : ToastrService, private fb: FormBuilder ) { }


  ngOnInit(): void {
    this.intializeForm();
  }

  intializeForm(){
    // this.registerForm = new FormGroup({
    //   username : new FormControl('',[Validators.required]),
    //   password : new FormControl('',[Validators.required,Validators.minLength(4),Validators.maxLength(8)]),
    //   confirmPassword : new FormControl('',[Validators.required,this.matchValues('password')])
    // })


    this.registerForm = this.fb.group({
      gender : ['male'],
      username : ['',[Validators.required]],
      knownAs : ['',[Validators.required]],
      dateOfBirth : ['',[Validators.required]],
      city : ['',[Validators.required]],
      country : ['',[Validators.required]],
      password : ['',[Validators.required,Validators.minLength(4),Validators.maxLength(8)]],
      confirmPassword : ['',[Validators.required,this.matchValues('password')]]
    })
    
    
  }

  matchValues(matchTo : string) : ValidatorFn{
    return (control : AbstractControl) => {
       return control?.value === control?.parent?.controls[matchTo].value ? null : {isMatching : true} 

      //ScenarioToCheck : if user revisit password and does not make any changes to confirm password, form status remains valid which is wrong .


      // if(control?.value === control?.parent?.controls[matchTo].value)
      //   {
      //     console.log("Matching DOne");
      //     return {isMatching : true}
      //   }
      // else{
      //   console.log("Matching NOt DOne");
      //   return {notMatching : false}
      // }
        
    }
  }

  register(){


    console.log(this.registerForm.value);
    // this._accountService.register(this.model).subscribe(response => {
    //   // console.log(response)
    // },error => {
    //   console.log(error)
    //   this._toastr.error(error.error);
    // })
  
  }

  cancel(){
    console.log('cancelled')
    this.cancelRegister.emit(false);
  }

}



//22Mar21, the commments made in ts and html file is done related to changes in reactive form 