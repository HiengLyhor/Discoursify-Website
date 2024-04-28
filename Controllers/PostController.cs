using Discoursify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Discoursify.Controllers
{
    public class PostController : Controller
    {

        [HttpGet]
        public ActionResult Create()
        {
            if (Session["USER_NAME"] ==  null)
            {
                TempData["Message"] = "Please login or sign up to write a post.";
                return View();
            }
            return View();
        }
        [HttpPost]
        public ActionResult Create(Post post) 
        {
        
            Post action = new Post();
            post.Owner = Session["USER_NAME"].ToString();

            string createPost = action.CreatePost(post);
            if(createPost == "success")
            {
                TempData["SuccessMessage"] = "Your post created successfully!";
                return RedirectToAction("Index", "Home");
            }

            TempData["ErrorPost"] = "Fail while trying to create a post.";
            return View(post);
        
        }

        [HttpGet]
        public ActionResult PostDetail(string postKey)
        {
            Post postData = new Post();
            List<PostComment> comments = new List<PostComment>();

            try
            {
                postData = postData.PostDetail(postKey);
                comments = new PostComment().GetPostComments(postKey);

                if(comments != null)
                {
                    postData.PostComments = comments;
                }
                
                if (postData != null)
                {

                    return View(postData);

                }
                return View("~/Home/Index");
            }
            catch (Exception ex)
            {

                TempData["Message"] = "There is an error during processing your process.";
                Console.WriteLine(ex.Message);
                postData = null;
                return View("~/Home/Index");

            }

        }

        public JsonResult CreateComment(string comment, string postKey)
        {
            try
            {

                if (Session["USER_NAME"] == null)
                {
                    var invalidOwner = new { code = 400, message = "Please kindly login or sign up first to comment." };
                    return Json(invalidOwner);
                }

                // Perform add comment to db
                bool create = new PostComment().CreateComment(postKey, Session["USER_NAME"].ToString(), comment);

                if (create)
                {
                    var data = new { code = 200, message = "Success" };
                    return Json(data);
                }

                var dataFalse = new { code = 400, message = "There is an error during processing your process." };
                return Json(dataFalse);

            }
            catch (Exception ex)
            {
                var errorData = new { code = 400, message = "An unexpected error during creating the comment.", ErrorMessage = ex.Message };
                return Json(errorData);
            }
        }

        public JsonResult Vote(string method, string postKey)
        {

            try
            {
                if (Session["USER_NAME"] == null)
                {
                    var invalidOwner = new { code = 400, message = "Please kindly login or sign up first to up vote/down vote." };
                    return Json(invalidOwner);
                }
                bool vote = new Post().Vote(method, Session["USER_NAME"].ToString(), postKey);

                if (vote)
                {
                    var data = new { code = 200, message = "Success" };
                    return Json(data);
                }

                var dataFalse = new { code = 400, message = "There is an error during processing your process." };
                return Json(dataFalse);
            }
            catch (Exception ex)
            {
                var errorData = new { code = 400, message = "An unexpected error during processing your process.", ErrorMessage = ex.Message };
                return Json(errorData);
            }

        }
    }
}