using Google.Protobuf.WellKnownTypes;
using Microsoft.Ajax.Utilities;
using Model.Dao;
using Model.EF;
using Nest;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class ChartController : Controller
    {
        OnlineDbContext db = new OnlineDbContext();
        // GET: Admin/Chart
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
        public decimal TotalSalebyMonth(int month,int year)
        {
            
            var list = db.Orders.Where(x => x.CreateDate.Month == month && x.CreateDate.Year == year);
            decimal total = 0;
            var a = from b in db.OrderDetails
                    join c in list on b.OrderID equals c.ID
                    select new { b.Total};
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

        public ActionResult GetData()
        {
            var query = db.OrderDetails.Include("Product")
                .GroupBy(x => x.Product.Name)
                .Select(g => new { name = g.Key, count = g.Sum(x=>x.Quantity)}).ToList();
            
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetData01()
        {
            var query = db.OrderDetails.Include("Product")
                .GroupBy(x => x.Product.ProductCategory.Name)
                .Select(g => new { name = g.Key, count = g.Sum(x => x.Quantity) }).ToList();

            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetData02()
        {
            string[] monthNames = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
            
            var query = db.OrderDetails.Include("Order")
                .GroupBy(x=>x.Order.CreateDate.Month)
                .Select(g => new { name =g.Key, count = g.Sum(x => x.Total) })
                .ToList();
          
           foreach(var item in query)
            {
               for(int i = 0; i < monthNames.Length; i++)
                {
                    item.name.Equals(monthNames[i]);
                }
            }


            return Json(query, JsonRequestBehavior.AllowGet);
        }
    }
}