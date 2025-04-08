using Entities.Entities;
using Microsoft.AspNetCore.Http;

namespace Business.Services.Abstract
{
    public interface IImageService
    {
        Task<IEnumerable<Image>> GetAllAsync();
        Task<IEnumerable<Image>> GetAllWithoutParameterAsync();
        Task<IEnumerable<Image>> GetAllImagesByProductIdAsync(int? productId);
        Task<Image> GetByIdAsync(int? id);
        Task<Image> GetByProductIdForAddByIdAsync(int? id);
        Task<bool> CreateAsync(int? productId, IEnumerable<IFormFile> images);
        Task<bool> UpdateAsync(int? productId, int id, IFormFile image);
        Task<bool> DeleteAsync(Image entity, int? id);
        Task<bool> SetActiveAsync(int id);
        Task<bool> SetDeActiveAsync(int id);
    }
}
