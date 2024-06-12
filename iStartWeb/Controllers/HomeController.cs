
using iStartWeb.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;


namespace iStartWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var users = await httpClient.GetFromJsonAsync<IEnumerable<UserViewModel>>("https://localhost:7230/api/users/GetUsers");
            return View(users);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var res =  await client.GetFromJsonAsync<UserViewModel>($"https://localhost:7230/api/users/GetUsersById/{id}")!;
            return View(res);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Edit(UserViewModel Model)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("https://localhost:7230/api/users/PostUser", Model);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return RedirectToAction(nameof(Index));
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound();
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }


        // POST: User/Edit/5
    }
}
