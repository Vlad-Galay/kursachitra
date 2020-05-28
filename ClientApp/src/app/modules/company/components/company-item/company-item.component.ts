import { Component, OnInit, Input } from '@angular/core';
import { Company } from '../../models/company';
import { CompanyService } from 'src/app/services/company-service';
import { Router } from '@angular/router';

/**
 * @title Card with multiple sections
 */
@Component({
  selector: 'company-item',
  templateUrl: 'company-item.component.html',
  styleUrls: ['company-item.component.css']
})
export class CompanyItemComponent implements OnInit {
  public url: String = 'https://localhost:44382/company';
  @Input()
  public company: Company;
  constructor(private companyService: CompanyService, private router: Router) {}
  ngOnInit(): void {
    console.log(this.company);
  }
  openDetails() {
    console.log('clicked');
    this.companyService.setCurrent(this.company);
    this.router.navigate(['/company']);
  }
}
