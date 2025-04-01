using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Application.Common.Utility;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Data;

namespace Ecommerce.Infrastructure.Repository
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public BookingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public void Update(Booking entity)
        {
            throw new NotImplementedException();
        }

        //public void UpdateStatus(int bookingId, string bookingStatus, int villaNumber=0)
        //{
        //    var bookingFromDb = _dbContext.Bookings.FirstOrDefault(b => b.Id == bookingId);
        //    if (bookingFromDb != null)
        //    {
        //        bookingFromDb.status = bookingStatus;
        //        if (bookingStatus == SD.Status_CheckedIn)
        //        {
        //            bookingFromDb.VillaNumber = villaNumber;
        //            bookingFromDb.ActualCheckInDate = DateTime.Now;
        //        }
        //        if (bookingStatus == SD.Status_Completed)
        //        {
        //            bookingFromDb.ActualCheckOutDate = DateTime.Now;
        //        }
        //    }
        //}

        //public void UpdateStripePaymentID(int bookingId, string sessionId, string paymentIntentId)
        //{
        //    var bookingFromDb = _dbContext.Bookings.FirstOrDefault(b => b.Id == bookingId);
        //    if (bookingFromDb != null)
        //    {
        //        if (!string.IsNullOrEmpty(sessionId))
        //        {
        //            bookingFromDb.StripeSessionId = sessionId;
        //        }
        //        if (!string.IsNullOrEmpty(paymentIntentId))
        //        {
        //            bookingFromDb.StripePaymentIntentId = paymentIntentId;
        //            bookingFromDb.PaymentDate = DateTime.Now;
        //            bookingFromDb.IsPaymentSuccessful = true;
        //        }
        //    }
        //}
    }
}
