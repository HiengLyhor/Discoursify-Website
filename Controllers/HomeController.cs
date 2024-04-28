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
        public ActionResult Index(string filter)
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }

            var posts = new LazyLoadView().GetPostsFromDatabase(filter);

            // Pass the posts and other relevant data to the view
            var model = new LazyLoadView
            {
                Posts = posts
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