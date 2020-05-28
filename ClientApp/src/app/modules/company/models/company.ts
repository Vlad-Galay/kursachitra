import { Category } from "../../category/models/category";
import { Tag } from "../../tag/models/tag";
import { CompanyTag } from "./company-tag";

export class Company {
    Id:number;
    Title:String;
    Description:String;
    Content:String;
    Category:Category;
    PicsUrl:String;
    Publish4Time:Date;
    CompanyTags:CompanyTag[];
    constructor(){
        this.CompanyTags = [];
        this.Id = 0;
        this.Title = '';
        this.Publish4Time = null;
        this.PicsUrl ='';
        
    }
    static cloneBase(base: Company): Company {
        const result = new Company();
        result.Title = base.Title;
        result.Description = base.Description;
        result.Content = base.Content;
        if (base.CompanyTags == null){
            return result;
        }
        base.CompanyTags.forEach( tag => {
          result.CompanyTags.push(tag);
        });
        return result;
      }
}
