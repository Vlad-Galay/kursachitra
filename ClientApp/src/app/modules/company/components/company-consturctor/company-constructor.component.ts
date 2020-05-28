import { Component, OnInit, AfterContentInit } from '@angular/core';
import { CategoryService } from 'src/app/services/category-service';
import { Category } from 'src/app/modules/category/models/category';
import { FormGroup, FormControl } from '@angular/forms';
import { CompanyService } from 'src/app/services/company-service';
import { Company } from '../../models/company';
import { TagService } from 'src/app/services/tag-service';
import { Tag } from 'src/app/modules/tag/models/tag';
import { CompanyTag } from '../../models/company-tag';
import { DialogOKComponent } from 'src/app/modules/dialogs/components/dialog-ok/dialog-ok.component';
import { DialogErrorComponent } from 'src/app/modules/dialogs/components/dialog-error/dialog-error.component';
import { MatDialog } from '@angular/material/dialog';
import { HttpHeaderResponse } from '@angular/common/http';


@Component({
  selector: 'company-constructor',
  templateUrl: './company-constructor.component.html',
  styleUrls: ['./company-constructor.component.css']
})
export class CompanyConstructorComponent implements OnInit, AfterContentInit {
  private dialogConfig;
  public tabInd = 0;
  public companyForm: FormGroup;
  public companyListForm: FormGroup;
  private categories: Category[];
  private companys: Company[];
  private tags: Tag[];
  private companyTags: Tag[] = [];
  public modes = ['New Company', 'Edit Company'];
  public formMode = 'New company';
  constructor(private categoryService: CategoryService, private companySerice: CompanyService,
    private tagService: TagService, private dialog: MatDialog) {

  }
  ngOnInit(): void {
    this.categoryService.getCategories().subscribe((result: Category[]) => {
      this.categories = result;
    }), error => console.log(error);

    this.tagService.getTags().subscribe((result: Tag[]) => {
      this.tags = result;
    }), error => console.log(error);

    this.companyForm = new FormGroup({
      title: new FormControl(),
      content: new FormControl(),
      description: new FormControl(),
      picpass: new FormControl(),
      categoryId: new FormControl(),
      tagId: new FormControl(),
      pictureUrl: new FormControl(),
    });
    this.companyListForm = new FormGroup({
      companyId: new FormControl(),
    });
    this.getCompanys();

    this.dialogConfig = {
      height: '200px',
      width: '400px',
      disableClose: true,
      data: {}
    };

  }
  ngAfterContentInit() {

  }
  public hasError = (controlName: string, errorName: string) => {
    return this.companyForm.controls[controlName].hasError(errorName);
  }
  public onCancel = () => {

  }
  public clearForm() {


  }
  public performAction(formValue: FormGroup) {
    const company: Company = new Company();
    const category = this.categories.find(x => x.Id === formValue.controls.categoryId.value);
    company.Category = category;
    company.Content = formValue.controls.content.value;
    company.Title = formValue.controls.title.value;
    company.Description = formValue.controls.description.value;
    company.PicsUrl = formValue.controls.pictureUrl.value;
    company.Id = 0;
    if (this.companyTags.length > 0) {
      company.CompanyTags = [];
      this.companyTags.forEach(element => {
        const companyTag = new CompanyTag();
        companyTag.TagId = element.Id;
        companyTag.CompanyId = 0;
        company.CompanyTags.push(companyTag);
      });
    }
    if (this.formMode == 'New company') {
      if (!this.CheckUniqueTitleValue(company.Title)) {
        this.showDialog(true, "Can't add tag", 'Company with that title already exists');
        return;
      }
      this.companySerice.addCompany(company).subscribe((result: HttpHeaderResponse) => {
        if (result.status === 200) {
          this.showDialog(true, 'Add ok', 'Company was added');
          this.getCompanys();
          this.clearForm();
          this.tabInd = 1;
          return;
        } else {
          this.showDialog(false, "Can't add company", 'company was not added');
          return;
        }
      });
    }
    if (this.formMode == 'Edit company') {
      company.Id = this.companyListForm.controls.companyId.value;
      company.Publish4Time = null;
      this.companySerice.editCompany(company.Id, company).subscribe((result: HttpHeaderResponse) => {
        if (result.status === 200) {
          this.showDialog(true, 'Edit ok', 'Company was edited');
          this.getCompanys();
          this.clearForm();
          this.tabInd = 1;
          return;
        } else {
          this.showDialog(false, "Can't edit company", 'company was not edited');
          return;
        }
      });
    }

  }
  public addTag(formValue: FormGroup) {
    if (this.companyTags.findIndex(x => x.Id == formValue.controls.tagId.value) == -1) {
      this.companyTags.push(this.tags.find(t => t.Id == formValue.controls.tagId.value))
    }

  }
  public removeTag(formValue: FormGroup) {
    if (this.companyTags.findIndex(x => x.Id == formValue.controls.tagId.value) != -1) {
      this.companyTags.splice(this.companyTags.findIndex(t => t.Id == formValue.controls.tagId.value), 1);
    }
    console.log(this.companyTags);
  }

  public getCompanys() {
    this.companySerice.getCompanys().subscribe(res => {
      this.companys = res;
    }), error => console.log(error);

  }
  public showDialog(ok: boolean, title: String, message: String) {
    this.dialogConfig.data = {
      'message': message,
      'title': title
    };
    if (ok) {
      const dialogRef = this.dialog.open(DialogOKComponent, this.dialogConfig);
    } else {
      const dialogRef = this.dialog.open(DialogErrorComponent, this.dialogConfig);
    }

  }
  public CheckUniqueTitleValue(value: String): boolean {
    for (const company of this.companys) {
      if (company.Title == value) {
        return false;
      }
    }
    return true;
  }
  public goNew() {
    this.tabInd = 0;
    this.formMode = 'New company';
  }
  public goEdit() {
    this.formMode = 'Edit company';
    this.tabInd = 0;
    const companyToEdit = this.companys.find(a => a.Id == this.companyListForm.controls.companyId.value);
    this.companyForm.controls.title.setValue(companyToEdit.Title);
    this.companyForm.controls.content.setValue(companyToEdit.Content);
    this.companyForm.controls.description.setValue(companyToEdit.Description);
    this.companyTags = [];
    companyToEdit.CompanyTags.forEach(element => {
      this.companyTags.push(this.tags.find(t => t.Id == element.TagId));
    });
    this.companyForm.controls.categoryId.setValue(companyToEdit.Category.Id);

  }
  public goDelete() {
    this.formMode = 'Delete company';
    this.companySerice.removeCompany(this.companyListForm.controls.companyId.value).subscribe((result: HttpHeaderResponse) => {
      if (result.status === 200) {
        this.showDialog(true, 'Delete ok', 'Company was deleted');
        this.getCompanys();
        this.clearForm();
        this.tabInd = 1;
        return;
      } else {
        this.showDialog(false, "Can't delete company", 'company was not deleted');
        return;
      }
    });
  }

}
