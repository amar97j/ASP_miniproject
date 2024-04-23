using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class BankController : Controller
    {

        private readonly BankContext _context;

        public BankController(BankContext context) {
            _context = context;
        }

        private static List<BankBranch> bankBranches = new List<BankBranch>()
        {
            new BankBranch{Id = 1, LocationName = "Qadsiys", LocationURL = "https://www.google.com/maps/dir/29.3334544,48.0673006/%D8%A8%D9%8A%D8%AA+%D8%A7%D9%84%D8%AA%D9%85%D9%88%D9%8A%D9%84+%D8%A7%D9%84%D9%83%D9%88%D9%8A%D8%AA%D9%8A,+Hamad+Alkhaled+St,+kuwait%E2%80%AD%E2%80%AD/@29.3300826,47.9542369,12z/data=!3m1!4b1!4m9!4m8!1m1!4e1!1m5!1m1!1s0x3fcf9ca79161387b:0x30210b1335d485e4!2m2!1d48.0016982!2d29.3507565?entry=ttu" ,BranchManager
            = "Ali", EmployeeCount = 20 },

            new BankBranch{Id = 10, LocationName = "Adan", LocationURL = "https://www.google.com/maps/dir/29.3334544,48.0673006/%D8%A8%D9%8A%D8%AA+%D8%A7%D9%84%D8%AA%D9%85%D9%88%D9%8A%D9%84+%D8%A7%D9%84%D9%83%D9%88%D9%8A%D8%AA%D9%8A,+Hamad+Alkhaled+St,+kuwait%E2%80%AD%E2%80%AD/@29.3300826,47.9542369,12z/data=!3m1!4b1!4m9!4m8!1m1!4e1!1m5!1m1!1s0x3fcf9ca79161387b:0x30210b1335d485e4!2m2!1d48.0016982!2d29.3507565?entry=ttu" ,BranchManager
            = "Abdllah", EmployeeCount = 200 }
        };



        public IActionResult Index()
        {
            var context = _context;
            return View(context.BankBranches.ToList());
        }


        public IActionResult Details(int id)
        {
            using (var context = _context)
            {
                var branch = context.BankBranches.Include(r=>r.Employees).SingleOrDefault(r=>r.Id == id);
                if (branch == null)
                {
                    return RedirectToAction("Index");
                }

                return View(branch);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            var form = new NewBranchForm();

            return View(form);
        }

        [HttpPost]
        public IActionResult Create(NewBranchForm branchForm)
        {

            if (ModelState.IsValid)
            {
                using (var context = _context)
                {
                    var newBranch = new BankBranch
                    {
                        LocationName = branchForm.LocationName,
                        LocationURL = branchForm.LocationURL,
                        BranchManager = branchForm.BranchManager,
                        EmployeeCount = branchForm.EmployeeCount

                    };

                    context.BankBranches.Add(newBranch);
                    context.SaveChanges();

                }



                return RedirectToAction("Index");
            }

            return View(branchForm);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            using (var context = _context)
            {
                var branch = context.BankBranches.Find(id);
                if (branch == null)
                {
                    return RedirectToAction("Index");
                }
                var form = new NewBranchForm();
                form.LocationURL = branch.LocationURL;
                form.BranchManager = branch.BranchManager;
                form.LocationName = branch.LocationName;
                form.EmployeeCount = branch.EmployeeCount;

                return View(form);
            }
        }

        [HttpPost]
        public IActionResult Edit(int id, NewBranchForm newBranchForm)
        {

            using (var context = _context)
            {
                var branch = context.BankBranches.Find(id);
                if (branch != null)
                {
                    branch.LocationName = newBranchForm.LocationName;
                    branch.LocationURL = newBranchForm.LocationURL;
                    branch.BranchManager = newBranchForm.BranchManager;
                    branch.EmployeeCount = newBranchForm.EmployeeCount;
                    context.SaveChanges();
                    return RedirectToAction("Index");

                }

                return View();
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            using (var context = _context)
            {
                var branch = context.BankBranches.Find(id);
                if (branch == null)
                {
                    return RedirectToAction("Index");
                }

                return View(branch);
            }


        }

        [HttpPost]
        public IActionResult Delete(int id, BankBranch b)
        {
            using (var context = _context)
            {
                var branch = context.BankBranches.Find(id);
                if (branch == null)
                {
                    return RedirectToAction("Index");
                }

                context.BankBranches.Remove(branch);
                context.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult AddEmployee(int Id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddEmployee(int id, AddEmployeeForm form)
        {
            if (ModelState.IsValid)
            {
                var database = _context;
                var branch = database.BankBranches.Find(id);
                var newEmployee = new Employee();

                newEmployee.Name = form.Name;
                newEmployee.CivilId = form.CivilId;
                newEmployee.Position = form.Position;
                branch.Employees.Add(newEmployee);

                database.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(form);



        }
    }
}