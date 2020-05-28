using MyBlogApp.BLL.Interfaces;
using MyBlogApp.DAL.Entity;
using MyBlogApp.DAL.Entity.Infrastructure;
using MyBlogApp.DAL;
using MyBlogApp.BLL.Exceptions;
using MyBlogApp.DAL.Exceptions;
namespace MyBlogApp.BLL.ServiceImpl
{
    public class CompanyService : ICompanyService
    {
        private DAOFactory daoFactory;
        public CompanyService(DAOFactory daoFactory) 
        {
            this.daoFactory = daoFactory;
        }

        public void AddCompany(Company company)
        {
            if (company == null)
                throw new ServiceNullArgumentException("company argument was null");
            try
            {
                daoFactory.GetCompanyRepo().AddCompany(company);
            }
            catch (DALException ex)
            {
                throw new ServiceException($"DAL exception : {ex.Message}");
            }
            
        }

        public Company GetCompany(int id)
        {
            try
            {
                return daoFactory.GetCompanyRepo().GetCompany(id);
            }
            catch (DALException ex)
            {
                throw new ServiceException($"DAL exception : {ex.Message}");
            }
            
        }

        public PagedList<Company> GetCompanys(CompanyQueryParameters parameters)
        {   
            if (parameters == null)
                throw new ServiceNullArgumentException("companyqueryparams was null");
            try
            {
                return daoFactory.GetCompanyRepo().GetCompanys(parameters);
            }
            catch (DALException ex)
            {
                throw new ServiceException($"DAL exception : {ex.Message}");
            }
            
        }

        public void RemoveCompany(int id)
        {
            try
            {
                daoFactory.GetCompanyRepo().RemoveCompany(id);
            }
            catch (DALException ex)
            {
                throw new ServiceException($"DAL exception : {ex.Message}");
            }
            
        }

        public void RemoveCompany(Company company)
        {
            if (company == null)
                return;
            try
            {
                daoFactory.GetCompanyRepo().RemoveCompany(company.Id);
            }
            catch (DALException ex)
            {
                throw new ServiceException($"DAL exception : {ex.Message}");
            }
            
        }

        public void EditCompany(int id, Company newCompany)
        {
            try
            {
                daoFactory.GetCompanyRepo().EditCompany(id, newCompany);
            }
            catch (DALException ex)
            {
                throw new ServiceException($"DAL exception : {ex.Message}");
            }
            
        }
    }
}
