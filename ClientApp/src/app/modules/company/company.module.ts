import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { CompanyConstructorComponent } from './components/company-consturctor/company-constructor.component';
import { CompanyItemComponent } from './components/company-item/company-item.component';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CompanyListComponent } from './components/company-list/company-list.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MatButtonModule} from '@angular/material/button';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatIconModule} from '@angular/material/icon';
import { CompanyViewerComponent } from './components/company-viewer/company-viewer.component';
import {MatTabsModule} from '@angular/material/tabs';
import {MatDatepickerModule} from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MyDialogModule } from '../dialogs/dialogs.module';
@NgModule({
  declarations: [
    CompanyConstructorComponent,
    CompanyItemComponent,
    CompanyListComponent,
    CompanyViewerComponent,
  ],
  imports: [
    BrowserModule,
    RouterModule,
    HttpClientModule,
    MatCardModule,
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatToolbarModule,
    MatIconModule,
    MatTabsModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MyDialogModule
  ],
  providers: [],
  exports: [CompanyConstructorComponent,CompanyItemComponent,CompanyListComponent,CompanyViewerComponent]
})

export class CompanyModule {
}
