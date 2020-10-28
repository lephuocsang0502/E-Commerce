using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ProductCategoryDao
    {
        OnlineDbContext db = null;
        public ProductCategoryDao()
        {
            db = new OnlineDbContext();
        }
        public long Insert(ProductCategory productCategory)
        {
            db.ProductCategories.Add(productCategory);
            db.SaveChanges();
            return productCategory.ID;
        }
        public List<ProductCategory> ListAll()
        {
            return db.ProductCategories.Where(x => x.Status == true).ToList();
        }

        public ProductCategory ViewDetail(long id)
        {
            return db.ProductCategories.Find(id);
        }
    }
}
