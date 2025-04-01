using ECommerce.Application.Common.DTO;

namespace Ecommerce.Application.Services.Interface
{
    public interface IDashboardService
    {
        Task<RadioBarChartDto> GetTotalBookingRadialChartData();
        Task<RadioBarChartDto> GetRegisteredUserChartData();
        Task<RadioBarChartDto> GetRevenueChartData();
        Task<PieChartDto> GetBookingPieChartData();
        Task<LineChartDto> GetMemberAndBookingLineChartData();
    }
}
