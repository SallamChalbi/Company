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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public EmployeeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Add(Employee employee)
        {
            _dbContext.Employees.Add(employee);
            return _dbContext.SaveChanges();
        }

        public int Update(Employee employee)
        {
            _dbContext.Employees.Update(employee);
            return _dbContext.SaveChanges();
        }

        public int Delete(Employee employee)
        {
            _dbContext.Employees.Remove(employee);
            return _dbContext.SaveChanges();
        }

        public Employee Get(int id)
            => _dbContext.Employees.Find(id);

        public IEnumerable<Employee> GetAll()
            => _dbContext.Employees.AsNoTracking().ToList();
    }
}
