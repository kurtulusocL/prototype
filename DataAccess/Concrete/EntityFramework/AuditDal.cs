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
    public class AuditDal : EntityRepositoryBase<Audit, ApplicationDbContext>, IAuditDal
    {
        public AuditDal(ApplicationDbContext context) : base(context)
        {
        }
    }
}
