using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context.MSSQL;
using Entities.Entities;

namespace DataAccess.Concrete.EntityFramework
{
    public class ExceptionLoggerDal : EntityRepositoryBase<ExceptionLogger, ApplicationDbContext>, IExceptionLoggerDal
    {
        public ExceptionLoggerDal(ApplicationDbContext context) : base(context)
        {
        }
    }
}
