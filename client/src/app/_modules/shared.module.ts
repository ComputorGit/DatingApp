import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastrModule } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs'
import {BsDatepickerModule} from 'ngx-bootstrap/datepicker'
import {PaginationModule} from 'ngx-bootstrap/pagination'


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    ToastrModule.forRoot({
      positionClass:'toast-bottom-right'
    }),
    TabsModule.forRoot(),
    BsDatepickerModule.forRoot(),
    PaginationModule.forRoot()
    
  ],
  exports :[
    CommonModule,
    ToastrModule,
    TabsModule,
    BsDatepickerModule,
    PaginationModule
  ]
})
export class SharedModule { }
