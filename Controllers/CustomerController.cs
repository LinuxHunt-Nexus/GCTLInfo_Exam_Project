using GCTLInfo_Exam_Project.Data;
using GCTLInfo_Exam_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Immutable;

namespace GCTLInfo_Exam_Project.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CustomerController(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public IActionResult Index()
        {
            //try
            //{
            //    var customerList =  _context.Customers.ToImmutableArray();

            //    return View(customerList);

            //}
            //catch (Exception ex)
            //{

            //    throw ex;
            //}
            return View();
        }
        public async Task<IActionResult> GetAll()
        {
            var customerList = await _context.Customers.ToListAsync();
            return Json(new { data = customerList });
        }

        //  GET
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            // Save Customer Information first
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return RedirectToAction("CreateDeliveryInfo");
        }
        [HttpGet]
        public IActionResult CreateDeliveryInfo()
        {
            var customers = _context.Customers.ToList();
            var customersList = customers.Select(c => new { c.Id, c.CustomerName }).ToList();
            ViewBag.Customers = customersList;  // Directly passing the customers list to ViewBag

            // A default list with one empty DeliveryInfo for rendering the form
            var deliveryInfoList = new List<DeliveryInfo> { new DeliveryInfo() };

            return View(deliveryInfoList);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDeliveryInfo(List<DeliveryInfo> deliveryAddresses)
        {
            var customers = await _context.Customers.ToListAsync();
            if (customers != null && customers.Any())
            {
                ViewBag.Customers = new SelectList(customers, "Id", "CustomerName");
            }
            else
            {
                ViewBag.Customers = new SelectList(Enumerable.Empty<SelectListItem>());
            }

            // Check if model state is valid
            if (!ModelState.IsValid)
            {
                return View(deliveryAddresses);
            }

            // Save all DeliveryInfo records
            foreach (var deliveryInfo in deliveryAddresses)
            {
                _context.DeliveryAddresses.Add(deliveryInfo);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("Index"); // Redirect after saving
        }



        //  GET
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0) return NotFound();  // Ensure the id is valid
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();
            return View(customer);
        }


        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(customer);
                await _context.SaveChangesAsync();

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(customer);

        }


        public IActionResult DeleveryIndex()
        {
            return View();
        }
        public async Task<IActionResult> GetAllDelevery()
        {
            var customerList = await _context.DeliveryAddresses
                .Include(d => d.Customer)  // Customer সম্পর্কিত ডাটা ইনক্লুড করা
                .ToListAsync();

            var result = customerList.Select(d => new {
                d.DeliveryAddress,
                d.ContactPerson,
                d.Phone,
                CustomerName = d.Customer.CustomerName,  // Customer থেকে CustomerName বের করা
                d.DeliveryID
            }).ToList();

            return Json(new { data = result });
        }



        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var customerId = await _context.Customers.FindAsync(id);
            if (customerId == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customerId);
            await _context.SaveChangesAsync();

            return Ok();
        }
        [HttpDelete("DeleteDeliveryInfo/{deliveryID}")]
        public async Task<IActionResult> DeleteDeliveryInfo(int deliveryID)
        {
            var delId = await _context.DeliveryAddresses.FindAsync(deliveryID);
            if (delId == null)
            {
                return NotFound();
            }

            _context.DeliveryAddresses.Remove(delId);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
