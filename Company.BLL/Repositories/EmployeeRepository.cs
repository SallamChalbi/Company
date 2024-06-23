﻿using Company.BLL.Interfaces;
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

        public IQueryable<Employee> GetEmployeesByAdress(string adress)
        {
            return _dbContext.Employees.Where(E => E.Adress.ToLower() == adress.ToLower());
        }
    }
}