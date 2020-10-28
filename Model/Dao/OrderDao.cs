using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class OrderDao
    {
        OnlineDbContext db = null;
        public OrderDao()
        {
            db = new OnlineDbContext();
        }
        public long Insert(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
            return order.ID;
        }
        public List<OrderDetail> ListOrderDetails(long? id)
        {
            var product = db.OrderDetails.FirstOrDefault(x=>x.OrderID==id);
            return db.OrderDetails.Where(x => x.OrderID == id ).ToList();
        }
        public bool ChangeStatus(long id)
        {
            var user = db.Orders.Find(id);
            user.Status = !user.Status;
            db.SaveChanges();
            return user.Status;
        }
    }
}
