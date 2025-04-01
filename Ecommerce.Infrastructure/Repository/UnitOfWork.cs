using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        public IVillaRepository Villa {get; private set;}
        public IVillaNumberRepository VillaNumber { get; private set; }
        public IAmenityRepository Amenity { get; private set; }
        public IBookingRepository Booking { get; private set; }
        public IApplicationUserRepository User { get; private set; }

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            Villa = new VillaRepository(_dbContext);
            VillaNumber = new VillaNumberRepository(_dbContext);
            Amenity = new AmenityRepository(_dbContext);
            Booking = new BookingRepository(_dbContext);
            User = new ApplicationUserRepository(_dbContext);
        }
        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
