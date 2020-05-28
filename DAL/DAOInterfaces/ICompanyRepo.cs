using MyBlogApp.DAL.Entity;
using MyBlogApp.DAL.Entity.Infrastructure;
namespace MyBlogApp.DAL.DAOInterfaces
{
    public interface ICompanyRepo
    {
        PagedList<Company> GetCompanys(CompanyQueryParameters parameters);
        void AddCompany(Company company);
        Company GetCompany(int id);
        void RemoveCompany(int id);
        void EditCompany(int oldCompanyId, Company newCompany);
    }
}
