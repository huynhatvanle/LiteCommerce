using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace SV20T1020045.Web.Controllers
{
    public class TestContoller : Controller
    {
        public IActionResult Create()
        {
            var model = new Models.Person()
            {
                Name="Nhật Huy",
                BirthDate= DateTime.Now,
                Salary = 10.25m
            };
            return View(model);
        }
        public IActionResult Save(Models.Person model, string BirthDateInput = "")
        {
            //chuyen birthdateInput sang gia tri kieu ngay
            DateTime? dvalue = stringtoDateTime(BirthDateInput);
            if (dvalue.HasValue)
            {
                model.BirthDate = dvalue.Value;
            }
            return Json(model);
        }
        private DateTime? stringtoDateTime(string s, string formats = "d/M/YYYY;d-M-yyyy;y.M.yyyy")
        {
            try
            {
                return DateTime.ParseExact(s, formats.Split(';'), CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }

    }

    
    }