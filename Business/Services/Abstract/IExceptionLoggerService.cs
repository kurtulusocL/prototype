using Entities.Entities;

namespace Business.Services.Abstract
{
    public interface IExceptionLoggerService
    {
        Task<IEnumerable<ExceptionLogger>> GetAllAsync();
        Task<IEnumerable<ExceptionLogger>> GetAllWithoutParameterAsync();
        Task<ExceptionLogger> GetByIdAsync(int? id); 
        Task<bool> DeleteAsync(ExceptionLogger entity, int? id);
        Task<bool> SetActiveAsync(int id);
        Task<bool> SetDeActiveAsync(int id);
    }
}
