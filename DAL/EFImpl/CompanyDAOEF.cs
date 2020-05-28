using Microsoft.EntityFrameworkCore;
using MyBlogApp.DAL.DAOInterfaces;
using MyBlogApp.DAL.Entity;
using MyBlogApp.DAL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using MyBlogApp.DAL.Entity.Infrastructure;

namespace MyBlogApp.DAL.EFImpl
{
    public class CompanyDAOEF : ICompanyRepo
    {
        private ApplicationDbContext dbContext;
        public CompanyDAOEF(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void AddCompany(Company company)
        {
            if (company == null)
                throw new NullArgumentDALException("Company argument was null");

            if (company.CompanyTags == null)
                company.CompanyTags = new List<CompanyTag>();

            company.PublishTime = DateTime.Now;
            dbContext.Companys.Add(company);
            dbContext.Attach<Category>(company.Category);
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new DALException($"Error while updating db {ex.Message}");
            }

        }
        public void EditCompany(int oldCompanyId, Company newCompany)
        {
            if (newCompany == null)
                throw new NullArgumentDALException("New company was null");
            var company = dbContext.Companys.Where(a => a.Id == oldCompanyId).Include(a => a.CompanyTags).First();
            if (company == null)
                throw new DALException($"Can't find company with id:{oldCompanyId}");

            company.Category = newCompany.Category;
            company.Content = newCompany.Content;
            company.Description = newCompany.Description;
            company.Title = newCompany.Title;
            company.PicsUrl = newCompany.PicsUrl;
            
            newCompany.Id = oldCompanyId;
            if (company.CompanyTags == newCompany.CompanyTags)
            {
                foreach (var item in company.CompanyTags)
                {
                    dbContext.Attach<CompanyTag>(item);
                }
            }
            else
            {
                company.CompanyTags = null;
                try
                {
                    dbContext.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    throw new DALException($"Error while updating company {ex.Message}");
                }
                company.CompanyTags = newCompany.CompanyTags;
            }
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new DALException($"Error while updating company {ex.Message}");
            }
        }

        public Company GetCompany(int id)
        {
            try
            {
                return dbContext.Companys.Where(a => a.Id == id).First();
            }
            catch
            {
                throw new DALException("Can't get company");
            }
            
        }
        public PagedList<Company> GetCompanys(CompanyQueryParameters parameters)
        {
            static double ConvertToUnixTimestamp(DateTime date)
            {
                DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                TimeSpan diff = date.ToUniversalTime() - origin;
                return Math.Floor(diff.TotalMilliseconds);
            }
            IEnumerable<Company> companys = null;
            try
            {
                companys = dbContext.Companys.Include(a => a.Category).Include(a => a.CompanyTags);
            }
            catch
            {
                throw new DALException("Can't get companys");
            }

            if (parameters.CategoryId != -1)
                companys = companys.Where(a => a.Category.Id == parameters.CategoryId);
   
            if (parameters.TitleContains?.Length != 0)
                companys = companys.Where(a => a.Title.Contains(parameters.TitleContains));

            if (parameters.MaxDate != 1)
            {
                companys = companys.Where(a => ConvertToUnixTimestamp(a.PublishTime) >= parameters.MinDate && ConvertToUnixTimestamp(a.PublishTime) <= parameters.MaxDate);
            }
            

            if (parameters.Tags != null && parameters.Tags?.Length != 0)
            {
                var companysWithTags = new List<Company>();
                var tags = parameters.Tags.Split(' ');
                var tagsIds = new List<int>();
                int value;
                bool parsed;
                foreach (var tag in tags)
                {
                    parsed = int.TryParse(tag, out value);
                    if (parsed)
                    {
                        tagsIds.Add(value);
                    }
                }

                bool containAllTags;
                foreach (var company in companys)
                {
                    containAllTags = true;
                    foreach (var tagId in tagsIds)
                    {
                        if (!company.CompanyTags.Any(a => a.TagId == tagId))
                        {
                            containAllTags = false;
                        }
                    }
                    if (containAllTags)
                    {
                        companysWithTags.Add(company);
                    }
                }

                companys = companysWithTags;
            }

            return PagedList<Company>.ToPagedList(companys,
                parameters.PageNumber,
                parameters.PageSize);
        }

        public void RemoveCompany(Company company)
        {
            dbContext.Companys.Remove(company);
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new DALException("Can't delete company" + ex.Message);
            }
            
        }

        public void RemoveCompany(int id)
        {
            RemoveCompany(GetCompany(id));
        }
    }
}
