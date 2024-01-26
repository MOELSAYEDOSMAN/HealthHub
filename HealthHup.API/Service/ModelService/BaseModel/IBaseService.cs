using System.Linq.Expressions;

namespace HealthHup.API.Service.ModelService.BaseModel
{
    public interface IBaseService<T> where T : class
    {
        Task<bool> AddAsync(T input);
        Task<bool> RemoveAsync(T input);
        Task<bool> UpdateAsync(T input);
        Task SaveChaneAsync();
        Task<IList<T>> GetAll();
        Task<T> Get(Guid Id, string[]? Inculde = null);
        Task<T> find(Expression<Func<T, bool>> condation, string[] inculde = null);
        Task<IList<T>> findBy(Expression<Func<T, bool>> condation, string[] inculde = null);
        Task<int> CountAsync();
    }
}
