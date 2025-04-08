using Business.Services.Abstract;
using Business.Services.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.Extensions.DependencyInjection;

namespace Business.DependencyResolver.DependencyInjection
{
    public static class DependencyContainer
    {
        public static void DependencyService(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthManager>();
            services.AddScoped<ICacheService, CacheManager>();

            services.AddScoped<IAuditDal, AuditDal>();
            services.AddScoped<IAuditService, AuditManager>();

            services.AddScoped<IExceptionLoggerDal, ExceptionLoggerDal>();
            services.AddScoped<IExceptionLoggerService, ExceptionLoggerManager>();

            services.AddScoped<ICategoryDal, CategoryDal>();
            services.AddScoped<ICategoryService, CategoryManager>();

            services.AddScoped<IImageDal, ImageDal>();
            services.AddScoped<IImageService, ImageManager>();

            services.AddScoped<IProductDal,ProductDal>();
            services.AddScoped<IProductService, ProductManager>();

            services.AddScoped<ISubcategoryDal, SubcategoryDal>();
            services.AddScoped<ISubcategoryService, SubcategoryManager>();

            services.AddScoped<IUnitInStockDal, UnitInStockDal>();
            services.AddScoped<IUnitInStockService, UnitInStockManager>();

            services.AddScoped<IUserDal, UserDal>();
            services.AddScoped<IUserService, UserManager>();

            services.AddScoped<IUserRoleDal, UserRoleDal>();
            services.AddScoped<IUserRoleService, UserRoleManager>();

            //services.AddHostedService<UpdateStoryImageStatusService>();
        }
    }
}
