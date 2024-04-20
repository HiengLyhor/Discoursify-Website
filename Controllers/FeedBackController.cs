using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Discoursify.Controllers
{
    public class FeedBackController : Controller
    {
        // GET: FeedBack
        public ActionResult Feedback()
        {
            return View();
        }
    }
}