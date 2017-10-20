using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using System.Net;
using PagedList;
using System.Data.Entity;
using System.IO;
using System.Text;
using System.Configuration;

namespace SimpleBlog.Controllers
{
    public class HomeController : Controller
    {
        SimpleBlogDbContext db = new SimpleBlogDbContext();
        //public ActionResult Index()
        //{
        //    //Comment comment = new Comment();
        //    //comment.Name = "Quyet";
        //    //comment.Content = "Noi dung comment dau tien";
        //    ////comment.DatePosted = new DateTime();

        //    //Blog blog = new Blog();
        //    //blog.Subject = "Bai viet dau tien";
        //    ////blog.DatePosted = new DateTime();
        //    //blog.Body = "Noi dung cua bai viet dau tien";
        //    //blog.Comments.Add(comment);

        //    //Category cat = new Category();
        //    //cat.CategoryName = "Android";
        //    //cat.Blogs.Add(blog);

        //    //db.Categories.Add(cat);
        //    //db.SaveChanges();



        //    return View(db.Blogs.ToList());
        //}

        [HttpGet]
        public ActionResult Index(int? page)
        {
            int pageSize = 2;
            int pageNumber = (page ?? 1);
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;

            return View(db.Blogs.OrderByDescending(x => x.ID).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            // Get category
            List<Category> cate = db.Categories.ToList();
            // Tạo SelectList
            SelectList cateList = new SelectList(cate, "ID", "CategoryName", 1);
            // Set vào ViewBag
            ViewBag.CategoryList = cateList;

            return View();
        }

        public ActionResult Create(Blog blog)
        {
            if (ModelState.IsValid)
            {
                db.Blogs.Add(blog);
                db.SaveChanges();

            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }

            List<Category> cate = db.Categories.ToList();
            // Tạo SelectList
            SelectList cateList = new SelectList(cate, "ID", "CategoryName");
            // Set vào ViewBag
            ViewBag.CategoryList = cateList;

            return View(blog);
        }

        [HttpPost]
        public ActionResult Edit(Blog blog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blog).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }

            return View(blog);
        }

        [HttpGet, ActionName("TimKiem")]
        public ActionResult TimKiem(int? page, int? pageSize, string type, int otherType)
        {
            int pageNumber = (page ?? 1);
            int pageSizeX = (pageSize ?? 2);
            List<Blog> blogs;
            int[] types;
            if (type != null && type != "")
            {
                types = type.Split(',').Select(n => int.Parse(n)).ToArray();

                if(types != null && type.Length > 0 && otherType == -1)
                {
                    blogs = (from Emp in db.Blogs where types.Contains((int)(Emp.Type ?? 1)) select Emp).ToList();
                } else if(types != null && type.Length > 0 && otherType != -1)
                {
                    blogs = (from Emp in db.Blogs where types.Contains((int)(Emp.Type ?? 1)) && (Emp.OtherType ?? 1) == otherType select Emp).ToList();
                } else if(types == null && otherType != -1)
                {
                    blogs = (from Emp in db.Blogs where (Emp.OtherType ?? 1) == otherType select Emp).ToList();
                } else
                {
                    blogs = db.Blogs.ToList();
                }
            } else
            {
                if(otherType != -1)
                {
                    blogs = (from Emp in db.Blogs where (Emp.OtherType ?? 1) == otherType select Emp).ToList();
                } else
                {
                    blogs = db.Blogs.ToList();
                }
            }

            return PartialView("BlogInfo", blogs.OrderByDescending(x => x.ID).ToPagedList(pageNumber, pageSizeX));
        }


        [HttpGet, ActionName("SearchIndex")]
        public ActionResult SearchIndex(int? page, int? pageSize, string searchText)
        {
            // Check dieu kien dau vao
            int pageNumber = (page ?? 1);
            int pageSizeX = (pageSize ?? 2);
            if (string.IsNullOrEmpty(searchText))
            {
                searchText = "undefined";
            }


            List<Blog> blogs;
            if (searchText != "undefined")
            {
                blogs = (from Emp in db.Blogs
                             where Emp.Subject.Contains(searchText)
                             select Emp).ToList();
            }
            else
            {
                blogs = db.Blogs.ToList();
            }

            return PartialView("BlogInfo", blogs.OrderByDescending(x => x.ID).ToPagedList(pageNumber, pageSizeX));
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

        [HttpPost]
        public ActionResult Export()
        {
            ExportToExcel();
            return new ExcelResult();
        }

        class ExcelResult : ActionResult
        {
            public override void ExecuteResult(ControllerContext context)
            {
                
            }
        }
        // Generate HTML to excel
        public void ExportToExcel()
        {
            string Filename = "ExcelFrom" + DateTime.Now.ToString("mm_dd_yyy_hh_ss_tt") + ".xls";
            string FolderPath = HttpContext.Server.MapPath("/App_Data/ExcelFiles/");
            string FilePath = Path.Combine(FolderPath, Filename);

            //Step-1: Checking: the file name exist in server, if it is found then remove from server.------------------
            if (System.IO.File.Exists(FilePath))
            {
                System.IO.File.Delete(FilePath);
            }

            //Step-2: Get Html Data & Converted to String----------------------------------------------------------------
            string HtmlResult = RenderRazorViewToString("~/Views/Home/BlogInfo.cshtml", db.Blogs.ToList());

            //Step-4: Html Result store in Byte[] array------------------------------------------------------------------
            byte[] ExcelBytes = Encoding.ASCII.GetBytes(HtmlResult);

            //Step-5: byte[] array converted to file Stream and save in Server------------------------------------------- 
            using (Stream file = System.IO.File.OpenWrite(FilePath))
            {
                file.Write(ExcelBytes, 0, ExcelBytes.Length);
            }

            //Step-6: Download Excel file 
            Response.Charset = "UTF-8";
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(Filename));
            Response.WriteFile(FilePath);
            Response.End();
            Response.Flush();
        }

        protected string RenderRazorViewToString(string viewName, object model)
        {
            if (model != null)
            {
                ViewData.Model = model;
            }
            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        // Giải phóng Database
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}