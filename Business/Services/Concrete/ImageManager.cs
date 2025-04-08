using System.Linq.Expressions;
using Business.Services.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context.MSSQL;
using Entities.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Business.Services.Concrete
{
    public class ImageManager : IImageService
    {
        readonly IImageDal _imageDal;
        readonly IProductDal _productDal;
        readonly ApplicationDbContext _context;
        public ImageManager(IImageDal imageDal, ApplicationDbContext context, IProductDal productDal)
        {
            _imageDal = imageDal;
            _context = context;
            _productDal = productDal;
        }

        public async Task<Image> GetByProductIdForAddByIdAsync(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var product = await _productDal.GetAsync(i => i.Id == id);
            if (product != null)
            {
                var model = new Image
                {
                    ProductId = product.Id
                };
                return model;
            }
            return null;
        }

        public async Task<bool> CreateAsync(int? productId, IEnumerable<IFormFile> images)
        {
            if (productId == null)
            {
                throw new ArgumentNullException(nameof(productId));
            }
            if (images != null)
            {
                var errors = new List<string>();
                foreach (var file in images)
                {
                    var model = new Image
                    {
                        ProductId = productId
                    };

                    var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/product/multiImage/");
                    if (!Directory.Exists(directoryPath))
                    {
                        Console.WriteLine($"Path is preparing: {directoryPath}");
                        Directory.CreateDirectory(directoryPath);
                    }

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(directoryPath, fileName);
                    try
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        model.ImageUrl = fileName;
                        var result = await _imageDal.AddAsync(model);
                        if (!result)
                        {
                            errors.Add($"Error {fileName}.");
                        }
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"Error {fileName} : {ex.Message}");
                    }
                }
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(Image entity, int? id)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity was null");

            if (id == null)
                throw new ArgumentNullException(nameof(id), "Id was null");

            var data = await _imageDal.GetAsync(i => i.Id == id);
            if (data != null)
            {
                var result = await _imageDal.DeleteAsync(data);
                return result;
            }
            return false;
        }

        public async Task<IEnumerable<Image>> GetAllAsync()
        {
            try
            {
                var result = await _imageDal.GetAllByIncludeAsync(
                    new Expression<Func<Image, bool>>[]
                    {
                         i=>i.IsActive==true
                    }, null, i => i.Product);
                return result.OrderByDescending(i => i.CreatedDate).ToList();
            }
            catch (Exception)
            {
                return new List<Image>();
            }
        }

        public async Task<IEnumerable<Image>> GetAllImagesByProductIdAsync(int? productId)
        {
            try
            {
                if (productId == null)
                    throw new ArgumentNullException(nameof(productId), "Product Id was null");

                    var result = await _imageDal.GetAllIncludeByIdAsync(productId, "ProductId",
                        new Expression<Func<Image, bool>>[]
                        {
                            i=>i.IsActive == true
                        }, null, i => i.Product);
                return result;
            }
            catch (Exception)
            {
                return new List<Image>();
            }
        }

        public async Task<IEnumerable<Image>> GetAllWithoutParameterAsync()
        {
            try
            {
                var result = await _imageDal.GetAllByIncludeAsync(
                    new Expression<Func<Image, bool>>[]
                    {
                        
                    }, null, i => i.Product);
                return result.OrderByDescending(i => i.CreatedDate).ToList();
            }
            catch (Exception)
            {
                return new List<Image>();
            }
        }

        public async Task<Image> GetByIdAsync(int? id)
        {
            try
            {
                if (id == null)
                    throw new ArgumentNullException(nameof(id), "Id was null");

                return await _imageDal.GetByIncludeAsync(i => i.Id == id, i => i.Product);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while getting the entity.", ex);
            }
        }

        public async Task<bool> SetActiveAsync(int id)
        {
            try
            {
                var active = await _context.Set<Image>().Where(i => i.Id == id).FirstOrDefaultAsync();
                if (active != null)
                {
                    active.IsActive = true;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while setting Active the entity.", ex);
            }
        }

        public async Task<bool> SetDeActiveAsync(int id)
        {
            try
            {
                var active = await _context.Set<Image>().Where(i => i.Id == id).FirstOrDefaultAsync();
                if (active != null)
                {
                    active.IsActive = false;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while setting DeActive the entity.", ex);
            }
        }

        public async Task<bool> UpdateAsync(int? productId, int id, IFormFile image)
        {
            if (productId == null)
                throw new ArgumentNullException(nameof(productId), "Product Id was null");

            if (image != null)
            {
                var errors = new List<string>();
                var model = new Image
                {
                    ProductId = productId,
                    Id = id
                };
                if (model != null)
                {
                    var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/product/multiImage/");
                    if (!Directory.Exists(directoryPath))
                    {
                        Console.WriteLine($"Path is preparing: {directoryPath}");
                        Directory.CreateDirectory(directoryPath);
                    }
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    var filePath = Path.Combine(directoryPath, fileName);
                    try
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }
                        model.ImageUrl = fileName;
                        var result = await _imageDal.UpdateAsync(model);
                        if (!result)
                        {
                            errors.Add($"Error {fileName}.");
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"Error {fileName} : {ex.Message}");
                    }
                }
                return false;
            }
            return false;
        }
    }
}
