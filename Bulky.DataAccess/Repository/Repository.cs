using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbset;
        public Repository(ApplicationDbContext context)
        {
            _context = context; 
            this.dbset = _context.Set<T>();
            //dbset==_context.Caregories
            _context.Products.Include(u => u.Category);
        }
        public void Add(T entity)
        {
            dbset.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includePropertities = null, bool track = false)
        {
            IQueryable<T> query; 
            if(track)
            {
                query = dbset;
            }
            else
            {
                query = dbset.AsNoTracking();

            }
            
            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includePropertities))
            {
                foreach (var propert in includePropertities
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(propert);

                }
            }
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter,string? includePropertities=null)
        {
            IQueryable<T> query = dbset;
            if(filter != null)
            {
                query = query.Where(filter);

            }
           
            if (!string.IsNullOrEmpty(includePropertities))
            {
                foreach(var propert in includePropertities
                    .Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query=query.Include(propert);

                }
            }
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbset.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbset.RemoveRange(entity);
        }
    }
}
