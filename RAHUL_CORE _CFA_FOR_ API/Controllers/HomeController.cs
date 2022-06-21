using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RAHUL_CORE__CFA_FOR__API.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RAHUL_CORE__CFA_FOR__API.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        Uri url = new Uri("http://localhost:8272/api/RKS/");

        HttpClient Client;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            Client = new HttpClient();

            Client.BaseAddress = url;
        }

        
      
        [HttpGet]

        public IActionResult Index()
        {

            List<Student> emplist = new List<Student>();
            HttpResponseMessage listemp = Client.GetAsync(Client.BaseAddress + "get/allDetail").Result;


            if (listemp.IsSuccessStatusCode)
            {
                string data = listemp.Content.ReadAsStringAsync().Result;


                var emp = JsonConvert.DeserializeObject<List<Student>>(data);

                //foreach (var item in emp)
                //{

                //    emplist.Add(new Student
                //    {
                //        RollNo = item.RollNo,
                //        Address = item.Address,
                //        Class = item.Class,
                //        Marks = item.Marks,
                //        MobileNo = item.MobileNo,
                //        Name = item.Name,


                //    });
                //}
                emplist = emp;
            }
            return View(emplist);
        }
        [HttpGet]
        public ActionResult AddStu()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddStu(Student obj)
        {
            string data = JsonConvert.SerializeObject(obj);

            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage res = Client.PostAsync(Client.BaseAddress + "Add/Student", content).Result;

            if (res.IsSuccessStatusCode)
            {

                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");
        }




        public ActionResult update(int Id)
        {
            var res = Client.GetAsync(Client.BaseAddress + "Edit/Detail" + '?' + "Id" + "=" + Id.ToString()).Result;
            string data = res.Content.ReadAsStringAsync().Result;
            var emp = JsonConvert.DeserializeObject<Student>(data);
            return View("AddStu", emp);

        }

        public ActionResult Delete(int Id)
        {
            var res = Client.DeleteAsync(Client.BaseAddress + "del/student" + '?' + "Id" + "=" + Id.ToString()).Result;
            return RedirectToAction("Index");

        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
