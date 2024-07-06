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
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext dbContext) : base(dbContext) 
        {
        }

        public override async Task<IEnumerable<Employee>> GetAllAsync()
            => await _dbContext.Employees.Include(e => e.Department).AsNoTracking().ToListAsync();
        public IQueryable<Employee> GetEmployeesByAdress(string adress)
            => _dbContext.Employees.Where(E => E.Adress.ToLower().Contains(adress.ToLower()));

        public IQueryable<Employee> SearchByName(string name)
            => _dbContext.Employees.Where(E => E.Name.Contains(name)).Include(E => E.Department).AsNoTracking();
    }
}
