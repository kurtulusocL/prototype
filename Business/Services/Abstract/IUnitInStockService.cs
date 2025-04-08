using Entities.Entities;

namespace Business.Services.Abstract
{
    public interface IUnitInStockService
    {
        Task<IEnumerable<UnitInStock>> GetAllAsync();
        Task<IEnumerable<UnitInStock>> GetAllWithoutParameterAsync();
        Task<IEnumerable<UnitInStock>> GetAllStocksByUserIdAsync(string userId);
        Task<IEnumerable<UnitInStock>> GetAllStocksByProductIdAsync(int? productId);
        Task<UnitInStock> GetByIdAsync(int? id);
        Task<UnitInStock> GetByProductForAddByIdAsync(int? id);
        Task<bool> CreateAsync(int quantity, string warehouse, int? productId, string appUserId);
        Task<bool> UpdateAsync(int quantity, string warehouse, int? productId, string appUserId, int id);
        Task<bool> DeleteAsync(UnitInStock entity, int? id);
        Task<bool> SetActiveAsync(int id);
        Task<bool> SetDeActiveAsync(int id);
    }
}
