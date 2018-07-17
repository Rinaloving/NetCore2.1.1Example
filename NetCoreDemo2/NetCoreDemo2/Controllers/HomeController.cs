using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreDemo2.Models;

namespace NetCoreDemo2.Controllers
{
    [Authorize]  // 需要用户登录后才能看见
    public class HomeController : Controller
    {
        private readonly HelloWorldDBContext _context;

        public HomeController(HelloWorldDBContext context)
        {
            _context = context;
        }
        //public HomeController()
        //{

        //}

        //public string Index()
        //{
        //    return "你好，世界！此消息来自 HomeController....";
        //}

        //修改HomeController.cs 方法

        //public ContentResult Index()
        //{
        //    // 返回一串字符串
        //    return Content("你好，世界！这条信息来自使用了 Action Result 的 Home 控制器");
        //}

        // 再次修改
        //public ObjectResult Index()
        //{
        //    var employee = new Employee { ID = 1, Name = "小白" };
        //    return new ObjectResult(employee);
        //}

        //返回视图
        [AllowAnonymous] // 允许匿名用户访问
        public ViewResult Index()
        {
            //var employee = new Employee { ID = 1, Name = "小白" };
            //return View(employee);
            var model = new HomePageViewModel();

            SQLEmployeeData sqlData = new SQLEmployeeData(_context);
            model.Employees = sqlData.GetAll();
         

            return View(model);
        }


        public ViewResult Detail(int id)
        {
            var model = new HomePageViewModel();
            SQLEmployeeData sqlData = new SQLEmployeeData(_context);

            Employee employee = sqlData.Get(id);

            return View(employee);
        }



        //编辑员工
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = new HomePageViewModel();
            SQLEmployeeData sqlData = new SQLEmployeeData(_context);

            Employee employee = sqlData.Get(id);

            if (employee == null)
            {
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        //新增员工
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Edit(int id, EmployeeEditViewModel input)
        {
            SQLEmployeeData sqlData = new SQLEmployeeData(_context);
            var employee = sqlData.Get(id);

            if (employee != null && ModelState.IsValid)
            {
                employee.Name = input.Name;
                _context.SaveChanges();
                return RedirectToAction("Detail", new { id = employee.ID });

            }
            return View(employee);
        }
        [HttpPost]
        public IActionResult Create(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee();
                employee.Name = model.Name;

                SQLEmployeeData sqlData = new SQLEmployeeData(_context);
                sqlData.Add(employee);
                return RedirectToAction("Detail", new { id = employee.ID });
            }

            return View();
        }



        public class SQLEmployeeData
        {
            private HelloWorldDBContext _context { get; set; }

            public SQLEmployeeData(HelloWorldDBContext context)
            {
                _context = context;
            }

            public void Add(Employee emp)
            {
                _context.Add(emp);
                _context.SaveChanges();
            }

            public Employee Get(int ID)
            {
                return _context.Employees.FirstOrDefault(e=>e.ID == ID);
            }


            public IEnumerable<Employee> GetAll()
            {
                return _context.Employees.ToList<Employee>();
            }
        }


      


        public class HomePageViewModel
        {
            public IEnumerable<Employee> Employees { get; set; }
        }

        public class EmployeeEditViewModel
        {
            [Required, MaxLength(80)]
            public string Name { get; set; }
        }

       

    }
}