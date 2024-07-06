﻿using Company.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Data.Configurations
{
    internal class EmployeeConfigurations : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(E => E.Name).HasColumnType("varchar")
                .HasMaxLength(50).IsRequired();
            builder.Property(E => E.Adress).IsRequired();
            builder.Property(E => E.Salary).HasColumnType("decimal(12, 2)");
            builder.Property(E => E.Gender)
                .HasConversion(
                    (Gender) => Gender.ToString(),
                    (genderAsString) => (Gender)Enum.Parse(typeof(Gender), genderAsString, true)
                );
        }
    }
}
