import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Company } from '../modules/company/models/company';
import { CompanyPageParams } from '../modules/company/models/company-page-params';

@Injectable({ providedIn: 'root' })
export class CompanyService {
  private currentAtricle: Company;
  private base = 'https://localhost:44382/api/companys';
  constructor(private http: HttpClient) { }

  addCompany(company: Company): Observable<any> {
    return this.http.post(this.base, company, {
      observe: 'response'
    });
  }

  setCurrent(company: Company) {
    this.currentAtricle = company;
  }
  getCurrent(): Company {
    return this.currentAtricle;
  }
  getCompanys(): Observable<any> {
    return this.http.get(this.base);
  }
  getCompanysFiltered(companyParams: CompanyPageParams): Observable<Company[]> {
    const query = this.base + '?';
    if (isNaN(companyParams.CategoryId)) {
      companyParams.CategoryId = -1;
    }
    if (!companyParams.Tags) {
      companyParams.Tags = '';
    }
    const pars = new HttpParams({
      fromObject: {
        PageSize: companyParams.pageSize.toString(),
        PageNumber: companyParams.pageNumber.toString(),
        CategoryId: companyParams.CategoryId.toString(),
        Tags: companyParams.Tags.toString(),
        MinDate: companyParams.MinDate.toString(),
        MaxDate: companyParams.MaxDate.toString(),
      }
    });
    console.log(query);
    return this.http.get<Company[]>(this.base + '?' + pars.toString());
  }
  editCompany(id: number, company: Company): Observable<any> {
    return this.http.put(this.base + '?id=' + id, company, {
      observe: 'response'
    });
  }
  removeCompany(id: number): Observable<any> {
    return this.http.delete(this.base + '?id=' + id, {
      observe: 'response'
    });
  }
}
