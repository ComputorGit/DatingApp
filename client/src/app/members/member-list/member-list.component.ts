import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/Pagination';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/UserParams';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  //Adding Filter and these comment is to repush the code to github

  members$ : Observable<Member[]>;
  members : Member[];
  pagination : Pagination;
  userParams : UserParams;
  user : User;
  genderList = [{value : 'male', display:'males'},{value : 'female', display:'females'}]

  constructor(private memberService : MembersService, private accountService : AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user=>{
      this.user = user;
      this.userParams = new UserParams(user);
    })
   }

  ngOnInit(): void {
    // this.members$ = this.memberService.getMembers();
    this.loadMembers();
  }

  loadMembers(){

    console.log("Loading Member");
    this.memberService.getMembers(this.userParams).subscribe(response => {
      console.log(response);
      this.members = response.result;
      this.pagination = response.pagination;

    })
  }
  resetFilters(){
    this.userParams = new UserParams(this.user);
    return this.userParams

  }

  pageChanged(event : any){
    this.userParams.pageNumber = event.page;
    this.loadMembers();
  }

}
