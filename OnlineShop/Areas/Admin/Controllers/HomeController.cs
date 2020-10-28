using Model.EF;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Rotativa;
namespace OnlineShop.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        OnlineDbContext db = new OnlineDbContext();
        // GET: Admin/Home
        public ActionResult Index(OrderDetail orderDetail)
        {
            ViewBag.PageView = HttpContext.Application["PageView"].ToString();
            ViewBag.ClientOnline = HttpContext.Application["ClientOnlineNow"].ToString();
            ViewBag.TotalSale = TotalSale();
            ViewBag.TotalSalebyMonth = TotalSalebyMonth(DateTime.Now.Month, DateTime.Now.Year);
            ViewBag.NewOrder = NewOrder();
            return View();


        }
        public decimal NewOrder()
        {
            decimal neworder = db.Orders.Count(x => x.Status == false);
            return neworder;
        }

        public decimal TotalSale()
        {
            decimal totalsale = db.OrderDetails.Sum(x => x.Total).Value;
            return totalsale;
        }
        public decimal TotalSalebyMonth(int month, int year)
        {

            var list = db.Orders.Where(x => x.CreateDate.Month == month && x.CreateDate.Year == year);
            decimal total = 0;
            var a = from b in db.OrderDetails
                    join c in list on b.OrderID equals c.ID
                    select new { b.Total };
            List<OrderDetail> listorder = new List<OrderDetail>();

            foreach (var item in a)
            {
                OrderDetail temp = new OrderDetail();
                temp.Total = item.Total;

                listorder.Add(temp);
            }


            total += decimal.Parse(listorder.Sum(x => x.Total).Value.ToString());


            return total;
        }
        /* public ActionResult ExportPDF()
         {
             return new ActionAsPdf("Index")
             {
                 FileName = Server.MapPath("~/Areas/Admin/Data/Export.pdf")
             };
         }*/
    }
}