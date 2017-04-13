using System;
using System.Linq;
using System.Web.Mvc;
using StudentsDB.Models;

namespace StudentsDB.Controllers
{
    public class HomeController : Controller
    {

        public StudentDBEntities db = new StudentDBEntities();
   
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult List(String Lecturer)
        {
            ViewBag.Message = "Your Current Students";

            // setup or access session variable
            if (Lecturer != null) System.Web.HttpContext.Current.Session["Lecturer"] = Lecturer;
            if (System.Web.HttpContext.Current.Session["Lecturer"] != null) Lecturer = System.Web.HttpContext.Current.Session["Lecturer"].ToString();
            
            // no lecturer ???
            if (String.IsNullOrEmpty(Lecturer))
            {
                ViewBag.Lecturer = null;
                return View();
            }

            // set up the Lecturer in the view
            ViewBag.Lecturer = Lecturer;

            // check if admin
            if (Lecturer.Equals("admin"))
            {
                // get all the students
               var query = from c in db.rhawthorne_StudentDB
                            orderby c.id
                            select c;
                // pass the students to the view
                return View(query.ToList());
            }
            else
            {
                // get the students from the database for the lecturer
                var query = from c in db.rhawthorne_StudentDB
                            where c.Lecturer == Lecturer
                            orderby c.id
                            select c;
                // pass the students to the view
                return View("List", query.ToList());
            }

        }

        public ActionResult Details(int id)
        {
            ViewBag.Message = "Edit a student's details";
            var query = from c in db.rhawthorne_StudentDB
                        where c.id == id
                        select c;
            // pass the students to the view
            return View(query.First());
        }

        public ActionResult Help()
        {
            ViewBag.Message = "Simple Help";

            return View();
        }

        public ActionResult New()
        {
            ViewBag.Message = "Add a new student";
            return View("Details", null);
        }

        public ActionResult Add(int id, String FirstName, String LastName, String StudentNumber)
        {
            String Lecturer = System.Web.HttpContext.Current.Session["Lecturer"].ToString();
            rhawthorne_StudentDB student = new rhawthorne_StudentDB()
            {
                FirstName = FirstName,
                LastName = LastName,
                StudentNumber = StudentNumber,
                Lecturer = Lecturer
            };
            db.rhawthorne_StudentDB.Add(student);
            db.SaveChanges();
            return this.List(null);
        }

        public ActionResult Save(int id, String FirstName, String LastName, String StudentNumber)
        {
            String Lecturer = System.Web.HttpContext.Current.Session["Lecturer"].ToString();
            rhawthorne_StudentDB student = db.rhawthorne_StudentDB.Find(id);
            student.FirstName = FirstName;
            student.LastName = LastName;
            student.StudentNumber = StudentNumber;
            student.Lecturer = Lecturer;
            db.SaveChanges();
            return this.List(null);
        }

        public ActionResult Delete(rhawthorne_StudentDB student)
        {
            rhawthorne_StudentDB remove = db.rhawthorne_StudentDB.Find(student.id);
            db.rhawthorne_StudentDB.Remove(remove);
            db.SaveChanges();
            return this.List(null);
        }
    }
}