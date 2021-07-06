using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using shopapp.business.Abstract;
using shopapp.webui.EmailServices;
using shopapp.webui.Extensions;
using shopapp.webui.Identity;
using shopapp.webui.Models;
using System.Threading.Tasks;

namespace shopapp.webui.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;//kullanıcı oluşturma, login, parola sıfırlama
        private SignInManager<User> _signInManager;//cookie işlemleri yönetimi için hazır Identity sınıfları
        private IEmailSender _emailSender;
        private ICartService _cartService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender, ICartService cartService)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._emailSender = emailSender;
            this._cartService = cartService;
        }

        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginModel()
            {
                ReturnUrl = returnUrl//yakalanan ReturnUrl yi ReturnUrl propertisine atar ve view a gönderir.
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]////get ile gönderilen token bilgisi posta gelmiyorsa bu durumda hata verecek
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

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Lütfen e-posta hesabınıza gelen link ile üyeliğinizi onaylayınız!");
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
        [ValidateAntiForgeryToken]
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
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail", "Account", new
                {
                    userId = user.Id,
                    token = code
                });

                // email
                await _emailSender.SendEmailAsync(model.Email, "Hesabınızı onaylayınız.", $"Lütfen email hesabınızı onaylamak için linke <a href='https://localhost:44397{url}'>tıklayınız.</a>");

                return RedirectToAction("Login", "Account");
            }

            ModelState.AddModelError("", "Bilinmeyen hata oldu lütfen tekrar deneyiniz.");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            TempData.Put("message", new AlertMessage()
            {
                Title = "Oturum Kapatıldı",
                Message = "Hesabınız güvenli bir şekilde kapatıldı.",
                AlertType = "warning"
            });

            return Redirect("~/");//home/index
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Geçersiz Token",
                    Message = "Geçertiz token!",
                    AlertType = "danger"
                });

            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    //cart objesini oluştur....
                    _cartService.InitializeCart(userId);

                    TempData.Put("message", new AlertMessage()
                    {
                        Title = "Hesabınız onaylanmıştır.",
                        Message = "Hesabınız onaylanmıştır.",
                        AlertType = "success"
                    });
                    return View();
                }
            }

            TempData.Put("message", new AlertMessage()
            {
                Title = "Hesabınız onaylanmadı!",
                Message = "Hesabınız onaylanmadı!",
                AlertType = "warning"
            });

            return View();

        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                TempData.Put("message", new AlertMessage()
                {
                    Title = "E-posta adresi girilmedi!",
                    Message = "E-posta adresi girilmedi!",
                    AlertType = "danger"
                });
                
                return View();
            }

            var user = await _userManager.FindByEmailAsync(Email);

            if (user == null)
            {
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Kullanıcı bulunamadı!",
                    Message = "Kullanıcı bulunamadı!",
                    AlertType = "danger"
                });

                return View();
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = Url.Action("ResetPassword", "Account", new
            {
                userId = user.Id,
                token = code
            });

            // email
            await _emailSender.SendEmailAsync(Email, "Parola Resetleme", $"Lütfen parolanızı resetlemek için linke <a href='https://localhost:44397{url}'>tıklayınız.</a>");
            
            TempData.Put("message", new AlertMessage()
            {
                Title = "Şifre yenileme",
                Message = "Şifre yenileme linkiniz e-posta adresinize gönderildi.",
                AlertType = "success"
            });

            return View();
        }

        public IActionResult ResetPassword(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Home", "Index");
            }

            var model = new ResetPasswordModel { Token = token };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return RedirectToAction("Home", "Index");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }

            TempData.Put("message", new AlertMessage()
            {
                Title = "Şifre yenileme",
                Message = "Şifre yenileme başarısız!",
                AlertType = "danger"
            });

            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
