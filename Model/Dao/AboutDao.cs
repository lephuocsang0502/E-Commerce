using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class AboutDao
    {
        OnlineDbContext db = null;
        public AboutDao()
        {
            db = new OnlineDbContext();
        }
        public About GetAbout()
        {
            return db.Abouts.SingleOrDefault(x => x.Status == true);
        }
    }
}
