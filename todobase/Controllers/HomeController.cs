using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using todobase.Models;

namespace todobase.Controllers
{
    public class HomeController : Controller
    {
        private readonly TodoRepository _repository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(AppDbContext context, ILogger<HomeController> logger)
        {
            _repository = new TodoRepository(context);
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var todos = await _repository.GetAllAsync();
            return View(todos);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
