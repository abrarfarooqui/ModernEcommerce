using Ecommerce.Domain.Entities;
using ECommerce.Application.Common.DTO;

namespace Ecommerce.Application.Common.Utility
{
    public static class SD
    {
        public const string Role_Customer = "Customer";
        public const string Role_Admin = "Admin";

        public const string Status_Pending = "Pending";
        public const string Status_Approved = "Approved";
        public const string Status_CheckedIn = "CheckedIn";
        public const string Status_Completed = "Completed";
        public const string Status_Cancelled = "Cancelled";
        public const string Status_Refunded = "Refunded";

        public static int VillaRoomsAvailable_Count(int villaId, List<VillaNumber> villaNumberList, DateOnly checkInDate, int nights, List<Booking> bookings)
        {
            List<int> bookingInDate = new();
            int finalAvailableRoomsForAllNights = int.MaxValue;
            var roomsInVilla = villaNumberList.Where(x => x.VillaId == villaId).Count();

            for (int i = 0; i < nights; i++)
            {
                var villasBooked = bookings.Where(u => u.CheckInDate <= checkInDate.AddDays(i) && u.CheckOutDate > checkInDate.AddDays(i) && u.VillaId == villaId);
                foreach (var booking in villasBooked)
                {
                    if (!bookingInDate.Contains(booking.Id))
                    {
                        bookingInDate.Add(booking.Id);
                    }
                }
                var totalAvailbleRooms = roomsInVilla - bookingInDate.Count;
                if (totalAvailbleRooms == 0)
                {
                    return 0;
                }
                else
                {
                    if (finalAvailableRoomsForAllNights > totalAvailbleRooms)
                    {
                        finalAvailableRoomsForAllNights = totalAvailbleRooms;
                    }
                }
            }
            return finalAvailableRoomsForAllNights;
        }
        public static RadioBarChartDto GetRadialChartDataModel(int totalCount, double currentMonthCount, double previousMonthCount)
        {
            RadioBarChartDto radialBarChartVM = new();

            int increaseDecreaseRatio = 100;
            if (previousMonthCount != 0)
            {
                increaseDecreaseRatio = Convert.ToInt32((currentMonthCount - previousMonthCount) / previousMonthCount * 100);
            }
            radialBarChartVM.TotalCount = totalCount;
            radialBarChartVM.CountInCurrentMonth = Convert.ToInt32(currentMonthCount);
            radialBarChartVM.HasRatioIncreased = currentMonthCount > previousMonthCount;
            radialBarChartVM.Series = new int[] { increaseDecreaseRatio };

            return radialBarChartVM;
        }

    }
}
