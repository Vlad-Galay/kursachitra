import { Component, OnInit, AfterContentInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { CompanyService } from 'src/app/services/company-service';
import { Company } from '../../models/company';
import { CompanyPageParams } from '../../models/company-page-params';
import { Router } from '@angular/router';
import { CategoryService } from 'src/app/services/category-service';
import { Category } from 'src/app/modules/category/models/category';
import { Tag } from 'src/app/modules/tag/models/tag';
import { TagService } from 'src/app/services/tag-service';

@Component({
  selector: 'company-list',
  templateUrl: './company-list.component.html',
  styleUrls: ['./company-list.component.css']
})
export class CompanyListComponent implements OnInit {
  filterForm: FormGroup;
  invalidDatePicked = false;
  companys: Company[];
  tags: Tag[];
  selectedTags: Tag[];
  companyParams: CompanyPageParams;
  categories: Category[];
  hasCompanys = true;
  constructor(private categoryService: CategoryService, private companyService: CompanyService,
    private router: Router, private tagService: TagService) {
    this.companyParams = new CompanyPageParams();
    this.companyParams.pageSize = 5;

  }
  ngOnInit(): void {
    this.categoryService.getCategories().subscribe((result: Category[]) => {
      this.categories = result;
    }), error => console.log(error);
    this.tagService.getTags().subscribe((result: Tag[]) => {
      this.tags = result;
    });
    this.filterForm = new FormGroup({
      categoryId: new FormControl(),
      startDate: new FormControl(new Date()),
      endDate: new FormControl(new Date()),
      titleContains: new FormControl(),
      tagId: new FormControl(),
    });
    this.companyParams.pageNumber = 1;
    this.companyParams.CategoryId = -1;
    this.companyParams.Tags = '';
    this.companyParams.TitleContains = '';
    this.getPageCompanys();

  }
  getPageCompanys(): void {
    this.companyService.getCompanysFiltered(this.companyParams).subscribe((result: Company[]) => {
      this.companys = result;
      this.hasCompanys = false;
      if (this.companys) {
        if (this.companys.length >= 1) {
          this.hasCompanys = true;
        }
      }
    }), error => console.log(error);



  }
  nextPage() {
    this.companyParams.pageNumber++;
    this.getPageCompanys();
    window.scrollTo(0, 0);

  }
  filter() {
    this.companyParams.pageNumber = 1;
    const date: Date = this.filterForm.controls.startDate.value;
    if (!this.filterForm.value.categoryId) {
      this.companyParams.CategoryId = -1;
    } else {
      if ((this.filterForm.value.categoryId.value)) {
        if (typeof (this.filterForm.value.categoryId) == 'undefined') {
          this.companyParams.CategoryId = -1;
        } else {
          this.companyParams.CategoryId = Number.parseInt(this.filterForm.value.categoryId);
        }

      }
    }


    if (this.filterForm.controls.startDate) {
      this.companyParams.MinDate = this.filterForm.controls.startDate.value.valueOf;
    }
    if (this.filterForm.controls.endDate) {
      this.companyParams.MaxDate = this.filterForm.controls.endDate.value.valueOf;
    }
    if (this.companyParams.MaxDate < this.companyParams.MinDate) {
      this.invalidDatePicked = true;
      this.companyParams.MinDate = null;
      this.companyParams.MaxDate = null;
    }

    if (this.selectedTags) {
      this.companyParams.Tags = '';
      this.selectedTags.forEach(element => {
        this.companyParams.Tags += element.Id.toString() + ' ';
      });

    }

    console.log(this.companyParams);
    this.getPageCompanys();

  }
  prevPage() {
    if (this.companyParams.pageNumber > 1) {
      this.companyParams.pageNumber--;
    } else {
      return;
    }
    this.getPageCompanys();
    window.scrollTo(0, 0);
  }

  selectTag() {
    if (!this.selectedTags) {
      this.selectedTags = [];
    }
    const id = +this.filterForm.value.tagId;
    if (this.selectedTags.findIndex(t => t.Id == id) == -1) {
      this.selectedTags.push(this.tags.find(t => t.Id == id));
    }


  }
  unselectTag() {
    if (!this.selectedTags) {
      this.selectedTags = [];
    }
    const id = Number.parseInt(this.filterForm.controls.tagId.value);
    if (this.selectedTags.findIndex(t => t.Id == id) != -1) {
      this.selectedTags.splice(this.selectedTags.findIndex(t => t.Id == id), 1);
    }
  }

}
