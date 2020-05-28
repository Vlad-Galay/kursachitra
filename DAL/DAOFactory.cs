using MyBlogApp.DAL.DAOInterfaces;

namespace MyBlogApp.DAL
{
    public class DAOFactory
    {
        private ICategoryRepo categoryRepo;
        private ICompanyRepo companyRepo;
        private ITagRepo tagRepo;
        public DAOFactory(ICategoryRepo categoryRepo, ICompanyRepo companyRepo, ITagRepo tagRepo)
        {
            this.tagRepo = tagRepo;
            this.categoryRepo = categoryRepo;
            this.companyRepo = companyRepo;
        }
        public ICategoryRepo GetCategoryRepo()
        {
            return categoryRepo;
        }
        public ICompanyRepo GetCompanyRepo()
        {
            return companyRepo;
        }
        public ITagRepo GetTagRepo()
        {
            return tagRepo;
        }
        
    }
}
