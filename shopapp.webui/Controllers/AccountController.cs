using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using shopapp.webui.Identity;
using shopapp.webui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shopapp.webui.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;//kullanıcı oluşturma, login, parola sıfırlama
        private SignInManager<User> _signInManager;//cookie işlemleri yönetimi için hazır Identity sınıfları

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        public IActionResult Login(string returnUrl=null)
        {
            return View(new LoginModel()
            {
                ReturnUrl = returnUrl//yakalanan ReturnUrl yi ReturnUrl propertisine atar ve view a gönderir.
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // var user = await _userManager.FindByNameAsync(model.UserName);
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                //CreateMessage("Bu e-posta ile daha önce bir hesap oluşturulmamış", "danger");
                ModelState.AddModelError("", "Bu e-posta ile daha önce hesap oluşturulmamış!");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);//ilk true:60 dk süre sonunda cookie silinirve çıkış yapar eğer tekrar istek yoksa. istek olduğunda süre baştan başlar. false ise süre bitiminde çıkış yapılır. , ikinci false:başarısız girişte hesap kilitlenme işlemi

            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl ?? "~/");//~/ home index i temsil eder
            }

            //CreateMessage("Girilen kullanıcı adı veya parola yanlış", "danger");
            ModelState.AddModelError("", "Girilen kullanıcı adı veya parola yanlış!");
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // generate token
                // email
                return RedirectToAction("Login", "Account");
            }

            ModelState.AddModelError("", "Bilinmeyen hata oldu lütfen tekrar deneyiniz.");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/");//home/index
        }

            //private void CreateMessage(string message, string allertType)
            //{
            //    var msg = new AlertMessage()
            //    {
            //        Message = message,
            //        AlertType = allertType
            //    };
            //    TempData["message"] = JsonConvert.SerializeObject(msg);
            //    //{"Message":"samsung isimli ürün eklendi!","AlertType":"success"} jsonconvert ile bu şekile çevrilir(Layout ta bu bilgi alınacak)
            //}
        }
}
