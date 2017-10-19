using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimpleBlog.Controllers
{
    public class CategoryController : Controller
    {
        SimpleBlogDbContext db = new SimpleBlogDbContext();
        // GET: Category
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category cat)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(cat);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}