using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.Models;

namespace Website.Controllers
{

  public class EnrollmentController : Controller
  {
    private SocialContext db = new SocialContext();



    // GET: /Enrollment/
    public ViewResult Index()
    {
      return View();//db.Enrollments.ToList());
    }

    public ViewResult List()
    {
      return View(db.Enrollments.ToList());
    }

    /*public JsonResult Json()
    {
      return Json(db.Enrollments.ToList(), JsonRequestBehavior.AllowGet);
    }*/

    public JsonResult Json(int id)
    {
      List<Enrollment> enrollments;
      if (id == 0)
        enrollments = db.Enrollments.ToList();
      else
        enrollments = (from e in db.Enrollments where e.EnrollmentID > id select e).ToList(); 
      return Json(enrollments.ToList(), JsonRequestBehavior.AllowGet);
    }

    public FileContentResult CSV()
    {
      var enrollments = db.Enrollments.ToList();
      var csv = "Voornaam,Tussennaam,Achternaam,E-mail,School,FacebookID\n";

      Func<string, string> safe = x => String.Format("'{0}'", (""+x).Replace("'", "\\'")); //.Replace("'", "\\'")

      foreach (var item in enrollments)
      {
        csv += String.Format(
          "{0},{1},{2},{3},{4},{5}\n",
          safe(item.FirstName),
          safe(item.MiddleName),
          safe(item.LastName),
          safe(item.Email), 
          safe(item.School),
          safe(item.FacebookID));
      }
      return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "Enrollments.csv");
    }

    // GET: /Enrollment/Details/5
    public ViewResult Details(int id)
    {
      Enrollment enrollment = db.Enrollments.Find(id);
      return View(enrollment);
    }

    // GET: /Enrollment/Create
    public ActionResult Create()
    {
      return View();
    }

    // POST: /Enrollment/Create
    [HttpPost]
    public ActionResult Create(Enrollment enrollment)
    {
      if (ModelState.IsValid)
      {
        db.Enrollments.Add(enrollment);
        db.SaveChanges();
        return RedirectToAction("Index");
      }

      return View(enrollment);
    }

    //
    // GET: /Enrollment/Edit/5

    public ActionResult Edit(int id)
    {
      Enrollment enrollment = db.Enrollments.Find(id);
      return View(enrollment);
    }

    //c
    // POST: /Enrollment/Edit/5

    [HttpPost]
    public ActionResult Edit(Enrollment enrollment)
    {
      if (ModelState.IsValid)
      {
        db.Entry(enrollment).State = EntityState.Modified;
        db.SaveChanges();
        return RedirectToAction("List");
      }
      return View(enrollment);
    }

    //
    // GET: /Enrollment/Delete/5

    public ActionResult Delete(int id)
    {
      Enrollment enrollment = db.Enrollments.Find(id);
      return View(enrollment);
    }

    //
    // POST: /Enrollment/Delete/5

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Enrollment enrollment = db.Enrollments.Find(id);
      db.Enrollments.Remove(enrollment);
      db.SaveChanges();
      return RedirectToAction("List");
    }

    protected override void Dispose(bool disposing)
    {
      db.Dispose();
      base.Dispose(disposing);
    }
  }
}