using Discoursify.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Discoursify.Controllers
{
    public class ProfileController : Controller
    {

        [HttpGet]
        public ActionResult ViewProfile(string type)
        {
            Profile profileData = new Profile();
            if (Session["USER_NAME"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            profileData = profileData.profileData(Session["USER_NAME"].ToString());

            if( type == null)
            {
                type = "post";
            }

            profileData.ProfileActions = profileData.getDataByAction(type, Session["USER_NAME"].ToString());
            return View("ViewProfile", profileData);
        }
        public ActionResult ViewOtherProfile(string profile)
        {
            Profile profileData = new Profile();

            if (Session["USER_NAME"] == null)
            {
                profileData = profileData.profileData(profile);

                profileData.ProfileActions = profileData.getDataByAction("post", profile);

                return View(profileData);
            }

            if (Session["USER_NAME"].ToString() == profile)
            {
               return ViewProfile("post");
            }
            
            profileData = profileData.profileData(profile);

            profileData.ProfileActions = profileData.getDataByAction("post", profile);

            return View(profileData);
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            {
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        public ActionResult AddTelegramId(string telegramId)
        {
            Profile profile = new Profile();
            string result = profile.updateTelegramId(Session["USER_NAME"].ToString(), telegramId);
            if(result != null)
            {
                return ViewProfile("post");
            }
            return ViewProfile("post");
        }

        public ActionResult EditPost(string postKey)
        {
            return View();
        }
        public ActionResult DeletePost(string postKey) 
        {  
            ProfileAction action = new ProfileAction();

            action.deletePost(postKey, Session["USER_NAME"].ToString());
            TempData["SuccessMessage"] = "You have deleted your post successfully.";

            return RedirectToLocal("~/Profile/ViewProfile");
        }

        public ActionResult DeleteComment(string commentKey) 
        {
            ProfileAction action = new ProfileAction();

            action.deleteComment(commentKey, Session["USER_NAME"].ToString());
            TempData["SuccessMessage"] = "You have deleted your comment successfully.";

            return RedirectToLocal("~/Profile/ViewProfile");
        }
        public ActionResult Unlike(string likeKey) 
        {
            ProfileAction action = new ProfileAction();

            action.unlike(likeKey, Session["USER_NAME"].ToString());
            TempData["SuccessMessage"] = "You have unliked the post successfully.";

            return RedirectToLocal("~/Profile/ViewProfile");
        }

    }
}