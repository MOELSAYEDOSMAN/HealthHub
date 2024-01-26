using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HealthHup.API.Service.ModelService.BaseModel
{
    public class BaseService<T>:IBaseService<T> where T : class
    {
        private readonly ApplicatoinDataBaseContext _db;
        public BaseService(ApplicatoinDataBaseContext db)
        {
            _db = db;
        }
        public async Task<bool> AddAsync(T input)
        {
            await _db.Set<T>().AddAsync(input);
            await SaveChaneAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(T input)
        {
            _db.Set<T>().Update(input);
            await SaveChaneAsync();
            return true;
        }
        public async Task<bool> RemoveAsync(T input)
        {
            _db.Set<T>().Remove(input);
            await SaveChaneAsync();
            return true;
        }

        public async Task<T> Get(Guid Id, string[]? Inculde = null)
        {
            var table = _db.Set<T>();
            if (Inculde == null)
                foreach (var inc in Inculde)
                    table.Include(inc);
            return await table.FindAsync(Id);
        }

        public async Task<IList<T>> GetAll()
        => await _db.Set<T>().ToListAsync();
        public async Task<T> find(Expression<Func<T, bool>> condation, string[] inculde = null)
        {
            IQueryable<T> query = _db.Set<T>();
            if (inculde != null)
                foreach (var incluse in inculde)
                    query = query.Include(incluse);
            return await query.SingleOrDefaultAsync(condation);
        }

        public async Task<IList<T>> findBy(Expression<Func<T, bool>> condation, string[] inculde = null)
        {
            IQueryable<T> query = _db.Set<T>();
            if (inculde != null)
                foreach (var incluse in inculde)
                    query = query.Include(incluse);
            return await query.Where(condation).ToListAsync();
        }
        public async Task<int> CountAsync()
            => await _db.Set<T>().CountAsync();
        public async Task SaveChaneAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}