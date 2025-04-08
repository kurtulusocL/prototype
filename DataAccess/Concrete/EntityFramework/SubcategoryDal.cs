using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context.MSSQL;
using Entities.Entities;

namespace DataAccess.Concrete.EntityFramework
{
    public class SubcategoryDal : EntityRepositoryBase<Subcategory, ApplicationDbContext>, ISubcategoryDal
    {
        public SubcategoryDal(ApplicationDbContext context) : base(context)
        {
        }
    }
}
