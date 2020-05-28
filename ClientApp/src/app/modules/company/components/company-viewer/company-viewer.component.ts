import { Component, OnInit, Input } from '@angular/core';
import { Company } from '../../models/company';
import { CompanyService } from 'src/app/services/company-service';
import { Router } from '@angular/router';
import { TagService } from 'src/app/services/tag-service';
import { Tag } from 'src/app/modules/tag/models/tag';

@Component({
  selector: 'company-viewer',
  templateUrl: 'company-viewer.component.html',
  styleUrls: ['company-viewer.component.css'],
})
export class CompanyViewerComponent implements OnInit {
  public url: String = 'https://localhost:44382/company-constructor';
  public company: Company;
  public companyTags: Tag[];
  constructor(private companyService: CompanyService, private router: Router, private tagService: TagService) {

  }
  ngOnInit(): void {
    this.company = this.companyService.getCurrent();
    this.tagService.getTagsOfCompany(this.company.Id).subscribe(result => {
      this.companyTags = result;
    });
    console.log(this.company);
    console.log(this.companyTags);

  }
  companyClick() {
  }

}
