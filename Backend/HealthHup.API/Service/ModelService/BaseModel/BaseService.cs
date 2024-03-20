using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace HealthHup.API.Service.ModelService.BaseModel
{
    public class BaseService<T>:IBaseService<T> where T : class
    {
        public readonly ApplicatoinDataBaseContext _db;
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
           
            Console.WriteLine("RemoveBase");

            _db.Set<T>().Remove(input);
            await SaveChaneAsync();
            return true;
        }
        public async Task<bool> RemoveRangeAsync(List<T> Items)
        {
            _db.Set<T>().RemoveRange(Items);
            await SaveChaneAsync();
            return true;
        }
        public async Task<T> GetAsync(Guid Id, string[]? Inculde = null)
        {
            var table = _db.Set<T>();
            if (Inculde != null)
                foreach (var inc in Inculde)
                    table.Include(inc);
            return await table.FindAsync(Id);
        }

        public async Task<IList<T>> GetAllAsync(Expression<Func<T, object>>? OrderBy = null)
        {
            IQueryable<T> query = _db.Set<T>();
            if (OrderBy != null)
                query = query.OrderBy(OrderBy);
            return await query.ToListAsync();
        }
        public async Task<T> findAsync(Expression<Func<T, bool>> condation, string[]? inculde = null)
        {
            IQueryable<T> query = _db.Set<T>();
            if (inculde != null)
                foreach (var incluse in inculde)
                    query = query.Include(incluse);
            return await query.SingleOrDefaultAsync(condation);
        }

        public async Task<T> findAsNotTrakingync(Expression<Func<T, bool>> condation, string[]? inculde = null,bool? AsNotTraking=false)
        {
            IQueryable<T> query = _db.Set<T>();
            if (AsNotTraking??false)
                query = query.AsNoTracking();
            if (inculde != null)
                foreach (var incluse in inculde)
                    query = query.Include(incluse);
            return await query.SingleOrDefaultAsync(condation);
        }

        public async Task<IList<T>> findByAsync(Expression<Func<T, bool>> condation, string[]? inculde = null, Expression<Func<T, object>>? OrderBy = null)
        {
            IQueryable<T> query = _db.Set<T>();
            if (inculde != null)
                foreach (var incluse in inculde)
                    query = query.Include(incluse);
            return await query.Where(condation).ToListAsync();
        }
        public async Task<IList<T>> findByExAsync(Expression<Func<T, bool>> condation, Expression<Func<T, object>>[]? include=null, Expression<Func<T, object>>? OrderBy=null)
        {
            IQueryable<T> query = _db.Set<T>().AsNoTracking();
            if (include != null)
                foreach (var i in include)
                    query = query.Include(i);

            if (OrderBy != null)
                query=query.OrderBy(OrderBy);

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