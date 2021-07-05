using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using shopapp.business.Abstract;
using shopapp.entity;
using shopapp.webui.Extensions;
using shopapp.webui.Identity;
using shopapp.webui.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static shopapp.webui.Models.RoleModel;

namespace shopapp.webui.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;

        public AdminController(IProductService productService, ICategoryService categoryService, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            this._productService = productService;
            this._categoryService = categoryService;
            this._roleManager = roleManager;
            this._userManager = userManager;
        }

        public async Task<IActionResult> UserEdit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var selectedRoles = await _userManager.GetRolesAsync(user);//kullanıcının rolleri
                var roles = _roleManager.Roles.Select(i => i.Name);//tüm roller

                ViewBag.Roles = roles;
                return View(new UserDetailsModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    SelectedRoles = selectedRoles
                });
            }
            return Redirect("~/admin/user/list");
        }


        [HttpPost]
        public async Task<IActionResult> UserEdit(UserDetailsModel model, string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.EmailConfirmed = model.EmailConfirmed;

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        selectedRoles = selectedRoles ?? new string[] { };//gelen roller yoksa, boş bir dizi atar.

                        await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles).ToArray<string>());//formdan gelen rolleri ile veritabanındaki eski rollerini karşılaştıracak ve aynı rolleri çıkaracak ve daha önce olmayan rolleri eklemiş olacak.

                        await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles).ToArray<string>());// veritabanındaki eski roller arasından formdan gelen rolleri çıkaracak geriye kalan, olmaması gereken roller silinecek.

                        return Redirect("/admin/user/list");
                    }
                }
                return Redirect("/admin/user/list");
            }
            return View(model);

        }

        public IActionResult UserList()
        {
            return View(_userManager.Users);
        }

        public async Task<IActionResult> RoleEdit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            var members = new List<User>();//yukarıdaki rolename e ait olan kullanıcılar
            var nonMembers = new List<User>();//ait olmayan kullanıcılar

            foreach (var user in _userManager.Users)//tüm kullanıcıları döner
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name)?members:nonMembers;
                //ilgili kullanıcı yukarıdaki role aitse true değilse false döner. True ise list members ın referansını, false ise nonMembers ın referansını alır.

                list.Add(user);
            }

            var model = new RoleDetails()
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var userId in model.IdsToAdd ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }

                foreach (var userId in model.IdsToDelete ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }
            }
            return Redirect("/admin/role/" + model.RoleId);
        }

        public IActionResult RoleList()
        {
            return View(_roleManager.Roles);
        }

        public IActionResult RoleCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));

                if (result.Succeeded)
                {
                    return RedirectToAction("RoleList");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        public IActionResult ProductList()
        {
            return View(new ProductListViewModel()
            {
                Products = _productService.GetAll()

            });
        }

        public IActionResult CategoryList()
        {
            return View(new CategoryListViewModel()
            {
                Categories = _categoryService.GetAll()

            });
        }

        public IActionResult ProductCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProductCreate(ProductModel model)
        {
            if (ModelState.IsValid)//ProductModel de belirttiğimiz tüm kriteler doğruysa
            {
                var entity = new Product()
                {
                    Name = model.Name,
                    Url = model.Url,
                    Price = model.Price,
                    Description = model.Description,
                    ImageUrl = model.ImageUrl
                };

                if (_productService.Create(entity))//create bize bool tipinde döner
                {
                    TempData.Put("message", new AlertMessage()
                    {
                        Title = "Yeni Kayıt",
                        Message = "Kayıt Eklendi!",
                        AlertType = "success"
                    });

                    return RedirectToAction("ProductList");
                }//Eğer bu if, false dönerse Validation metodunda ErrorMessage propertysinin içi dolar. 
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Hata",
                    Message = _productService.ErrorMessage,
                    AlertType = "danger"
                });

                return View(model);
            }

            return View(model);
        }

        public IActionResult CategoryCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CategoryCreate(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new Category()
                {
                    Name = model.Name,
                    Url = model.Url
                };

                _categoryService.Create(entity);

                TempData.Put("message", new AlertMessage()
                {
                    Title = "Yeni Kayıt",
                    Message = $"{entity.Name} isimli kategori eklendi!",
                    AlertType = "success"
                });
                //{"Message":"samsung isimli ürün eklendi!","AlertType":"success"} jsonconvert ile bu şekile çevrilir(Layout ta bu bilgi alınacak)

                return RedirectToAction("CategoryList");
            }

            return View(model);
        }


        public IActionResult ProductEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _productService.GetByIdWithCategories((int)id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new ProductModel()
            {
                ProductId = entity.ProductId,
                Name = entity.Name,
                Url = entity.Url,
                Price = entity.Price,
                ImageUrl = entity.ImageUrl,
                Description = entity.Description,
                IsApproved=entity.IsApproved,
                IsHome=entity.IsHome,
                SelectedCategories = entity.ProductCategories.Select(i => i.Category).ToList()
            };

            ViewBag.Categories = _categoryService.GetAll();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductModel model, int[] categoryIds, IFormFile file)
        {
            //categoryIds parametresine formdan checked olan checkboxların value bilgisi dizi olarak geliyor.
            if (ModelState.IsValid)//ProductModel de belirttiğimiz tüm kriteler doğruysa
            {
                var entity = _productService.GetById(model.ProductId);
                if (entity == null)
                {
                    return NotFound();
                }
                entity.Name = model.Name;
                entity.Price = model.Price;
                entity.Url = model.Url;
                entity.Description = model.Description;
                entity.IsApproved = model.IsApproved;
                entity.IsHome = model.IsHome;

                if (file != null)//resim yükleme kodu. resim adını entity e yazdık ve resmi images klasörüne kaydettik.
                {
                    var extention = Path.GetExtension(file.FileName);//resim uzantısı
                    var randomName = string.Format($"{Guid.NewGuid()}{extention}");//resim adı
                    entity.ImageUrl = randomName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", randomName);//kayıt yolu

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                if (_productService.Update(entity, categoryIds))//create bize bool tipinde döner
                {
                    TempData.Put("message", new AlertMessage()
                    {
                        Title = "Güncelleme",
                        Message = $"{entity.Name} ürün kaydı güncellendi!",
                        AlertType = "success"
                    });

                    return RedirectToAction("ProductList");
                }//Eğer bu if, false dönerse Validation metodunda ErrorMessage propertysinin içi dolar. 

                TempData.Put("message", new AlertMessage()
                {
                    Title = "Hata",
                    Message = _productService.ErrorMessage,
                    AlertType = "danger"
                });
            }

            ViewBag.Categories = _categoryService.GetAll();

            return View(model);
        }

        public IActionResult CategoryEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _categoryService.GetByIdWithProducts((int)id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new CategoryModel()
            {
                CategoryId = entity.CategoryId,
                Name = entity.Name,
                Url = entity.Url,
                Products = entity.ProductCategories.Select(p => p.Product).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult CategoryEdit(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _categoryService.GetById(model.CategoryId);
                if (entity == null)
                {
                    return NotFound();
                }
                entity.Name = model.Name;
                entity.Url = model.Url;

                _categoryService.Update(entity);

                TempData.Put("message", new AlertMessage()
                {
                    Title = "Güncelleme",
                    Message = $"{entity.Name} isimli kategori güncellendi!",
                    AlertType = "success"
                });

                return RedirectToAction("CategoryList");
            }

            return View(model);
        }


        public IActionResult DeleteProduct(int productId)
        {
            var entity = _productService.GetById(productId);

            if (entity != null)
            {
                _productService.Delete(entity);
            }

            TempData.Put("message", new AlertMessage()
            {
                Title = "Silme İşlemi",
                Message = $"{entity.Name} isimli ürün silindi!",
                AlertType = "danger"
            });

            return RedirectToAction("ProductList");
        }

        public IActionResult DeleteCategory(int categoryId)
        {
            var entity = _categoryService.GetById(categoryId);

            if (entity != null)
            {
                _categoryService.Delete(entity);
            }

            TempData.Put("message", new AlertMessage()
            {
                Title = "Silme İşlemi",
                Message = $"{entity.Name} isimli kategori silindi!",
                AlertType = "danger"
            });

            return RedirectToAction("CategoryList");
        }

        [HttpPost]
        public IActionResult DeleteFromCategory(int productId, int categoryId)
        {
            _categoryService.DeleteFromCatgory(productId, categoryId);
            return Redirect("/admin/categories/" + categoryId);

        }

    }
}