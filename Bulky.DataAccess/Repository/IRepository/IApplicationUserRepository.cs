﻿using BulkyBook.Model.Models;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
      
        void Update(ApplicationUser applicationUser);
      
    }
}
