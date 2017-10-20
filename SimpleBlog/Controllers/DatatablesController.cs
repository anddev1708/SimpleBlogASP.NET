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
            var jsonData = new
            {
                data = from emp in db.Blogs.ToList() select emp
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }
    }
}