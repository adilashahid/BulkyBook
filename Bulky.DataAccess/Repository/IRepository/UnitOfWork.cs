﻿using BulkyBook.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public UnitOfWork(ApplicationDbContext context) 
        {
            _context = context;
            Category= new CategoryRepository(_context);
            Product= new ProductRepository(_context);
            Company=new CompanyRepository(_context);

        }
    

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
