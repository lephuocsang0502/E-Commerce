using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.Dao;
using OnlineShop.Common;
using PagedList;

namespace OnlineShop.Areas.Admin.Controllers
{
   
    public class UserController : BaseController
    {
        private OnlineDbContext db = new OnlineDbContext();
        [HasCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(string searchString,int page = 1, int pageSize = 1)
        {
            var dao = new UserDao();
            var model = dao.ListAllPaging(searchString,page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }
        [HttpGet]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create(string groupId =null)
        {
            var group = from l in db.UserGroups  select l;
            ViewBag.groupId = new SelectList(group, "ID", "Name");
            return View();
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create(User user, string groupId = null)
        {
            Random r = new Random();
            int[] b;
            string s = "QT"; // chuỗi để in ra
            b = new int[5]; // n là số phần tử
            for (int i = 0; i < 5; ++i)
            {
                b[i] = r.Next(0, 9);// xuat hien cac so tu 0 den 9
                s += b[i].ToString();
            }
            user.ID = s;
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var encryptedMd5Pas = Encryptor.MD5Hash(user.Password);
                user.Password = encryptedMd5Pas;

                string id = dao.Insert(user);
                if (id !=null)
                {
                  
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm user không thành công");
                }
                var group = from l in db.UserGroups select l;
                ViewBag.groupId = new SelectList(group, "ID", "Name");
            }
            return View("Index");
        }
        [HttpPost]
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(User user, string groupId = null)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (!string.IsNullOrEmpty(user.Password))
                {
                    var encryptedMd5Pas = Encryptor.MD5Hash(user.Password);
                    user.Password = encryptedMd5Pas;
                }

                var group = from l in db.UserGroups select l;
                ViewBag.groupId = new SelectList(group, "ID", "Name");
                var result = dao.Update(user);
                if (result)
                {
                 
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật user không thành công");
                }
                
            }
                return View("Index");
        }
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(string id, string groupId = null)
        {
            var group = from l in db.UserGroups select l;
            ViewBag.groupId = new SelectList(group, "ID", "Name");
            var user = new UserDao().ViewDetail(id);
            return View(user);
        }
        [HttpDelete]
        [HasCredential(RoleID = "DELETE_USER")]
        public ActionResult Delete(string id)
        {
            new UserDao().Delete(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult ChangeStatus(string id)
        {
            var result = new UserDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}