
using iStartWeb.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace iStartWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.PostAsJsonAsync("https://localhost:7230/api/users/Login", model);
            if(response.IsSuccessStatusCode)
            return RedirectToAction("Index", "Home");
            else
                return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.PostAsJsonAsync("https://localhost:7230/api/users/PostUser", model);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login");
                }
            }
            return RedirectToAction("Register", "Account");
        }

        
    }
}

