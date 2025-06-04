using System.Security.Claims;
using System.Text.Json;
using Keyper.Web1.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Keyper.Web1.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Auth()
        {
            return View(); // Giriş formu burada
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.UserName) || string.IsNullOrWhiteSpace(model.Password))
                return Json(new { success = false, message = "Boş alan bırakmayın." });

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("http://localhost:5039/account/check-login", model);

            if (!response.IsSuccessStatusCode)
            { 
                return Json(new { success = false, message = "Kullanıcı Adı veya Şifresi Hatalı" });
            }

            // Giriş başarılı -> Cookie oluştur
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.UserName)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                });

            return Json(new { success = true, message = "Giriş başarılı." });
        }

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var username = User.Identity?.Name;

            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Auth", "Account");
            

            var client = _httpClientFactory.CreateClient();

            
            var response = await client.GetAsync($"http://localhost:5039/getUserSites/{username}");
            

            if (!response.IsSuccessStatusCode)
            {
                ViewData["Error"] = "Kullanıcı bilgisi alınamadı.";
                return View(new List<DashboardViewModel>());
            }
            
            // ID çekmek için ayrı bir istek yapılıyor
            var context = new HttpClient();
            var userResponse = await context.GetAsync($"http://localhost:5039/getUserId/{username}");
            if (userResponse.IsSuccessStatusCode)
            {
                var json = await userResponse.Content.ReadAsStringAsync();
                ViewBag.UserId = json.Trim('"'); 
            }

            var jsonSites = await response.Content.ReadAsStringAsync();
            var dashboardViewModels = JsonSerializer.Deserialize<List<DashboardViewModel>>(jsonSites, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Console.WriteLine("Web tarafında kullanıcı: " + User.Identity?.Name);

            return View(dashboardViewModels);
        }
        

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Auth");
        }

        #region Crud işlemleri
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddSite([FromBody] SiteInfo model)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("http://localhost:5039/sites", model);
            var result = await response.Content.ReadAsStringAsync();

            Console.WriteLine("AddSite API cevabı: " + result); // 👈 BURASI ÖNEMLİ

            return Content(result, "application/json");
        }


        [Authorize]
        [HttpPut]
        public async Task<IActionResult> EditSite(int id, [FromBody] SiteInfo model)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.PutAsJsonAsync($"http://localhost:5039/sites/{id}", model);
            var result = await response.Content.ReadAsStringAsync();
            return Content(result, "application/json");
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteSite(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"http://localhost:5039/sites/{id}");
            var result = await response.Content.ReadAsStringAsync();
            return Content(result, "application/json");
        }
        
        
        #endregion
    }
}
