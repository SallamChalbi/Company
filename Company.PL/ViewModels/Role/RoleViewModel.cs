using System;

namespace Company.PL.ViewModels.Role
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public RoleViewModel()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
