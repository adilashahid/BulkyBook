using BulkyBook.Model.Models;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
      
        void Update(OrderHeader obj);
      
    }
}
