using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using System.Net;
using PagedList;

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

        public ActionResult Index(int? page)
        {
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(db.Blogs.OrderByDescending(x => x.ID).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            // Get category
            List<Category> cate = db.Categories.ToList();
            // Tạo SelectList
            SelectList cateList = new SelectList(cate, "ID", "CategoryName");
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

        [HttpPost]
        public ActionResult Index(Blog emp)
        {
            List<Blog> blogs;
            if (emp.Subject != null)
            {
                blogs = (from Emp in db.Blogs
                             where Emp.Subject.StartsWith(emp.Subject)
                             select Emp).ToList();
            }
            else
            {
                blogs = db.Blogs.ToList();
            }
            return PartialView("BlogInfo", blogs);
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
    }
}