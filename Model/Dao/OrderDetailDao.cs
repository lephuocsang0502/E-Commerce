using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    
    public class OrderDetailDao
    {
        OnlineDbContext db = null;
        public OrderDetailDao()
        {
            db = new OnlineDbContext();
        }
        public bool Insert(OrderDetail details)
        {
            try
            {
                db.OrderDetails.Add(details);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            
        }
        public List<OrderDetail> GetAll()
        {
            return db.OrderDetails.ToList();
        }

    }
}
