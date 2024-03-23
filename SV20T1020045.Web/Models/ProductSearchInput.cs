using SV20T1020045.Web.Models;

namespace SV20T1020045.Web.Models
{
    public class ProductSearchInput : PaginationSearchInput
    {
        public int CategoryID { get; set; } = 0;
        public int SupplierID { get; set; } = 0;
        public int minPrice { get; set; } = 0;
        public int maxPrice { get; set; } = 0;

    }
}
 