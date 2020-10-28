using Common;
using Model.Dao;
using Model.EF;
using Newtonsoft.Json.Linq;
using OnlineShop.Common;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace OnlineShop.Controllers
{
    public class CartController : Controller
    {
       
        private const string CartSession = "CartSession";
        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }

            return View(list);
        }

        public ActionResult AddItem(long productId, int quantity)
        {
            
            var product = new ProductDao().ViewDetail(productId);
            var cart = Session[CartSession];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.Product.ID == productId))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.ID == productId)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = quantity;
                    list.Add(item);
                }
                Session[CartSession] = list;
            }
            else
            {
                var item = new CartItem();
                item.Product = product;
                item.Quantity = quantity;
                var list = new List<CartItem>();
                list.Add(item);
                Session[CartSession] = list;
            }
            return RedirectToAction("Index");
        }
        public JsonResult DeleteAll()
        {

            Session[CartSession] = null;
            return Json(new
            {
                status = true
            });
        }
        public JsonResult Delete(long id)
        {
            var sessionCart = (List<CartItem>)Session[CartSession];
            sessionCart.RemoveAll(x => x.Product.ID == id);
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }
        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[CartSession];
            foreach(var item in sessionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.Product.ID == item.Product.ID);
                if (jsonItem != null)
                {
                    item.Quantity = jsonItem.Quantity;
                }
            }
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });                            
        }
        public ActionResult Payment()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;


            }

            return View(list);
        }
        [HttpPost]
        
        public ActionResult Payment(string shipName, string address, string mobile, string email)
        {
            var session = (UserLogin)Session[Common.CommonConstants.USER_SESSION];
           
            var order = new Order();
            
            if(session!=null)
            {
                order.CustomerID = session.UserID;
                order.CreateDate = DateTime.Now;
                order.ShipAddress = address;
                order.ShipEmail = email;
                order.ShipMobile = mobile;
                order.ShipName = shipName;
                order.Status = false;
               
            }
            else
            {
                order.CreateDate = DateTime.Now;
                order.ShipAddress = address;
                order.ShipEmail = email;
                order.ShipMobile = mobile;
                order.ShipName = shipName;
                order.Status = false;
                
            }
           
            try
            {
               
                var id = new OrderDao().Insert(order);
                
                var cart = (List<CartItem>)Session[CartSession];
                var detailDao = new OrderDetailDao();
                var orderdao = new OrderDao();
                decimal total = 0;
                foreach (var item in cart)
                {
                    var orderr = new Order();
                    var orderDetail = new OrderDetail();
                    
                    orderDetail.ProductID = item.Product.ID;
                    orderDetail.Price = item.Product.Price;
                    orderDetail.OrderID = id;
                    orderDetail.Quantity = item.Quantity;
                    total += (item.Product.Price.GetValueOrDefault(0) * item.Quantity);
                    orderDetail.Total = total;
                    
                  
                   
                    detailDao.Insert(orderDetail);
                    

                }
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/client/template/neworder.html"));

                content = content.Replace("{{CustomerName}}", shipName);
                content = content.Replace("{{Phone}}", mobile);
                content = content.Replace("{{Email}}", email);
                content = content.Replace("{{Address}}", address);
                content = content.Replace("{{Total}}", total.ToString("N0"));
                var toEmail = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();

               /* new MailHelper().SendMail(email, "Đơn hàng mới từ OnlineShop", content);
                new MailHelper().SendMail(toEmail, "Đơn hàng mới từ OnlineShop", content);*/

                string endpoint = ConfigurationManager.AppSettings["endpoint"].ToString();
                string partnerCode = ConfigurationManager.AppSettings["partnerCode"].ToString();
                string accessKey = ConfigurationManager.AppSettings["accessKey"].ToString();
                string serectKey = ConfigurationManager.AppSettings["serectKey"].ToString();
                string orderInfo = "DH" + DateTime.Now.ToString("yyyyMMddHHmmss");
                string returnUrl = ConfigurationManager.AppSettings["returnUrl"].ToString();
                string notifyUrl= ConfigurationManager.AppSettings["notifyUrl"].ToString();

                string amount = total.ToString();
                string orderId = Guid.NewGuid().ToString();
                string requestId=Guid.NewGuid().ToString();
                string extraData = order.ShipName+"SP"+order.ShipMobile;

                string rawHash = "partnerCode=" +
                    partnerCode + "&accessKey=" +
                    accessKey + "&requestId=" +
                    requestId + "&amount=" +
                    amount + "&orderId=" +
                    orderId + "&orderInfo=" +
                    orderInfo + "&returnUrl=" +
                    returnUrl + "&notifyUrl=" +
                    notifyUrl + "&extraData=" +
                    extraData;


                MoMoSecurity crypto = new MoMoSecurity();
                //sign signature SHA256
                string signature = crypto.signSHA256(rawHash, serectKey);
                JObject message = new JObject
                {
                    { "partnerCode", partnerCode },
                    { "accessKey", accessKey },
                    { "requestId", requestId },
                    { "amount", amount },
                    { "orderId",orderId },
                    { "orderInfo", orderInfo },
                    { "returnUrl", returnUrl },
                    { "notifyUrl", notifyUrl },
                    { "extraData", extraData },
                    { "requestType", "captureMoMoWallet" },
                    { "signature", signature }

                };

                string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

                JObject jmessage = JObject.Parse(responseFromMomo);

                return Redirect(jmessage.GetValue("payUrl").ToString());

            }
            catch(Exception ex)
            {
               
                throw (ex);
            }
            
        }
        public ActionResult Success()
        {
            /*string param = Request.QueryString.ToString().Substring(0, Request.QueryString.ToString().IndexOf("signature") - 1);
            param = Server.UrlDecode(param);
            MoMoSecurity crypto = new MoMoSecurity();
            string serectKey = ConfigurationManager.AppSettings["serectkey"].ToString();
            string signature = crypto.signSHA256(param, serectKey);
            if (signature != Request["signature"].ToString())
            {
                ViewBag.message = "thong tin req khong hop le";
                return View();
            }
            if (!Request.QueryString["errorCode"].Equals("0"))
            {
                ViewBag.message = "Payment failed";

            }
            else
            {
                ViewBag.message = "Payment Success";
                Session["Cart"] = new List<CartItem>();
            }*/
            return View();
        }


     
    }
}