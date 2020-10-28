using Model.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class SlideDescriptionDao
    {
        OnlineDbContext db = null;
        public SlideDescriptionDao()
        {
            db = new OnlineDbContext();
        }
        public Slide GetSlideDescription()
        {
            return db.Slides.FirstOrDefault(x => x.Status == true);
        }

    }
}