using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook.Web;
using Facebook.Web.Mvc;
using Website.Models;

namespace Website.Controllers
{
  public class HomeController : Controller
  {
    private SocialContext db = new SocialContext();
    public ActionResult Index()
    {
      ViewBag.Message = "Modify this template to kick-start your ASP.NET MVC application.";
      return View();
    }

    //[Authorize]
    [FacebookAuthorize(LoginUrl = "/Signup")]
    public ActionResult ProfileInfo()
    {
      var client = new FacebookWebClient();
      dynamic me = client.Get("me");
      ViewBag.Name = me.name;
      ViewBag.Id = me.id;
      //ViewBag.Message = "Your quintessential app description page.";

      return View();
    }

    public ActionResult Signup()
    {
      (new FacebookWebContext()).DeleteAuthCookie();
      return View();
    }

    [HttpPost]
    public ActionResult Signup(Enrollment enrollment)
    {
      if (ModelState.IsValid)
      {
        db.Enrollments.Add(enrollment);
        db.SaveChanges();
        return RedirectToAction("Signup");
      }
      return View(enrollment);
    }

    protected override void Dispose(bool disposing)
    {
      db.Dispose();
      base.Dispose(disposing);
    }
  }
}
