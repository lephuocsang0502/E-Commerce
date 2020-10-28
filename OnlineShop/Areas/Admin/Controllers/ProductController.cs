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
    public class ProductController : BaseController
    {
        private OnlineDbContext db = new OnlineDbContext();
        // GET: Admin/Product
        public ActionResult Index()
        {          

            return View(db.Products.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }
        [HttpPost]
       
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductDao();

                product.CreatedDate = DateTime.Now;
                long id = dao.Insert(product);
                ViewBag.msg = "Product Saved";
                if (id > 0)
                {

                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm product không thành công");
                }

            }
            SetViewBag();
            return View(product);
        }
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductDao();
                var result = dao.Update(product);
                if (result)
                {

                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật product không thành công");
                }
            }
            SetViewBag();
            return View("Index");
        }
        public ActionResult Edit(int id)
        {
            var product = new ProductDao().ViewDetail(id);
            SetViewBag();
            return View(product);
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new ProductDao().Delete(id);

            return RedirectToAction("Index");
        }

        public void SetViewBag(long? selectedId = null)
        {
            var dao = new ProductCategoryDao();
            ViewBag.CategoryID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }

        /*  public ActionResult Dashboard(Product product)
          {
              var dao = new ProductDao();
              var list = dao.GetAll();
              List<int> repar = new List<int>();
              var age = list.Select(x => x.Price).Distinct();

              foreach(var item in age)
              {
                  repar.Add(list.Count(x=>x.Price==item));
              }
              var rep = repar;
              ViewBag.AGE = age;
              ViewBag.REP = repar.ToList();
              return View();
          }*/
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