using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        OnlineDbContext db = new OnlineDbContext();
        // GET: Admin/Order
        public ActionResult Index()
        {

            return View(db.Orders.OrderByDescending(x => x.CreateDate).ToList());
        }
        public ActionResult Details(long? id)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var orderDetail = new OrderDao().ListOrderDetails(id);

            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            return View(orderDetail);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new OrderDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}