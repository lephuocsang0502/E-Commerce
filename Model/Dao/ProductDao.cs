using Model.EF;
using PagedList;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Data.Entity.Migrations;

namespace Model.Dao
{
    public class ProductDao
    {
        OnlineDbContext db = null;
        public static string USER_SESSION = "USER_SESSION";
        public ProductDao()
        {
            db = new OnlineDbContext();
        }
        public List<Product> GetAll()
        {
            return db.Products.Where(x => x.Status == true).ToList();
        }
        public List<string> ListName(string keyword)
        {
            return db.Products.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }
        public Product GetByID(string name)
        {
            return db.Products.SingleOrDefault(x => x.Name == name);
        }
        public long Insert(Product entity)
        {
            
            db.Products.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public IEnumerable<Product> ListAllPaging( int page, int pageSize)
        {
            IQueryable<Product> model = db.Products;
            

            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        public bool Update(Product entity)
        {
            try
            {
                var product = db.Products.Find(entity.ID);
                product.Name = entity.Name;
                product.MetaTitle = entity.MetaTitle;
                product.Price = entity.Price;
                product.Description = entity.Description;
                product.Detail = entity.Detail;
                product.PromotionPrice = entity.PromotionPrice;
                product.TopHot = entity.TopHot;

                product.Image = entity.Image;
                product.Image1 = entity.Image1;
                product.Image2 = entity.Image2;
                product.Image3 = entity.Image3;
                product.CategoryID = entity.CategoryID;
                product.Status = entity.Status;
                product.ModifiedDate = DateTime.Now;
                
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                //logging
                return false;
            }

        }
        public bool Delete(int id)
        {
            try
            {
                var product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

            public List<Product> ListNewProduct(int top)
        {
            return db.Products.OrderByDescending(x => x.CreatedDate ).Take(top).ToList();
        }
    
        public List<Product> ListBestSale(int top)
        {
            return db.Products.OrderByDescending(o => o.OrderDetails.Sum(oi => oi.Quantity)).Take(top).ToList();
        }
        public List<Product> ListSale(int top)
        {
            return db.Products.Where(x => x.PromotionPrice != null && x.PromotionPrice >x.Price).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }

        public List<Product> ListRelatedProducts(long productId,int top)
        {
            var product = db.Products.Find(productId);
            return db.Products.Where(x => x.ID != productId && x.CategoryID==product.CategoryID).Take(top).ToList();
        }
        public IEnumerable<Product> ListByCategoryId(long categoryID, int page, int pageSize)
        {
            IQueryable<Product> model = db.Products;

            return model.OrderByDescending(x => x.CreatedDate).Where(x=>x.CategoryID==categoryID).ToPagedList(page, pageSize);
        }
     
        public Product ViewDetail(long id)
        {
            return db.Products.Find(id);
        }
        public List<ProductViewModel> Search(string keyword, ref int totalRecord)
        {
            totalRecord = db.Products.Where(x => x.Name == keyword).Count();
            var model = (from a in db.Products
                         join b in db.ProductCategories
                         on a.CategoryID equals b.ID
                         where a.Name.Contains(keyword)
                         select new
                         {
                             CateMetaTitle = b.MetalTitle,
                             CateName = b.Name,
                             CreatedDate = a.CreatedDate,
                             ID = a.ID,
                             Images = a.Image,
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Price = a.Price
                         }).AsEnumerable().Select(x => new ProductViewModel()
                         {
                             CateMetaTitle = x.MetaTitle,
                             CateName = x.Name,
                             CreatedDate = x.CreatedDate,
                             ID = x.ID,
                             Images = x.Images,
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Price = x.Price
                         });
            model.OrderByDescending(x => x.CreatedDate);
            return model.ToList();
        }
    }
}
