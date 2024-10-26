using BulkyBook.Model.Models;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
      
        void Update(OrderHeader obj);
        void UpdateStasus(int id, string orderStasus, string? paymentStasus = null);
        void UpdateStripePayment(int id, string sessionId, string paymentIntenId);
    }
}
