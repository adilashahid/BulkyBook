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
    }
}
