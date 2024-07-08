using Company.BLL.Interfaces;
using Company.DAL.Data;
using Company.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase
    {
        private protected readonly ApplicationDbContext _dbContext;
        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(T entity)
            => _dbContext.Set<T>().Add(entity);

        public void Update(T entity)
            =>_dbContext.Set<T>().Update(entity);

        public void Delete(T entity)
            => _dbContext.Set<T>().Remove(entity);

        public async Task<T> GetAsync(int id)
            => await _dbContext.FindAsync<T>(id);
        //=> _dbContext.Set<T>().Find(id);

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Employee))
                return (IEnumerable<T>) await _dbContext.Employees.Include(e => e.Department).AsNoTracking().ToListAsync();

            return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
        }
    }
}
