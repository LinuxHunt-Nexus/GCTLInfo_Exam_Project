using GCTLInfo_Exam_Project.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GCTLInfo_Exam_Project.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IActionResult Index()
        {
            //var empList = _context.Employees.ToList();
            //return View(empList);
            return View();
        }
        public async Task<IActionResult> GetAll()
        {
            var empList = await _context.Employees.ToListAsync();
            return Json(new { data = empList });
        }
    }
}
