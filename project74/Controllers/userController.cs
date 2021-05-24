using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using project74.Models;
namespace project74.Controllers
{
    public class userController : Controller
    {
        dbContext db = new dbContext();
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult check(string UserName)
        {

            string retval = "";
            if (db.users.Any(r => r.username == UserName))
            {
                retval = "true";
            }
            else
            {
                if (db.block_user.Any(r => r.username == UserName))
                {
                    retval = "true";
                }
                else
                {
                    retval = "false";
                }
            }
            return Json(retval, JsonRequestBehavior.AllowGet);
        }
        public ActionResult register()
        {
            Console.WriteLine("rawan");

            ViewBag.cat = db.categories.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult register(user u, HttpPostedFileBase image_id)
        {
            if (ModelState.IsValid)
            {
                if (image_id != null)
                {
                    image_id.SaveAs(Server.MapPath($"~/attach/{image_id.FileName}"));
                    u.image_id = image_id.FileName;
                }
                db.users.Add(u);
                db.SaveChanges();
                return RedirectToAction("login");
            }
            return View();
        }
        public ActionResult login()
        {
            if (Request.Cookies["user"] != null)
            {
                Session["userid"] = Request.Cookies["user"].Values["id"];
                int id = int.Parse(Request.Cookies["user"].Values["id"]);
                user u2 = db.users.Where(n => n.id ==id).FirstOrDefault();
                Session["usertype"] = u2.admin;
                return RedirectToAction("index", "Home");
            }
            return View();
        }
        [HttpPost]
        public ActionResult login(user u,string rememberme)
        {
            user u2 = db.users.Where(n => n.username == u.username &&n.password==u.password).FirstOrDefault();
            if (u2 != null)
            {
                Session.Add("userid", u2.id);
                Session.Add("usertype", u2.admin);
                if (rememberme == "true")
                {
                    HttpCookie cook = new HttpCookie("user");
                    cook.Values.Add("id", u2.id.ToString());
                    cook.Values.Add("admin", u2.admin.ToString());
                    cook.Expires = DateTime.Now.AddDays(15);
                    Response.Cookies.Add(cook);
                }
                return RedirectToAction("index","Home");

            }
            else
            {
                ViewBag.status = "invalid username or password";
                return View();
            }
        }
        public ActionResult addcat()
        {
            int id = int.Parse(Session["userid"].ToString());
            ViewBag.cat = db.categories.ToList();
            ViewBag.usercat = db.user_cat.Where(n=>n.u_id==id).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult addcat(user_cat u)
        {
            u.u_id= int.Parse(Session["userid"].ToString());

            db.user_cat.Add(u);
            db.SaveChanges();
            return RedirectToAction("addpackages",u);
        }
        public ActionResult addpackages(package p)
        {
            package pk = db.packages.Where(n => n.p_name == p.p_name).FirstOrDefault();
            ViewBag.pk = pk;

            int id=int.Parse(Session["userid"].ToString());
            ViewBag.users= db.users.Where(n=>n.id==id).ToList();
            ViewBag.usercat = db.user_cat.ToList();
            ViewBag.cat = db.categories.ToList();
            List<category> cate = new List<category> { };
            foreach (project74.Models.user user in ViewBag.users)
            {
                foreach (project74.Models.user_cat usercat in ViewBag.usercat)
                {
                    foreach (project74.Models.category cat in ViewBag.cat)
                    {
                        if (usercat.u_id == user.id && usercat.cat_id == cat.cat_id)
                        {
                            cate.Add(cat);
                        }
                    }
                }
            }
            SelectList st = new SelectList(cate, "cat_id", "cat_name");
            ViewBag.c = st;
            return View();
        }
        [HttpPost]
        public ActionResult addpackages(package p, HttpPostedFileBase[] fileobj, image img)
        {
            package pk = db.packages.Where(n => n.p_name == p.p_name).FirstOrDefault();
            ViewBag.pk = pk;


            p.u_id = int.Parse(Session["userid"].ToString());
            db.packages.Add(p);
            db.SaveChanges();
            foreach (HttpPostedFileBase i in fileobj)
            {
                string imgext = Path.GetExtension(i.FileName);
                if (imgext == ".jpg" || imgext == ".png")
                {
                    var infile = Path.GetFileName(i.FileName);
                    var savepath = Path.Combine(Server.MapPath("~/attach/") + infile);
                    i.SaveAs(savepath);
                    img.img_url = i.FileName;
                    img.p_id = p.p_id;
                    db.images.Add(img);
                    db.SaveChanges();
                    ViewData["Message"] = "your selected " + fileobj.Length + " photos is uploaded to database";
                }
                else
                {
                    ViewData["Message"] = "please upload .jpg or .png images only";
                }
            }
     
            return RedirectToAction("profile");
        }


      

        public ActionResult logout()
        {


            Session["userid"] = null;
            Session["usertype"] = null;

            HttpCookie c = new HttpCookie("user");
            c.Expires = DateTime.Now.AddDays(-30);
            Response.Cookies.Add(c);

            return RedirectToAction("login");
        }
        public ActionResult profile()
        {
            
            int id = int.Parse(Session["userid"].ToString());
            user u = db.users.Where(n => n.id == id).FirstOrDefault();
            ViewBag.users = db.users.Where(n => n.id == id).ToList();
            ViewBag.usercat = db.user_cat.ToList();
            ViewBag.cat = db.categories.ToList();
            List<category> cate = new List<category> { };
            foreach (project74.Models.user user in ViewBag.users)
            {
                foreach (project74.Models.user_cat usercat in ViewBag.usercat)
                {
                    foreach (project74.Models.category cat in ViewBag.cat)
                    {
                        if (usercat.u_id == user.id && usercat.cat_id == cat.cat_id)
                        {
                            cate.Add(cat);
                        }
                    }
                }
            }
            ViewBag.c = cate;
            return View(u);
        }
        public ActionResult changepassword(int id)
        {
            user s = db.users.Where(n => n.id == id).FirstOrDefault();
            return View(s);
        }
        [HttpPost]
        public ActionResult changepassword(user u, string oldpass)
        {
            user us = db.users.Where(n => n.password == oldpass && n.id == u.id).SingleOrDefault();
            if (us != null)
            {
                if (u.confrim_pass == u.password)
                {
                    us.password = u.password;
                    us.confrim_pass = u.confrim_pass;
                    db.SaveChanges();
                    return RedirectToAction("profile");
                }
            }

            return View();
        }
        public ActionResult edit(int id)
        {
            user u = db.users.Where(n => n.id == id).FirstOrDefault();
            return View(u);
        }
        [HttpPost]
        public ActionResult edit(user u, HttpPostedFileBase image_id)
        {
            int id2 = int.Parse(Session["userid"].ToString());
            user us = db.users.Where(n => n.id == id2).SingleOrDefault();
            u.confrim_pass = u.password;
            if (ModelState.IsValid)
            {
                if (image_id != null)
                {
                    image_id.SaveAs(Server.MapPath($"~/attach/{image_id.FileName}"));
                    us.image_id = image_id.FileName;
                }
                us.loc = u.loc;
                us.fname = u.fname;
                us.lname = u.lname;
                us.mobile = u.mobile;
                us.Adress = u.Adress;
                us.confrim_pass = us.password;
                db.SaveChanges();
                return RedirectToAction("profile");
            }
            return View(us);
        }
        public ActionResult packages(int cat_id)
        {
            ViewBag.pack = db.packages.ToList();

            int id2 = int.Parse(Session["userid"].ToString());
            user_cat uc = db.user_cat.Where(n => n.cat_id == cat_id && n.u_id == id2).FirstOrDefault();
            ViewBag.allpack = db.packages.ToList();
            List<package> p = new List<package> { };
            List<image> im = new List<image> { };
            foreach (package item in ViewBag.allpack)
            {
                if (item.cat_id == uc.cat_id && item.u_id == uc.u_id)
                {
                    p.Add(item);
                }
            }
            ViewBag.packs = p;
            ViewBag.img = db.images.ToList();
            ViewBag.usercat = db.user_cat.Where(n => n.cat_id == cat_id).ToList();
            ViewBag.user = db.users.ToList();
            return View();
        }
        public ActionResult deletecat(int cat_id)
        {
            int id = int.Parse(Session["userid"].ToString());
            user_cat ca = db.user_cat.Where(n => n.cat_id == cat_id&&n.u_id==id).SingleOrDefault();
            db.user_cat.Remove(ca);
            db.SaveChanges();

            return RedirectToAction("profile");  
        }
        public ActionResult editpack(int p_id)
        {
            package p = db.packages.Where(n => n.p_id == p_id).FirstOrDefault();
            return View(p);
        }
        [HttpPost]
        public ActionResult editpack(package p)
        {
            package p2 = db.packages.Where(n => n.p_id == p.p_id).FirstOrDefault();
            p2.p_name = p.p_name;
            p2.p_desc = p.p_desc;
            db.SaveChanges();
            return RedirectToAction("packages", new { cat_id = p2.cat_id });
        }
        public ActionResult deletepack(int p_id)
        {
            package p = db.packages.Where(n => n.p_id == p_id).FirstOrDefault();
            int? cat_id1 = p.cat_id;
            db.packages.Remove(p);
            db.SaveChanges();
            return RedirectToAction("packages", new { cat_id = cat_id1 });
        }
    }
}