using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Model.Models;

namespace BulkyBook.DataAccess.Repository
{
    public class CategoryRepository :Repository<Category>,ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context):base(context)
        {
            _context = context;  
        }

        
        public void Update(Category category)
        {
            _context.Update(category);
        }
    }
}
