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
    public class ProductDal : EntityRepositoryBase<Product, ApplicationDbContext>, IProductDal
    {
        public ProductDal(ApplicationDbContext context) : base(context)
        {
        }
    }
}
