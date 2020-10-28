using OnlineShop.Areas.Admin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using OnlineShop.Common;
using Facebook;
using System.Configuration;
using Model.EF;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
       
        // GET: Admin/Login
        
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.Password),true);
                if (result==1)
                {
                    var user = dao.GetById(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;
                    userSession.GroupID = user.GroupID;
                    var listCredentials = dao.GetListCredential(model.UserName);

                    Session.Add(CommonConstants.SESSION_CREDENTIALS, listCredentials);
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return RedirectToAction("Index", "Home");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tai khoang ko ton tai");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tai khoang dang bi khoa");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Sai mat khau");
                }
                else if (result == -3)
                {
                    ModelState.AddModelError("", "Tài khoản của bạn không có quyền đăng nhập.");
                }
                else
                {
                    ModelState.AddModelError("", "Login fail");
                }
            }
            return View("Index");


        }

        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });

            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });
            Random r = new Random();
            int[] b;
            string s = "KH"; // chuỗi để in ra
            b = new int[5]; // n là số phần tử
            for (int i = 0; i < 5; ++i)
            {
                b[i] = r.Next(0, 9);// xuat hien cac so tu 0 den 9
                s += b[i].ToString();
            }

            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                // Get the user's information, like email, first name, middle name etc
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email,picture");
                string email = me.email;
                string userName = me.first_name + " " + me.middle_name + " " + me.last_name;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;
                string img = "https://graph.facebook.com/me/picture?type=large&access_token=" + accessToken;

                var user = new User();
                user.ID = s;
                user.Email = email;
                
                user.Image = img;
                user.UserName = firstname + " " + middlename + " " + lastname;
                user.Status = true;
                user.Name = firstname + " " + middlename + " " + lastname;
                user.CreatedDate = DateTime.Now;
                var resultInsert = new UserDao().InsertForFacebook(user);
                if (resultInsert != null)
                {
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;
                    userSession.GroupID = user.GroupID;
                    userSession.Name = user.Name;
                    userSession.Image = user.Image;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                }
            }
            return RedirectToAction("Index","Home");
        }
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            return Redirect("/Admin/Login/Index");
        }

    }
}