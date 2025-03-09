using GCTLInfo_Exam_Project.Data;
using GCTLInfo_Exam_Project.Models;
using GCTLInfo_Exam_Project.viewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace GCTLInfo_Exam_Project.Controllers
{
    public class RosterScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RosterScheduleController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: RosterSchedule
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var rosterSchedules = await _context.RosterSchedules
                    .Include(r => r.Employee)
                    .Include(r => r.Shift)
                    .Select(r => new
                    {
                        r.AI_ID,
                        r.RosterScheduleCode,
                        r.EmployeeID,
                        EmployeeName = r.Employee != null ? r.Employee.Name : "N/A",
                        Designation = r.Employee != null && r.Employee.Designation != null ? r.Employee.Designation.DesignationName : "N/A",
                        Date = r.Date.ToString("yyyy-MM-dd"),
                        DayOfWeek = r.Date.ToString("dddd"),
                        ShiftName = r.Shift != null ? r.Shift.ShiftName : "N/A",
                        ShiftStartTime = r.Shift != null ? r.Shift.ShiftStartTime.ToString("hh:mm tt") : "N/A",
                        ShiftEndTime = r.Shift != null ? r.Shift.ShiftEndTime.ToString("hh:mm tt") : "N/A",
                        r.Remarks
                    })
                    .ToListAsync();

                return Json(new { data = rosterSchedules });
            }
            catch (Exception ex)
            {
                // Log the error or display a message
                Console.WriteLine(ex.Message);
                return Json(new { data = new List<object>() }); // return empty list in case of error
            }
        }


        // GET: RosterSchedule/Create
        public async Task<IActionResult> Create()
        {
            var model = new RosterScheduleEntryViewModel
            {
                Shifts = await _context.Shifts.ToListAsync(),
                Employees = await (from emp in _context.Employees
                                   join des in _context.Designations
                                   on emp.DesignationCode equals des.DesignationCode
                                   select new EmployeeViewModel
                                   {
                                       EmployeeID = emp.EmployeeID,
                                       Name = emp.Name,
                                       DesignationName = des.DesignationName
                                   }).ToListAsync()
            };

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RosterScheduleEntryViewModel model)
        {
            if (model.DateFrom > model.DateTo)
            {
                ModelState.AddModelError("", "The 'Date From' must be earlier than 'Date To'.");
                return View(model);
            }

            if (model.SelectedEmployees == null || model.SelectedEmployees.Count == 0)
            {
                ModelState.AddModelError("", "Please select at least one employee.");
                return View(model);
            }
            
            foreach (var empID in model.SelectedEmployees)
            {
                var roster = new RosterSchedule
                {
                    EmployeeID = empID,
                    ShiftCode = model.ShiftCode,
                    Date = model.DateFrom,
                    Remarks = "Auto-generated",
                    EntryDate = DateTime.Now,
                    RosterScheduleCode = $"RS-{DateTime.Now:yyyyMMddHHmmss}-{empID}"
                };
                _context.RosterSchedules.Add(roster);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(decimal id)
        {
            var rosterSchedule = await _context.RosterSchedules
                //.Include(r => r.Employee)
                .Include(r => r.Shift)
                .FirstOrDefaultAsync(r => r.AI_ID == id);

            if (rosterSchedule == null)
            {
                return NotFound();
            }
            var model = new RosterScheduleEntryViewModel
            {
                AI_ID = rosterSchedule.AI_ID,
                EmployeeID = rosterSchedule.EmployeeID,
                ShiftCode = rosterSchedule.ShiftCode,
                DateFrom = rosterSchedule.Date,
                Remarks = rosterSchedule.Remarks,
                Shifts = await _context.Shifts.ToListAsync(),
                Employees = await (from emp in _context.Employees
                                   join des in _context.Designations
                                   on emp.DesignationCode equals des.DesignationCode
                                   where emp.EmployeeID == rosterSchedule.EmployeeID
                                   select new EmployeeViewModel
                                   {
                                       EmployeeID = emp.EmployeeID,
                                       Name = emp.Name,
                                       DesignationName = des.DesignationName
                                   }).ToListAsync() 
            };

            return View(model);
        }


        // POST: /RosterSchedule/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RosterScheduleEntryViewModel model)
        {

            if (!ModelState.IsValid)
            {
                var rosterSchedule = await _context.RosterSchedules
                    .FirstOrDefaultAsync(r => r.AI_ID == model.AI_ID);

                if (rosterSchedule == null)
                {
                    return NotFound();
                }

                rosterSchedule.EmployeeID = model.EmployeeID;
                rosterSchedule.ShiftCode = model.ShiftCode;
                rosterSchedule.Date = model.DateFrom;
                rosterSchedule.Remarks = model.Remarks;
                rosterSchedule.ModifyDate = DateTime.Now;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If we reach here, something went wrong
            model.Shifts = await _context.Shifts.ToListAsync();
            model.Employees = await (from emp in _context.Employees
                                     join des in _context.Designations
                                     on emp.DesignationCode equals des.DesignationCode
                                     select new EmployeeViewModel
                                     {
                                         EmployeeID = emp.EmployeeID,
                                         Name = emp.Name,
                                         DesignationName = des.DesignationName
                                     }).ToListAsync();

            return View(model);
        }

        // GET: RosterSchedule/Delete/5
        [HttpDelete]
        public async Task<IActionResult> Delete(decimal id)
        {
            var rosterSchedule = await _context.RosterSchedules.FindAsync(id);
            if (rosterSchedule == null)
            {
                return NotFound();
            }

            _context.RosterSchedules.Remove(rosterSchedule);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool RosterScheduleExists(decimal id)
        {
            return _context.RosterSchedules.Any(e => e.AI_ID == id);
        }
    }
}
