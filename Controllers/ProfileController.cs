using Discoursify.Models;
using System;
using System.Collections.Generic;
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
            return View(profileData);
        }
        public ActionResult ViewOtherProfile(string profile)
        {
            Profile profileData = new Profile();
            profileData = profileData.profileData(profile);

            profileData.ProfileActions = profileData.getDataByAction("post", profile);

            return View(profileData);
        }

    }
}