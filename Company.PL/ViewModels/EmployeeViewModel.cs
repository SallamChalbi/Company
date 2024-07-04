using Company.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Company.PL.ViewModels
{
    //public enum Gender 
    //{
    //    [EnumMember(Value = "Male")]
    //    Male = 1,
    //    [EnumMember(Value = "Female")]
    //    Female  = 2 
    //}
    public enum EmpType
    {
        FullTime = 1,
        PartTime = 2
    }
    public class EmployeeViewModel : ModelBase
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Max Length of Name is 50 Chars")]
        [MinLength(3, ErrorMessage = "Min Length of Name is 5 Chars")]
        public string Name { get; set; }

        [Range(22, 50,ErrorMessage = "Age Must be In Range From 22 to 50")]
        public int Age { get; set; }

        [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",
            ErrorMessage = "Adress must be like 123-Street-City-Country")]
        public string Adress { get; set; }

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Display(Name = "Hiring Date")]
        public DateTime HiringDate { get; set; }

        //public Gender Gender { get; set; }
        public EmpType EmployeeType { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
