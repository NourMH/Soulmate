using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using project74.Models;
namespace project74.Controllers
{
    public class HomeController : Controller
    {
        dbContext db = new dbContext();

        public ActionResult Index()
        {
            ViewBag.size = db.categories.Count();

            if (Session["userid"] != null)
            {
                return View(db.categories.ToList());
            }
           else if (Request.Cookies["user"] != null)
            {
                
                Session["userid"] = Request.Cookies["user"].Values["id"];
                int id = int.Parse(Request.Cookies["user"].Values["id"]);
                user u2 = db.users.Where(n => n.id == id).FirstOrDefault();
                Session["usertype"] = u2.admin;
                return View(db.categories.ToList());
            }

            return View(db.categories.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult vendors(int? id)
        {

            if (id != null)
            {
                ViewBag.cat = db.categories.Where(n => n.cat_id == id).FirstOrDefault().cat_id;
            }
            else
            {
                ViewBag.cat = db.categories.FirstOrDefault().cat_id;
            }
            return View(db.categories.ToList());


        }
        public ActionResult catdet(int? id)
        {
            if (id != null)
            {
                ViewBag.usercat = db.user_cat.Where(n => n.cat_id == id).ToList();
                ViewBag.pack = db.packages.ToList();
                ViewBag.user = db.users.ToList();
                ViewBag.img = db.images.ToList();
                category c = db.categories.Where(n => n.cat_id == id).FirstOrDefault();
                return PartialView(c);
            }
            else
                return PartialView();
        }
        

        public ActionResult viewPackages(int id)
        {
            int cat_id = int.Parse(Request.QueryString["cat_id"]);
            ViewBag.img = db.images.ToList();

            return View(db.packages.Where(n => n.cat_id == cat_id && n.u_id == id).ToList());
        }

    }
}