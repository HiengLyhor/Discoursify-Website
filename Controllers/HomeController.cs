using Discoursify.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Discoursify.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }

            int postsToSkip = (page - 1) * pageSize;

            // Retrieve the posts from the database based on the calculated skip and take
            var posts = new LazyLoadView().GetPostsFromDatabase(postsToSkip, pageSize);

            // Pass the posts and other relevant data to the view
            var model = new LazyLoadView
            {
                Posts = posts,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPostsCount = 10
            };

            return View(model);
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