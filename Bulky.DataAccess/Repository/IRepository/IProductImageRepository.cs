using BulkyBook.Model.Models;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IProductImageRepository : IRepository<ProductImage>
    {
      
        void Update(ProductImage productImage);
      
    }
}
