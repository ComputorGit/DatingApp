import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { unescapeIdentifier } from '@angular/compiler';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/internal/operators/map';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { PaginatedResult } from '../_models/Pagination';
import { UserParams } from '../_models/UserParams';

// const httpOptions = {
//   headers : new HttpHeaders({
//     Authorization : 'Bearer '+ JSON.parse(localStorage.getItem('user'))?.token
//   })
// }

@Injectable({
  providedIn: 'root'
})


export class MembersService {

  baseUrl = environment.apiUrl
  members : Member[] = []
  
  constructor(private _http : HttpClient) { }

  // getMembers(page?:number, itemsPerPage?:number){
    //  if(this.members.length > 0 ) return of(this.members)
    // return this._http.get<Member[]>(this.baseUrl + 'users').pipe(
    //   map( response =>{
    //     this.members = response
    //     return this.members
    //   })
    // )
    getMembers(userParams : UserParams){
    
      let params = this.getPaginationHeaders(userParams.pageNumber,userParams.pageSize);
      params = params.append('minAge',userParams.minAge.toString());
      params = params.append('maxAge',userParams.maxAge.toString());
      params = params.append('gender',userParams.gender.toString());
    
     return this.getOaginatedResult<Member[]>(this.baseUrl+ 'users', params)    ;
  }

  private getOaginatedResult<T>(url,params) {

    const paginatedResult : PaginatedResult<T> = new PaginatedResult<T>();
    return this._http.get<T>(url, { observe: 'response', params }).pipe(
      map(response => {
       paginatedResult.result = response.body;
        if (response.headers.get('Pagination') !== null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      }));
  }

  private getPaginationHeaders(pageNumber : number, pageSize : number){
    let params = new HttpParams();
    
      params = params.append('pageNumber',pageNumber.toString());
      params = params.append('pageSize',pageSize.toString());
      return params;
    }
  

  getMember(username : string){
    const member = this.members.find(x => x.userName === username)
    if(member != undefined) return of(member)

    //return this._http.get<Member>(this.baseUrl + 'users/'+ username, httpOptions)
     return this._http.get<Member>(this.baseUrl + 'users/'+ username)
  }

  updateMember(member : Member){
    return this._http.put(this.baseUrl + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    )
  }

  
}
