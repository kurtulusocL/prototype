using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context.MSSQL;
using Entities.Entities;

namespace DataAccess.Concrete.EntityFramework
{
    public class CategoryDal : EntityRepositoryBase<Category, ApplicationDbContext>, ICategoryDal
    {
        public CategoryDal(ApplicationDbContext context) : base(context)
        {
        }
    }
}
