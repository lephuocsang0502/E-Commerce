using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.ViewModel;

namespace OnlineShop.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index(int page=1,int pageSize=5)
        {
            var dao = new ProductDao();
            var model = dao.ListAllPaging(page, pageSize);
            return View(model);
        }
        [ChildActionOnly]
        public PartialViewResult ProductCategory()
        {
            var model = new ProductCategoryDao().ListAll();
            return PartialView(model);
        }
        [OutputCache(CacheProfile = "Cache1DayForProduct")]
        public ActionResult Category(long id,int page=1,int pageSize=1)
        {
            var category = new ProductCategoryDao().ViewDetail(id);
            ViewBag.Category = category;
           
            var model = new ProductDao().ListByCategoryId(id, page, pageSize);

            
            return View(model);
        }
        [OutputCache(CacheProfile = "Cache1DayForProduct")]
        public ActionResult Detail(long id)
        {
            var product = new ProductDao().ViewDetail(id);
            ViewBag.Category = new ProductCategoryDao().ViewDetail(product.CategoryID);         
            ViewBag.RelatedProducts = new ProductDao().ListRelatedProducts(id,3);
            return View(product);
        }
        public JsonResult ListName(string q)
        {
            var data = new ProductDao().ListName(q);
            return Json(new
            {
                data = data,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        [OutputCache(CacheProfile = "Cache1DayForProduct")]
        public ActionResult Search(string search)
        {
            int totalRecord = 0;
            var model = new ProductDao().Search(search, ref totalRecord);

            ViewBag.Total = totalRecord;
           
            ViewBag.Keyword = search;
           

            return View(model);
        }
      
    }
}