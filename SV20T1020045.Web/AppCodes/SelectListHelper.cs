using Microsoft.AspNetCore.Mvc.Rendering;
using SV20T1020045.BusinessLayers;

namespace SV20T1020045.Web
{
    public class SelectListHelper
    {
        /// <summary>
        /// Danh sách tỉnh thành 
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Provinces()
        {
            List<SelectListItem > list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn tỉnh/thành --"
            });
            foreach(var item in CommonDataService.ListOfProvinces())
            {
                list.Add(new SelectListItem()
                {
                    Value = item.ProvinceName,
                    Text = item.ProvinceName
                });
            }
            return list;
        }

        public static List<SelectListItem> Suppliers()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() 
            {
                Value = "0", 
                Text = "--Nhà cung cấp--" 
            });
            int rowCount = 0;
            foreach (var item in CommonDataService.ListOfSuppliers(out rowCount, 1, 0, ""))
            {
                list.Add(new SelectListItem()
                {
                    Value = item.SupplierID.ToString(),
                    Text = item.SupplierName
                });
            }
            return list;
        }
        public static List<SelectListItem> Categories()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() 
            {
                Value = "0", 
                Text = "--Loại hàng--"
            });
            int rowCount = 0;
            foreach (var c in CommonDataService.ListOfCategories(out rowCount, 1, 0, ""))
            {
                list.Add(new SelectListItem()
                {

                    Value = c.CategoryID.ToString(),
                    Text = c.CategoryName

                });
            }
            return list;
        }
      
    }
}
