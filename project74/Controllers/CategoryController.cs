using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using project74.Models;
namespace project74.Controllers
{
    public class CategoryController : Controller
    {
        dbContext db = new dbContext();
        // GET: Category
        public ActionResult Index()
        {
            if (Session["userid"] != null)
                return View(db.categories.ToList());
            else
                return RedirectToAction("login", "user");
        }

        //// GET: Category/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: Category/Create
        public ActionResult Create(category c)
        {
            category ct = db.categories.Where(n => n.cat_name == c.cat_name).FirstOrDefault();
            ViewBag.ct = ct;
            if (Session["userid"] != null)
            {
                user u = db.users.Where(n => n.admin != null).FirstOrDefault();
                if (Session["userid"].ToString() == u.id.ToString())
                {
                    return View();
                }
                return RedirectToAction("login", "user");
            }

            else
                return RedirectToAction("login", "user");
        }

        // POST: Category/Create
        [HttpPost]
        public ActionResult Create(category c, HttpPostedFileBase image)
        {
            category ct = db.categories.Where(n => n.cat_name == c.cat_name).FirstOrDefault();
            ViewBag.ct = ct;
            if (ModelState.IsValid && ct == null)
            {
                image.SaveAs(Server.MapPath($"~/attach/{image.FileName}"));
                c.image = image.FileName;

                db.categories.Add(c);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Create", c);
            }

        }

        public JsonResult check(string cat_name)
        {

            string retval = "";
            if (cat_name == "")
            {
                retval = "null";
            }
            else if (db.categories.Any(r => r.cat_name == cat_name))
            {
                retval = "true";
            }
            else
            {
                retval = "false";
            }
            return Json(retval, JsonRequestBehavior.AllowGet);
        }
        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            if (Session["userid"] != null)
            {
                user u = db.users.Where(n => n.admin != null).FirstOrDefault();
                if (Session["userid"].ToString() == u.id.ToString())
                {
                    category c = db.categories.Where(n => n.cat_id == id).SingleOrDefault();
                    return View(c);
                }
                return RedirectToAction("login", "user");
            }

            else
                return RedirectToAction("login", "user");
        }

        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult Edit(category c, HttpPostedFileBase image)
        {
            int id = int.Parse(RouteData.Values["id"].ToString());
            category ca = db.categories.Where(n => n.cat_id == id).SingleOrDefault();
            if (image != null)
            {
                image.SaveAs(Server.MapPath($"~/attach/{image.FileName}"));
                ca.image = image.FileName;
            }
            ca.cat_name = c.cat_name;
            ca.description = c.description;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int id)
        {
            if (Session["userid"] != null)
            {
                user u = db.users.Where(n => n.admin != null).FirstOrDefault();
                if (Session["userid"].ToString() == u.id.ToString())
                {
                    category ca = db.categories.Where(n => n.cat_id == id).SingleOrDefault();
                    db.categories.Remove(ca);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("login", "user");
            }

            else
                return RedirectToAction("login", "user");
        }

        // POST: Category/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}