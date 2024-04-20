using Discoursify.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security.AntiXss;
using System.Web.Security;

namespace Discoursify.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult SignUp(string username)
        {
            if(username == null)
            {
                return View();
            }
            Login login = new Login();
            string checkUsername = login.CheckUsername(username);
            if (checkUsername == "Existed")
            {
                ModelState.AddModelError("", "This username is existed, please use other username.");
            }
            else
            {
                ModelState.AddModelError("Success", "This username is available to use.");
            }
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(Login signUp) 
        {
            Login model = new Login();
            string checkUsername = model.CheckUsername(signUp.Username);

            if(checkUsername != "Existed")
            {
                string status = model.Register(signUp);
                if (status == "Success")
                {
                    TempData["SuccessMessage"] = "Your account has been created successfully. \nPlease login with your username and password.";
                    return RedirectToLocal("");
                }
            }
            else
            {
                ModelState.AddModelError("", "This username is existed, please use other username.");
            }
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult PageError()
        {
            return View();
        }

        public ActionResult BadRequest()
        {
            return View();
        }

        public ActionResult CheckUser(string username)
        {
            Login login = new Login();
            string checkUsername = login.CheckUsername(username);
            if(checkUsername == "Existed")
            {
                ModelState.AddModelError("", "This username is existed, please use other username.");
            }
            else
            {
                ModelState.AddModelError("Success", "This username is available to use.");
            }
            return RedirectToLocal("~/Login/SignUp");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToLocal("~/");
        }

        [HttpPost]
        public ActionResult Login(Login login, string returnUrl)
        {
            string status = login.loginAction(login.Email, login.Password);

            if(status == "Success")
            {
                try
                {

                    login = login.getUserLogin(login.Email);

                    StaticContent.init();
                    CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                    serializeModel.Username = login.Username;
                    serializeModel.UserRole = login.UserRole;

                    Session["USER_ROLE"] = login.UserRole;
                    Session["USER_NAME"] = login.Username;
                    Session["USER_EMAIL"] = login.Email;

                    string userData = JsonConvert.SerializeObject(serializeModel);
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                    1,
                    login.Username,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(15),
                    false, //pass here true, if you want to implement remember me functionality
                    userData);

                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, HttpUtility.HtmlDecode(AntiXssEncoder.HtmlEncode(encTicket, false)));

                }
                catch
                {
                    ModelState.AddModelError("", "Something went wrong during the process.");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View();
            }
            return RedirectToLocal(returnUrl);
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

    }
}