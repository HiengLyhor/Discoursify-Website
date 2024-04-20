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
            try
            {
                postData = postData.PostDetail(postKey);

                if (postData != null)
                {

                    return View(postData);

                }
                return View();
            }
            catch (Exception ex)
            {

                TempData["Message"] = "There is an error during processing your process.";
                Console.WriteLine(ex.Message);
                return View();

            }

        }
    }
}