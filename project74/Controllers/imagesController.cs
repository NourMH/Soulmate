using project74.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace project74.Controllers
{

    public class imagesController : Controller
    {
        dbContext db = new dbContext();

        public ActionResult Imgindex()
        {
            return View(db.images.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.package = new SelectList(db.packages, "p_id", "p_name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(HttpPostedFileBase[] fileobj, image img)
        {
            if (fileobj == null || fileobj.Length == 0)
            {
                ViewData["message"] = "please select at least one image";
            }

            foreach (HttpPostedFileBase i in fileobj)
            {
                string imgext = Path.GetExtension(i.FileName);
                if (imgext == ".jpg" || imgext == ".png")
                {
                    var infile = Path.GetFileName(i.FileName);
                    var savepath = Path.Combine(Server.MapPath("~/attach/") + infile);
                    i.SaveAs(savepath);
                    img.img_url = i.FileName;

                    db.images.Add(img);
                    db.SaveChanges();
                    ViewData["Message"] = "your selected " + fileobj.Length + " photos is uploaded to database";
                }
                else
                {
                    ViewData["Message"] = "please upload .jpg or .png images only";
                }
            }
            ViewBag.package = new SelectList(db.packages, "p_id", "p_name", img.package);
            return View("create");
        }

        public ActionResult Imgdetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            image image = db.images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        public ActionResult Editimg(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            image img = db.images.Where(n => n.img_id == id).SingleOrDefault();
            if (img == null)
            {
                return HttpNotFound();
            }
            ViewBag.package = new SelectList(db.packages.ToList(), "p_id", "p_name", img.package);

            return View(img);
        }

        [HttpPost]
        public ActionResult Editimg(image img, HttpPostedFileBase file )
        {
            image im = db.images.Where(n => n.img_id == img.img_id).SingleOrDefault();
            if (file != null)
            {
                im.img_url = file.FileName;

            }
            im.p_id = img.p_id;
            db.SaveChanges();
            return RedirectToAction("Imgindex");
        }
    public ActionResult Deleteimg(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            image image = db.images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        [HttpPost, ActionName("Deleteimg")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            image image = db.images.Find(id);
            db.images.Remove(image);
            db.SaveChanges();
            return RedirectToAction("Imgindex");
        }

    }

}