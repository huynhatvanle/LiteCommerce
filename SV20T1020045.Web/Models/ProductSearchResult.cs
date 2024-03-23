using SV20T1020045.DomainModels;
namespace SV20T1020045.Web.Models
{
    public class ProductSearchResult : BasePaginationResult
    {
        public int CategoryID { get; set; }
        public int SupplierID { get; set; }
        public decimal minPrice { get; set; }
        public decimal maxPrice { get; set; }
        public List<Product>? Data { get; set; }
        public List<Supplier>? suppliers { get; set; }
        public List<Category>? categories { get; set; }
    }
}
