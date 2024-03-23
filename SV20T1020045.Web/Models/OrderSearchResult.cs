using SV20T1020045.DomainModels;

namespace SV20T1020045.Web.Models
{
    /// <summary>
    /// biểu diễn dữ liệu kết quả tìm kiếm đơn hàng
    /// </summary>
    public class OrderSearchResult : BasePaginationResult
    {
        public int Status { get; set; } = 0;
        public string TimeRange { get; set; } = "";
        public List<Order> Data { get; set; } = new List<Order>();
    }
}
