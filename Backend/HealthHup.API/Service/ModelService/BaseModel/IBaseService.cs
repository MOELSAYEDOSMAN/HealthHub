using System.Linq.Expressions;

namespace HealthHup.API.Service.ModelService.BaseModel
{
    public interface IBaseService<T> where T : class
    {
        Task<bool> AddAsync(T input);
        Task<bool> RemoveAsync(T input);
        Task<bool> UpdateAsync(T input);
        Task SaveChaneAsync();
        Task<IList<T>> GetAllAsync(Expression<Func<T, object>>? OrderBy = null);
        Task<T> GetAsync(Guid Id, string[]? Inculde = null);
        Task<T> findAsync(Expression<Func<T, bool>> condation, string[]? inculde = null);
        Task<T> findAsNotTrakingync(Expression<Func<T, bool>> condation, string[]? inculde = null, bool? AsNotTraking = false);
        Task<IList<T>> findByAsync(Expression<Func<T, bool>> condation, string[]? inculde = null, Expression<Func<T, object>>? OrderBy = null);
        Task<IList<T>> findByExAsync(Expression<Func<T, bool>> condation, Expression<Func<T, object>>[]? include = null, Expression<Func<T, object>>? OrderBy = null);
        Task<int> CountAsync();
    }
}
