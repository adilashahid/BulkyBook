﻿using BulkyBook.Model.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess.Data
{
    public class ApplicationDbContext:IdentityDbContext<IdentityUser>
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)

        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData
                (
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
               new Category { Id = 2, Name = "History", DisplayOrder = 2 }

                );
            modelBuilder.Entity<Company>().HasData
                (
                new Company { Id = 1, Name = "Solution",
                    StreetAddress="123 sol",
                    City="Multan",
                    PostalCode="121212" ,
                 State="IL",
                    PhoneNumber="12345678"},
              new Company
              {
                  Id = 2,
                  Name = "Solution",
                  StreetAddress = "123 sol",
                  City = "Multan",
                  PostalCode = "121212"
                ,
                  State = "IL",
                  PhoneNumber = "12345678"
              }

                );
            modelBuilder.Entity<Product>().HasData(
           new Product
           {
               Id = 1,
               Title= "Fortune Of Time",
               Author="Bily Spark",
               Description="Availabe",
               ISBN="SW199999", 
               Price=19,
               Price50=80,
               Price100=110,
               CategoryId=1
               
           },
           new Product
           {
               Id = 2,
               Title = "Fortune Of Time",
               Author = "Bily Spark",
               Description = "Availabe",
               ISBN = "SW199999",
               Price = 19,
               Price50 = 80,
               Price100 = 110,
               CategoryId = 2
              
           }
                )
                ;

            base.OnModelCreating(modelBuilder);
        }
       
        
        }
    }

