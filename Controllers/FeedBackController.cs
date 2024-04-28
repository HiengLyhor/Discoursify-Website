using Discoursify.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            FeedBack feedback = new FeedBack();
            feedback.feedBacks = feedback.GetFeedback();

            return View(feedback);
        }

        public JsonResult Create(FeedBack data)
        {

            try
            {

                if (Session["USER_NAME"] == null)
                {
                    var invalidOwner = new { code = 400, message = "Please kindly login or sign up first to give feedback." };
                    return Json(invalidOwner);
                }

                // Perform add comment to db
                bool create = new FeedBack().CreateFeedBack(data.Title, data.Content, Session["USER_NAME"].ToString(), data.Rate);

                if (create)
                {
                    var dataSuccess = new { code = 200, message = "Success" };
                    return Json(dataSuccess);
                }

                var dataFalse = new { code = 400, message = "There is an error during processing your process." };
                return Json(dataFalse);

            }
            catch (Exception ex)
            {
                var errorData = new { code = 400, message = "An unexpected error during creating the feedback.", ErrorMessage = ex.Message };
                return Json(errorData);
            }
        }
    }
}