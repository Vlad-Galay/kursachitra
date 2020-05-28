using MyBlogApp.DAL.Entity;
using MyBlogApp.DAL.Entity.Infrastructure;

namespace MyBlogApp.BLL.Interfaces
{
    public interface ICompanyService
    {
        public PagedList<Company> GetCompanys(CompanyQueryParameters parameters);
        public Company GetCompany(int id);
        public void RemoveCompany(int id);
        public void AddCompany(Company company);
        public void EditCompany(int id, Company newCompany);
        
    }
}
