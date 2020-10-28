using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ContentDao
    {
        OnlineDbContext db = null;
        public static string USER_SESSION = "USER_SESSION";
        public ContentDao()
        {
            db = new OnlineDbContext();
        }
        public Content GetByID(long id)
        {
            return db.Contents.Find(id);
        }
    }
}
