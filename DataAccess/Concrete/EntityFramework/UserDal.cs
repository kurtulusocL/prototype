using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context.MSSQL;
using Entities.Entities.User;

namespace DataAccess.Concrete.EntityFramework
{
    public class UserDal : EntityRepositoryBase<AppUser, ApplicationDbContext>, IUserDal
    {
        public UserDal(ApplicationDbContext context) : base(context)
        {
        }
    }
}
