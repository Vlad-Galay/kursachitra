using MyBlogApp.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlogApp.BLL
{
    public class ServiceFactory
    {
        private ICompanyService companyService;
        private ITagService tagService;
        private ICategoryService categoryService;
        
        public ServiceFactory(ICompanyService companyService, ITagService tagService, ICategoryService categoryService)
        {
            this.tagService = tagService;
            this.categoryService = categoryService;
            this.companyService = companyService;
        }
        public ICompanyService GetCompanyService() 
        {
            return companyService;
        }
    }
}
