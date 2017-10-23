using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;

namespace SimpleBlog.Controllers
{
    public class DatatablesController : Controller
    {

        SimpleBlogDbContext db = new SimpleBlogDbContext();
        // GET: Datatables
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult getBlogs()
        {
            IQueryable<Blog> query = db.Blogs;

            var data = query.Select(p => new
            {
                ID = p.ID,
                CategoryId = p.CategoryId,
                Subject = p.Subject,
                Body = p.Body,
                DatePosted = p.DatePosted,
                Type = p.Type,
                OtherType = p.OtherType,
                CategoryName = p.Category.CategoryName
            }).ToList();

            var dataJson = new { data };
            return Json(dataJson, JsonRequestBehavior.AllowGet);

        }

        // Delete multi record
        [HttpPost]
        public ActionResult DeleteRecords(IEnumerable<int> itemIds)
        {

            if (itemIds != null & itemIds.Count() > 0)
            {
                List<Blog> selectedList = db.Blogs.Where(a => itemIds.Contains((int)a.ID)).ToList();
                foreach (var i in selectedList)
                {
                    db.Blogs.Remove(i);
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}