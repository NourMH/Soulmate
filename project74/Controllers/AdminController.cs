using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using project74.Models;

namespace project74.Controllers
{
    public class AdminController : Controller
    {
        dbContext db = new dbContext();
        public ActionResult ViewUsers(string username)
        {
            if (username == "")
            {
                List<user> u = db.users.Where(n=>n.admin==null).ToList();
                return View(u);
            }
            else if (username != null)
            {
                List<user> u = db.users.Where(n => n.username == username&&n.admin == null).ToList();
                return View(u);

            }

            else
            {
                List<user> u = db.users.Where(n => n.admin == null).ToList();
                return View(u);
            }
        }
        public ActionResult ViewBlock(string username)
        {
            if (username == "")
            {
                List<block_user> b = db.block_user.ToList();
                return View(b);
            }
            else if (username != null)
            {
                List<block_user> b = db.block_user.Where(n => n.username == username).ToList();
                return View(b);

            }
            else
            {
                List<block_user> b = db.block_user.ToList();
                return View(b);
            }

        }
        public ActionResult Block(int u_id)
        {
            user u = db.users.Where(n => n.id == u_id).FirstOrDefault();
            block_user b = new block_user();
            b.username = u.username;
            b.image_id = u.image_id;
            db.block_user.Add(b);


            db.users.Remove(u);
            db.SaveChanges();
            return RedirectToAction("ViewBlock");
        }



        public ActionResult UnBlock(int u_id)
        {
            block_user b = db.block_user.Where(n => n.id == u_id).FirstOrDefault();
            db.block_user.Remove(b);
            db.SaveChanges();
            return RedirectToAction("ViewBlock");
        }

        public ActionResult viewProfile(int u_id)
        {
            user u = db.users.Where(n => n.id == u_id).FirstOrDefault();
            ViewBag.usercat = db.user_cat.Where(n => n.u_id == u_id).ToList();
            List<package> packages = new List<package> { };
            foreach (var item in ViewBag.usercat)
            {
                foreach (var package in db.packages)
                {
                    if (package.cat_id == item.cat_id&& package.u_id==u.id)
                    {

                        packages.Add(package);
                    }
                }
            }
            List<image> images = new List<image> { };
            foreach (var item in packages)
            {
                foreach (var image in db.images)
                {
                    if (image.p_id == item.p_id)
                    {

                        images.Add(image);
                    }
                }
            }

            ViewBag.pack = packages;
            ViewBag.img = db.images.ToList();
            return View(u);
        }


    }
}