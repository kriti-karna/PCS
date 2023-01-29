using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PCS.Models;
using System.Diagnostics;


namespace PCS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PcsTestContext _context;

        public HomeController(ILogger<HomeController> logger, PcsTestContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var qualifications = _context.Qualifications.OrderBy(m => m.Id).ToList();
            ViewData["Qualifications"] = qualifications;
            var employees = _context.Employees.OrderBy(m => m.Id).ToList();
            ViewData["Employee"] = employees;
            return View();
        }

        public string Qualification(int id)
        {
            try
            {
                var qualification = _context.Qualifications.Where(m => m.Id == id).FirstOrDefault().Name;
                return qualification;
            }
            catch
            {
                return "";
            }
        }

        public string GetQualification(int id)
        {
            var qualification = _context.EmployeeQualifications.Where(m => m.EmpId == id).ToList();
            string result = "";
            foreach(var item in qualification)
            {
                var qualificationName = _context.Qualifications.Where(m => m.Id == item.QId).FirstOrDefault().Name;
                result += "<tr data-id=\"" + item.QId + "\"><td>" + item.QId + "</td>";
                result += "<td>" + qualificationName + "</td><td>" +
                    "<input type=\"number\" required class=\"form-control\" value=\"" + item.Marks + "\" name=\"Marks[]\" />" +
                    "<input hidden value=\"" + item.QId + "\" name=\"EQId[]\" />" +
                    "<input hidden value=\"" + item.EmpId + "\" name=\"EId\" />" +
                    "</td>";
            }
            
            return result;
        }

        public ActionResult EditQualification(int EID, int[] EQId, decimal[] Marks)
        {
            int count = EQId.Count();
            for(int i = 0; i <count; i++)
            {
                var checkExisting = _context.EmployeeQualifications.Where(m => m.EmpId == EID && m.QId == EQId[i]).FirstOrDefault();
                if(checkExisting != null) //Only mark edited
                {
                    checkExisting.Marks = Marks[i];
                    _context.Update(checkExisting);
                    _context.SaveChanges();
                }
                else //Included new qualification
                {
                    var newQualification = new EmployeeQualification
                    {
                        EmpId = EID,
                        QId = EQId[i],
                        Marks = Marks[i]
                    };
                    _context.Add(newQualification);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddEmployee(Employee emp, int[] EQId, decimal[] Marks, DateTime DobDate)
        {
            emp.Dob = new DateOnly(DobDate.Year, DobDate.Month, DobDate.Day);
            var today = DateTime.Now;
            emp.EntryDate = new DateOnly(today.Year, today.Month, today.Day);
            _context.Add(emp);
            _context.SaveChanges();
            int count = EQId.Count();
            for(int i = 0; i<count; i++)
            {
                EmployeeQualification eq = new EmployeeQualification();
                eq.EmpId = emp.Id;
                eq.QId = EQId[i];
                eq.Marks = Marks[i];
                _context.Add(eq);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult GetEmployee(int id)
        {
            var emp = _context.Employees.Where(m => m.Id == id).FirstOrDefault();
            string json = JsonConvert.SerializeObject(emp);
            return Json(json);
        }

        public ActionResult EditEmployee(Employee emp, DateTime DobDate)
        {
            emp.Dob = new DateOnly(DobDate.Year, DobDate.Month, DobDate.Day);
            var existing = _context.Employees.Where(m => m.Id == emp.Id).AsNoTracking().FirstOrDefault();
            emp.EntryDate = existing.EntryDate;
            _context.Update(emp);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public void DeleteEmp(int id)
        {
            string rawQuery = "DELETE FROM public.employee WHERE id = " + id;
            _context.Database.ExecuteSqlRaw(rawQuery);
            _context.SaveChanges();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}