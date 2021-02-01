using System;
using PagedList;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;

namespace ContosoUniversity.Controllers
{
    public class CourseController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Course
        public ViewResult Index(string sortOrder, string S_Title, string C_Title, string S_Credit_From, string C_Credit_From,
            string S_Credit_To, string C_Credit_To, int? page)
        {
            // Curent filter
            ViewBag.CurrentSort = sortOrder;
                
            // Filter Title
            ViewBag.Title_Asc   = String.IsNullOrEmpty(sortOrder) ? "title_asc"    : "";
            ViewBag.Title_Desc  = String.IsNullOrEmpty(sortOrder) ? "title_desc"   : "";

            // Filter Credit
            ViewBag.Credit_Asc  = String.IsNullOrEmpty(sortOrder) ? "credits_asc"  : "";
            ViewBag.Credit_Desc = String.IsNullOrEmpty(sortOrder) ? "credits_desc" : "";

            // Search
            var courses = from c in db.Courses
                          select c;

            if (S_Title != null)
            {
                page = 1;
            }
            else
            {
                S_Title = C_Title;
            }
            ViewBag.CurrentFilter = C_Title;

            
            if (S_Credit_To != null)
            {
                page = 1;
            }
            else
            {
                S_Credit_To = C_Credit_To;
            }

            ViewBag.CurrentFilter = S_Credit_To;
            
            if (S_Credit_To != null)
            {
                page = 1;
            }
            else
            {
                S_Credit_From = C_Credit_From;
            }

            ViewBag.CurrentFilter = S_Credit_From;


            if (!String.IsNullOrEmpty(S_Title))
            {
                courses = courses.Where(c => c.Title.Contains(S_Title));
            }
            if (!String.IsNullOrEmpty(S_Credit_To))
            {
                int Credit_From = int.Parse(S_Credit_From);
                int Credit_To = int.Parse(S_Credit_To);
                courses = courses.Where(c => c.Credits >= Credit_From && c.Credits <= Credit_To);
            }

            // Filter
            switch (sortOrder)
            {
                // Title
                case "title_desc":
                    courses = courses.OrderByDescending(c => c.Title);
                    break;
                case "title_asc":
                    courses = courses.OrderBy(c => c.Title);
                    break;

                // Credits
                case "credits_desc":
                    courses = courses.OrderByDescending(c => c.Credits);
                    break;
                case "credits_asc":
                    courses = courses.OrderBy(c => c.Credits);
                    break;

                // Default
                default:   
                    courses = courses.OrderBy(c => c.Title);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(courses.ToPagedList(pageNumber, pageSize));
        }


        // GET: Course/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Course/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,Title,Credits")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        // GET: Course/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseID,Title,Credits")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: Course/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
