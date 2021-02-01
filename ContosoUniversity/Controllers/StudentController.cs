using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Web.WebPages;
using Microsoft.Ajax.Utilities;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Windows.Documents;

namespace ContosoUniversity.Controllers
{
    
    public class StudentController : Controller
    {
        //private SchoolContext db = new SchoolContext();
        Uri baseAddress = new Uri("http://172.17.32.1/");
        HttpClient client;
        public StudentController() 
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public Enrollment Enrollments { get; private set; }

        // GET: Student
        public ViewResult Index(string sortOrder, string url_src, string S_LastName, string S_Namefirst, 
        string S_Enrollment, int? page)
        {
            List<Student> modelList = new List<Student>();
            // Curent Short
            ViewBag.CurrentSort = sortOrder;
            //Filter Lastname
            ViewBag.First_Name_Desc = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.First_Name_Asc  = String.IsNullOrEmpty(sortOrder) ? "name_asc"  : "";
            //Filter FirstName
            ViewBag.Last_Name_Desc  = String.IsNullOrEmpty(sortOrder) ? "name_last_desc" : "";
            ViewBag.Last_Name_Asc   = String.IsNullOrEmpty(sortOrder) ? "name_last_asc"  : "";
            //Filter Enroll_Date
            ViewBag.Date_Desc       = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.Date_Asc        = String.IsNullOrEmpty(sortOrder) ? "date_asc"  : "";

            if (!string.IsNullOrEmpty(S_Namefirst) || !string.IsNullOrEmpty(S_LastName) || !string.IsNullOrEmpty(S_Enrollment))
            {
                if (!string.IsNullOrEmpty(S_Enrollment))
                {
                    var SP_Enrollment = S_Enrollment.Split('-');
                    string Enrollment = SP_Enrollment[1] + "/" + SP_Enrollment[2] + "/" + SP_Enrollment[0];
                    url_src = "student_src?First_Name=" + S_Namefirst + "&Last_Name=" + S_LastName + "&Enrollment=" + Enrollment + "";
                }
                else
                {
                    url_src = "student_src?First_Name=" + S_Namefirst + "&Last_Name=" + S_LastName + "";
                }
                HttpResponseMessage response_src = client.GetAsync(url_src).Result;
                if (response_src.IsSuccessStatusCode)
                {
                    string data_src = response_src.Content.ReadAsStringAsync().Result;
                    if (data_src == "null")
                    {
                        return View(modelList.ToPagedList(1, 1));
                    }
                    else
                    {
                        modelList = JsonConvert.DeserializeObject<List<Student>>(data_src).ToList();
                        ViewBag.CurrentNF = S_Namefirst;
                        ViewBag.CurrentLN = S_LastName;
                        ViewBag.CurrentEN = S_Enrollment;
                    }
                }
            }
            else
            {
                var url = "student_get";
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    modelList = JsonConvert.DeserializeObject<List<Student>>(data).ToList();
                }
            }
            switch (sortOrder)
            {   
                // Last Name
                case "name_desc":
                    var url_name_desc = "student_short?First_Name=" + S_Namefirst + "&Last_Name="+ S_LastName +"&ODB=ORDER%20BY&Data=First_Name&Short=DESC";
                    HttpResponseMessage response_name_desc = client.GetAsync(url_name_desc).Result;
                    if (response_name_desc.IsSuccessStatusCode)
                    {
                        string data_name_desc = response_name_desc.Content.ReadAsStringAsync().Result;
                        modelList = JsonConvert.DeserializeObject<List<Student>>(data_name_desc).ToList();
                    }
                break;
                case "name_asc":
                    var url_name_asc = "student_short?First_Name=" + S_Namefirst + "&Last_Name=" + S_LastName + "&ODB=ORDER%20BY&Data=First_Name&Short=ASC";
                    HttpResponseMessage response_name_asc = client.GetAsync(url_name_asc).Result;
                    if (response_name_asc.IsSuccessStatusCode)
                    {
                        string data_name_asc = response_name_asc.Content.ReadAsStringAsync().Result;
                        modelList = JsonConvert.DeserializeObject<List<Student>>(data_name_asc).ToList();
                    }
                break;
                // Name first
                case "name_last_desc":
                    var url_fn_desc = "student_short?First_Name=" + S_Namefirst + "&Last_Name=" + S_LastName + "&ODB=ORDER%20BY&Data=Last_Name&Short=DESC";
                    HttpResponseMessage response_fn_desc = client.GetAsync(url_fn_desc).Result;
                    if (response_fn_desc.IsSuccessStatusCode)
                    {
                        string data_fn_desc = response_fn_desc.Content.ReadAsStringAsync().Result;
                        modelList = JsonConvert.DeserializeObject<List<Student>>(data_fn_desc).ToList();
                    }
                break;
                case "name_last_asc":
                    var url_fn_asc = "student_short?First_Name=" + S_Namefirst + "&Last_Name=" + S_LastName + "&ODB=ORDER%20BY&Data=Last_Name&Desc=DESC";
                    HttpResponseMessage response_fn_asc = client.GetAsync(url_fn_asc).Result;
                    if (response_fn_asc.IsSuccessStatusCode)
                    {
                        string data_fn_asc = response_fn_asc.Content.ReadAsStringAsync().Result;
                        modelList = JsonConvert.DeserializeObject<List<Student>>(data_fn_asc).ToList();
                    }
                break;
                // Enrollment
                case "date_desc":
                    var url_date_desc = "student_short?First_Name=" + S_Namefirst + "&Last_Name=" + S_LastName + "&ODB=ORDER%20BY&Data=EnrollmentDate&Short=DESC";
                    HttpResponseMessage response_date_desc = client.GetAsync(url_date_desc).Result;
                    if (response_date_desc.IsSuccessStatusCode)
                    {
                        string data_date_desc = response_date_desc.Content.ReadAsStringAsync().Result;
                        modelList = JsonConvert.DeserializeObject<List<Student>>(data_date_desc).ToList();
                    }
                break;
                case "date_asc":
                    var url_date_asc = "student_short?First_Name=" + S_Namefirst + "&Last_Name=" + S_LastName + "&ODB=ORDER%20BY&Data=EnrollmentDate&Desc=DESC";
                    HttpResponseMessage response_date_asc = client.GetAsync(url_date_asc).Result;
                    if (response_date_asc.IsSuccessStatusCode)
                    {
                        string data_date_asc = response_date_asc.Content.ReadAsStringAsync().Result;
                        modelList = JsonConvert.DeserializeObject<List<Student>>(data_date_asc).ToList();
                    }
                break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(modelList.ToPagedList(pageNumber, pageSize));

        }

        //private void DisplayFormat(object p)
        //{
        //    throw new NotImplementedException();
        //}

        //// GET: Student/Details/5
        public ActionResult Details(string id)
        {
            Student model = new Student();
            var url = "student_detail?id=" + id + "";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                string remove1 = data.Replace("[", "");
                string remove2 = remove1.Replace("]", "");
                model = new JavaScriptSerializer().Deserialize<Student>(remove2);
            }
            return View(model);
            
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        public ActionResult Create(Student model)
        {
            string url = "student_post";
            string jsonString = new JavaScriptSerializer().Serialize(model);
            HttpContent post_std = new StringContent(jsonString);

            post_std.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = client.PostAsync(url, post_std).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        //// GET: Student/Edit/5
        public ActionResult Edit(string id)
        {
            Student model = new Student();
            var url = "student_detail?id=" + id + "";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                string remove1 = data.Replace("[", "");
                string remove2 = remove1.Replace("]", "");
                model = new JavaScriptSerializer().Deserialize<Student>(remove2);
            }

            return View(model);
        }

        // POST: Student/Edit/5
        [HttpPost]
        public ActionResult Edit(Student model)
        {
            string url = "student_put";
            string jsonString = new JavaScriptSerializer().Serialize(model);
            HttpContent put_std = new StringContent(jsonString);
            put_std.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = client.PutAsync(url, put_std).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Student/Delete/5
        public ActionResult Delete(string id)
        {
            Student model = new Student();
            var url = "student_detail?id=" + id + "";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                string remove1 = data.Replace("[", "");
                string remove2 = remove1.Replace("]", "");
                model = new JavaScriptSerializer().Deserialize<Student>(remove2);
            }
            return View(model);
        }

        // POST: Student/Delete/5
        [HttpPost]
        public ActionResult Delete(Student model)
        {
            string url = "student_delete?id=" + model.ID_Student + "";

            var response = client.DeleteAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
