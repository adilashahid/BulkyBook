﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
       
        IEnumerable<T> GetAll(Expression<Func<T,bool>>? filter=null,string? includePropertities = null);
        T Get(Expression<Func<T, bool>> filter, string? includePropertities = null, bool track=false);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
