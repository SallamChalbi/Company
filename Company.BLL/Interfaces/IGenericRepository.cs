using Company.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Interfaces
{
    public interface IGenericRepository<T> where T : ModelBase
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        void Add(T employee);
        void Update(T employee);
        void Delete(T employee);
    }
}
