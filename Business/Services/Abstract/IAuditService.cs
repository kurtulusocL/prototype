using Entities.Entities;

namespace Business.Services.Abstract
{
    public interface IAuditService
    {
        Task<IEnumerable<Audit>> GetAllIncludeAsync();
        Task<IEnumerable<Audit>> GetAllIncludeWithoutParameterAsync();
        Task<Audit> GetByIdAsync(int? id);
        Task<bool> DeleteAsync(Audit entity, int? id);
        Task<bool> SetActiveAsync(int id);
        Task<bool> SetDeActiveAsync(int id);
    }
}
