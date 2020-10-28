using Model.Dao;
using Model.EF;
using OnlineShop.Common;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class HomeController : Controller
    {

        // GET: Home
/*        [OutputCache(Duration = 3600 * 24)]*/
        public ActionResult Index()
        {
            ViewBag.Slides = new SlideDao().ListAll();
            var productDao = new ProductDao();
            ViewBag.NewsProduct = productDao.ListNewProduct(3);
            ViewBag.BestSale = productDao.ListBestSale(3);
            ViewBag.Sale = productDao.ListSale(3);
            return View();
        }
       
        [ChildActionOnly]
        public ActionResult MainMenu()
        {
            var model = new MenuDao().ListByGroupId(1);
            return PartialView(model);
        }

        [ChildActionOnly]
     
        public PartialViewResult HeaderCart()
        {
            var cart = Session[CommonConstants.CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
           
            return PartialView(list);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 3600 * 24)]
        public ActionResult Footer()
        {
            var model = new FooterDao().GetFooter();
            return PartialView(model);
        }

        [ChildActionOnly]
      /*  [OutputCache(Duration = 3600 * 24)]*/
        public ActionResult DescriptionSlide()
        {
            var model = new SlideDescriptionDao().GetSlideDescription();
            return PartialView(model);
        }
        [OutputCache(Duration = 3600 * 24)]
        public ActionResult About()
        {
            var model = new AboutDao().GetAbout();
            return PartialView(model);
        }
        [OutputCache(Duration = 3600 * 24)]
        public ActionResult Contact()
        {
            
            return PartialView();
        }
        public ActionResult Chat()
        {
            return View();
        }
    }
}