using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020045.BusinessLayers;


namespace SV20T1020045.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login() 
        {
            return View();  
        }

        public IActionResult AccessDenined()
        {

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username= "", string password = "")
        {
            ViewBag.Username = username;
            if(string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("Error", "Nhập tên và mật khẩu!");
                return View();
            }
            var userAccount = UserAccountService.Authorize(username, password); 
            if(userAccount == null)
            {
                ModelState.AddModelError("Error", "Đăng nhập thất bại!");
                return View();  
            }
            //Đăng nhập thành công, tạo dữ liệu để lưu thông tin đăng nhập
            var userData = new WebUserData()
            {
                UserId = userAccount.UserID,
                UserName = userAccount.UserName,
                DisplayName = userAccount.FullName,
                Email = userAccount.Email,
                Photo = userAccount.Photo,  
                ClientIP = HttpContext.Connection.RemoteIpAddress?.ToString(),
                SessionId = HttpContext.Session.Id,
                AdditionalData="",
                Roles = userAccount.RoleNames.Split(',').ToList(),
            };
            await HttpContext.SignInAsync(userData.CreatePrincipal());
            /*return Json(User.GetUserData());*/
            return RedirectToAction("Index","Home");   
        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        public IActionResult InformationAccount()
        {
            return View();
        }
        public IActionResult ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(oldPassword))
                ModelState.AddModelError("oldPassword", "Vui lòng nhập mật khẩu");
            if (string.IsNullOrWhiteSpace(newPassword))
                ModelState.AddModelError("newPassword", "Vui lòng nhập mật khẩu mới");
            if (string.IsNullOrWhiteSpace(confirmPassword))
                ModelState.AddModelError("confirmPassword", "Vui lòng nhập lại mật khẩu mới");
            if (confirmPassword != newPassword)
            {
                ModelState.AddModelError("kt1", "Mật khẩu mới không trùng khớp!");
            }
            if (!ModelState.IsValid)
            {
                return View("InformationAccount");
            }
            var userData = User.GetUserData();
            bool result = UserAccountService.ChangePassword(userData.Email, oldPassword, newPassword);

            if (result)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("kt2", "Mật khẩu không đúng!");
                return View("InformationAccount");
            }
        }
    }
}
