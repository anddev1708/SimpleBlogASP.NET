using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimpleBlog.Controllers
{
    public class SlickGridController : Controller
    {
        SimpleBlogDbContext db = new SimpleBlogDbContext();
        // GET: SlickGrid
        public ActionResult Index()
        {
            return View(db.Blogs.ToList());
        }

        [HttpGet]
        public ActionResult getListData()
        {
            List<Blog> data = db.Blogs.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}