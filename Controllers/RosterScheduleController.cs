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
                .Include(r => r.Employee)
                .Include(r => r.Shift)
                .FirstOrDefaultAsync(r => r.AI_ID == id);

            if (rosterSchedule == null)
            {
                return NotFound();
            }

            ViewBag.Employees = new SelectList(_context.Employees, "EmployeeID", "Name", rosterSchedule.EmployeeID);
            ViewBag.Shifts = new SelectList(_context.Shifts, "ShiftCode", "ShiftName", rosterSchedule.ShiftCode);

            return View(rosterSchedule);
        }

        // POST: /RosterSchedule/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, RosterSchedule rosterSchedule)
        {
            if (id != rosterSchedule.AI_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rosterSchedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RosterScheduleExists(rosterSchedule.AI_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(rosterSchedule);
        }


        // GET: RosterSchedule/Delete/5
        public async Task<IActionResult> Delete(decimal id)
        {
            var rosterSchedule = await _context.RosterSchedules
                .Include(r => r.Employee)
                .Include(r => r.Shift)
                .FirstOrDefaultAsync(r => r.AI_ID == id);

            if (rosterSchedule == null)
            {
                return NotFound();
            }

            return View(rosterSchedule);
        }


        // POST: RosterSchedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(decimal id)
        {
            var rosterSchedule = _context.RosterSchedules.Find(id);

            if (rosterSchedule != null)
            {
                _context.RosterSchedules.Remove(rosterSchedule);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool RosterScheduleExists(decimal id)
        {
            return _context.RosterSchedules.Any(e => e.AI_ID == id);
        }
    }
}
