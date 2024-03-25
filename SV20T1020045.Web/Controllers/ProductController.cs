using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020045.BusinessLayers;
using SV20T1020045.DomainModels;
using SV20T1020045.Web.Models;
using System.Reflection;

namespace SV20T1020045.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
    public class ProductController : Controller
    {
        private const int PAGE_SIZE = 25;
        private const string PRODUCT_SEARCH = "product_search";
        public IActionResult Index()
        {
             ProductSearchInput? input = ApplicationContext.GetSessionData<ProductSearchInput>(PRODUCT_SEARCH);
            if (input == null)
            {
                input = new ProductSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    CategoryID = 0,
                    SupplierID = 0,
                    minPrice = 0,
                    maxPrice = 0,
                };
            }
            return View(input);
        }
        public IActionResult Search(ProductSearchInput input)
        {
            int rowCount = 0;
            var data = ProductDataService.ListProducts(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "", input.CategoryID, input.SupplierID, input.minPrice, input.maxPrice);

            var model = new ProductSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                RowCount = rowCount,
                SearchValue = input.SearchValue ?? "",
                Data = data
            };

            ApplicationContext.SetSessionData(PRODUCT_SEARCH, input);
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung mặt hàng ";
            var model = new Product()
            {
                ProductID = 0
            };
            return View("Edit", model);
        }
        public IActionResult Save(Product data, IFormFile? uploadPhoto)
        {
            if (string.IsNullOrWhiteSpace(data.ProductDescription))
            {
                ModelState.AddModelError("ProductDescription", "Mô tả không được để trống");
            }
            if (string.IsNullOrWhiteSpace(data.ProductName))
            {
                ModelState.AddModelError("ProductName", "Tên mặt hàng không được để trống");
            }
            if (string.IsNullOrWhiteSpace(data.SupplierID.ToString()))
            {
                ModelState.AddModelError("SupplierID", "Tên nhà cung cấp không được để trống");
            }
            if (string.IsNullOrWhiteSpace(data.CategoryID.ToString()))
            {
                ModelState.AddModelError("CategoryID", "Tên loại hàng không được để trống");
            }
            if (string.IsNullOrWhiteSpace(data.Unit))
            {
                ModelState.AddModelError("Unit", "Đơn vị tính không được để trống");
            }
            if (string.IsNullOrWhiteSpace(data.Price.ToString()))
            {
                ModelState.AddModelError("Price", "Giá không được để trống");
            }
            if (uploadPhoto != null)
            {
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                string folder = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, @"images/products");
                string filepath = Path.Combine(folder, fileName);
                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = fileName;
            }
            if (!ModelState.IsValid)
            {

                ViewBag.Title = data.ProductID == 0 ? "Bổ sung mặt hàng" : "Cập nhật mặt hàng";
                return View("Edit", data);
            }
            if (data.ProductID == 0)
            {
                ProductDataService.AddProduct(data);

            }
            else
            {
                ProductDataService.UpdateProduct(data);

            }
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id )
        {
            ViewBag.Title = "Cập nhật thông tin mặt hàng";
          
            Product? model = ProductDataService.GetProduct(id);
            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }
        public IActionResult Delete(int id)
        {
            if (Request.Method == "POST")
            {
                ProductDataService.DeleteProduct(id);
                return RedirectToAction("Index");
            }
            var model = ProductDataService.GetProduct(id);
            if (model == null)
                return RedirectToAction("Index");
            ViewBag.allowDelete = !ProductDataService.IsUsedProduct(id);
            return View(model);

        }
        public IActionResult Photo(int id, string method, int photoId = 0)
        {
            ProductPhoto? model = new ProductPhoto();
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh";
                    model = new ProductPhoto()
                    {
                        PhotoID = 0,
                        ProductID = id
                    };
                    break;
                case "edit":
                    ViewBag.Title = " Thay đổi ảnh";
                    model = ProductDataService.GetPhoto( photoId);
                    if (model == null)
                        return RedirectToAction("Edit", new { id = id });
                    break;
                case "delete":
                    // Xóa ảnh (xóa trực tiếp, không cần confirm)
                    ProductDataService.DeletePhoto(photoId);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult SavePhoto(ProductPhoto data, IFormFile uploadPhoto )
        {
            //Xử lý ảnh upload
            if (uploadPhoto != null)
            {
                string filename = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                string folder = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, @"images/products");
                string filePath = Path.Combine(folder, filename);// Đương dẫn đến file cần lưu

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = filename;
            }
            if (string.IsNullOrWhiteSpace(data.Photo))
            {
                ModelState.AddModelError("Photo", "Ảnh không được để trống");
            }
            if (string.IsNullOrWhiteSpace(data.Description))
            {
                ModelState.AddModelError("Description", "Mô tả không được để trống");
            }
            if (string.IsNullOrWhiteSpace(data.DisplayOrder.ToString()))
            {
                ModelState.AddModelError("DisplayOrder", "Thứ tự không được để trống");
            }
            if (!ModelState.IsValid)
            {
                return View("Photo", data);
            }
            if (data.PhotoID == 0)
            {
                long id = ProductDataService.AddPhoto(data);
            }
            else
            {
                bool result = ProductDataService.UpdatePhoto(data);
            }
            return RedirectToAction("Edit", new { id = data.ProductID });
        }

        public IActionResult Attribute(int id, string method, int attributeId = 0)
        {
            ProductAttribute? model = new ProductAttribute();
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính mặt hàng";
                    model = new ProductAttribute()
                    {
                        AttributeID = 0,
                        ProductID = id
                    };
                    return View(model);
                case "edit":
                    ViewBag.Title = "Thay đổi thuộc tính mặt hàng";
                    model = ProductDataService.GetAttribute(attributeId);
                    if (model == null)
                        return RedirectToAction("Index");

                    return View(model);
                case "delete":
                    // Xóa thuộc tính (xóa trực tiếp, không cần confirm)
                    ProductDataService.DeleteAttribute(Convert.ToInt64(attributeId));
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult SaveAttribute(ProductAttribute data)
        {
            if (string.IsNullOrWhiteSpace(data.AttributeName))
            {
                ModelState.AddModelError("AttributeName", "Tên thuộc tính không được để trống");
            }
            if (string.IsNullOrWhiteSpace(data.AttributeValue))
            {
                ModelState.AddModelError("AttributeValue", "Giá trị không được để trống");
            }
            if (string.IsNullOrWhiteSpace(data.DisplayOrder.ToString()))
            {
                ModelState.AddModelError("DisplayOrder", "Thứ tự không được để trống");
            }

            if (!ModelState.IsValid)
            {

                return View("Attribute", data);
            }

            if (data.AttributeID == 0)
            {

                ProductDataService.AddAttribute(data);
                var product = ProductDataService.GetProduct(data.ProductID);
                return View("Edit",product);
            }
            else
            {
                ProductDataService.UpdateAttribute(data);
                return RedirectToAction("Edit", new { productID = data.ProductID });
            }
        }
    }
}
