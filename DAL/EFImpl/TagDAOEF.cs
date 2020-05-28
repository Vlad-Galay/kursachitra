using MyBlogApp.DAL.DAOInterfaces;
using MyBlogApp.DAL.Entity;
using System.Collections.Generic;
using System.Linq;
using MyBlogApp.DAL.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MyBlogApp.DAL.EFImpl
{
    public class TagDAOEF : ITagRepo
    {
        private ApplicationDbContext dbContext;
        public TagDAOEF(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void AddTag(Tag tag)
        {
            if (tag == null)
                throw new NullArgumentDALException("Tag was null");
            dbContext.Tags.Add(tag);
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new DALException($"Can't add tag {ex.Message}");
            }
        }

        public void EditTag(int id, Tag newTag)
        {
            if (newTag == null)
                throw new NullArgumentDALException("New tag was null");
            var oldTag = GetTag(id);
            oldTag.Value = newTag.Value;
            try
            {
                dbContext.Tags.Update(oldTag);
                dbContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new DALException($"Can't edit tag {ex.Message}");
            }
        }

        public Tag GetTag(int id)
        {
            try
            {
                return dbContext.Tags.Where(t => t.Id == id).First();
            }
            catch
            {
                throw new DALException("Can't get tag");
            }
            
        }

        public IEnumerable<Tag> GetTags()
        {
            try
            {
                return dbContext.Tags;
            }
            catch
            {
                throw new DALException("Can't get tags");
            }
            
        }

        public IEnumerable<Tag> GetTagsOfCompany(int companyId)
        {
            var result = new List<Tag>();
            if (dbContext.Companys.Where(a => a.Id == companyId).Count() != 1)
                return result;
            var companyTags = dbContext.Companys.Include(a => a.CompanyTags).Where(a => a.Id == companyId).First()?.CompanyTags;
            if (companyTags == null)
                return result;
            foreach (var artTag in companyTags)
            {
                result.AddRange(dbContext.Tags.Where(t => t.Id == artTag.TagId));
            }
            return result;
        }

        public void RemoveTag(Tag tag)
        {
            dbContext.Tags.Remove(tag);
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new DALException($"Can't remove tag {ex.Message}");
            }
        }

        public void RemoveTag(int id)
        {
            RemoveTag(GetTag(id));
        }
    }
}
