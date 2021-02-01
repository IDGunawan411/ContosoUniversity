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
    public class EnrollmentController : Controller
    {
        private SchoolContext db = new SchoolContext();

        public ActionResult Index(string sortOrder, string currentFilter, string currentFilterSatu, 
        string currentFilterDua, string searchGrade, string searchTitle, string searchName, int? page)
        {

            var enrollments = from e in db.Enrollments select e;


            // Current Short
            ViewBag.CurrentSort = sortOrder;

            // Short Title
            ViewBag.Title_Asc  = String.IsNullOrEmpty(sortOrder) ? "title_asc"  : "";
            ViewBag.Title_Desc = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            
            // Short LastName
            ViewBag.LastName_Asc  = String.IsNullOrEmpty(sortOrder) ? "last_name_asc"  : "";
            ViewBag.LastName_Desc = String.IsNullOrEmpty(sortOrder) ? "last_name_desc" : "";

            // Short Grade
            ViewBag.Grade_Asc  = String.IsNullOrEmpty(sortOrder) ? "grade_asc"  : "";
            ViewBag.Grade_Desc = String.IsNullOrEmpty(sortOrder) ? "grade_desc" : "";

            if (searchGrade != null)
            {
                page = 1;
            }
            else
            {
                searchGrade = currentFilter;
            }

            ViewBag.CurrentFilter = searchGrade;

            if (searchTitle != null)
            {
                page = 1;
            }
            else
            {
                searchTitle = currentFilterSatu;
            }

            ViewBag.CurrentFilter = searchTitle;

            if (searchName != null)
            {
                page = 1;
            }
            else
            {
                searchName = currentFilterDua;
            }

            ViewBag.CurrentFilter = searchName;

            
            if (!String.IsNullOrEmpty(searchGrade))
            {
                enrollments = enrollments.Where(e => e.Grade.ToString().Contains(searchGrade));
            }
            if (!String.IsNullOrEmpty(searchTitle))
            {
                enrollments = enrollments.Include(e => e.Course).Where(e => e.Course.Title.Contains(searchTitle));
            }
            //if (!String.IsNullOrEmpty(searchName))
            //{
            //    enrollments = enrollments.Include(e => e.Course).Where(e => e.Student.LastName.Contains(searchName));
            //}


            switch (sortOrder)
            {   
                // Title
                case "title_desc":
                    enrollments = enrollments.OrderByDescending(e => e.Course.Title);
                    break; 
                case "title_asc":
                    enrollments = enrollments.OrderBy(e => e.Course.Title);
                    break;

                // LastName
                //case "last_name_asc":
                //    enrollments = enrollments.OrderBy(e => e.Student.LastName);
                //    break;
                //case "last_name_desc":
                //    enrollments = enrollments.OrderByDescending(e => e.Student.LastName);
                //    break;

                // Grade
                case "grade_asc":
                    enrollments = enrollments.OrderBy(e => e.Grade);
                    break;
                case "grade_desc":
                    enrollments = enrollments.OrderByDescending(e => e.Grade);
                    break;

                // Default
                default:
                    enrollments = enrollments.OrderBy(e => e.CourseID);
                    break;

            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(enrollments.ToPagedList(pageNumber, pageSize));
            
        }

        // GET: Enrollment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // GET: Enrollment/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title");
            ViewBag.StudentID = new SelectList(db.Students, "ID", "LastName");
            return View();
        }

        // POST: Enrollment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EnrollmentID,CourseID,StudentID,Grade")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Enrollments.Add(enrollment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Students, "ID", "LastName", enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Enrollment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Students, "ID", "LastName", enrollment.StudentID);
            return View(enrollment);
        }

        // POST: Enrollment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EnrollmentID,CourseID,StudentID,Grade")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enrollment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            } 
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Students, "ID", "LastName", enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Enrollment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // POST: Enrollment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Enrollment enrollment = db.Enrollments.Find(id);
            db.Enrollments.Remove(enrollment);
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
