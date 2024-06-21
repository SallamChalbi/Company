using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Company.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _repository;
        public DepartmentController(IDepartmentRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var department = _repository.GetAll();
            return View(department);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                var count = _repository.Add(department);
                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        public IActionResult Details(int? id)
        {
            if (id is null)
                return BadRequest();

            var department = _repository.Get(id.Value);
            if(department is null)
                return NotFound();

            return View(department);
        }
    }
}
