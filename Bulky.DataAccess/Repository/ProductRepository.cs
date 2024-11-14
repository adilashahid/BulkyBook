using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>,IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Product product)
        {
            var objfromdb = _context.Products.FirstOrDefault(u => u.Id == product.Id);
            if (objfromdb != null)
            {
                objfromdb.Title = product.Title;
                objfromdb.Description = product.Description;
                objfromdb.Price50 = product.Price50;
                objfromdb.ISBN = product.ISBN;
                objfromdb.Listprice = product.Listprice;
                objfromdb.Price = product.Price;
                objfromdb.Price100 = product.Price100;
                objfromdb.CategoryId = product.CategoryId;
                objfromdb.Author = product.Author;
                //if(objfromdb.ImageUrl!=null)
                //{
                //    objfromdb.ImageUrl=product.ImageUrl;
                //}

            }
        }
    }
}
