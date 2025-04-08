using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context.MSSQL;
using Entities.Entities.User;

namespace DataAccess.Concrete.EntityFramework
{
    public class UserRoleDal : EntityRepositoryBase<AppUserRole, ApplicationDbContext>, IUserRoleDal
    {
        public UserRoleDal(ApplicationDbContext context) : base(context)
        {
        }
    }
}
