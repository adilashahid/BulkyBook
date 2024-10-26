using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Model.Models;

namespace BulkyBook.DataAccess.Repository
{
    public class OrderHeadetRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderHeadetRepository(ApplicationDbContext context):base(context)
        {
            _context = context;  
        }

        
        public void Update(OrderHeader obj)
        {
            _context.OrderHeaders.Update(obj);
        }

        

        public  void UpdateStasus(int id, string orderStasus, string? paymentStasus)
        {
            var orderFromDb=_context.OrderHeaders.FirstOrDefault(x => x.Id == id);
            if (orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStasus;
                if (!string.IsNullOrEmpty(paymentStasus))
                {
                    orderFromDb.PaymentStasus= paymentStasus;

                }
            }
        }

        public  void UpdateStripePayment(int id, string sessionId, string paymentIntenId)
        {
            var orderFromDb = _context.OrderHeaders.FirstOrDefault(x => x.Id == id);
            if (!string.IsNullOrEmpty(sessionId))
            {
                orderFromDb.SessionId= sessionId;
            }
            if (!string.IsNullOrEmpty(paymentIntenId))
            {
                orderFromDb.PaymentIntenId = paymentIntenId;
                orderFromDb.PaymentDate = DateTime.Now;
            }
        }
    }
}
