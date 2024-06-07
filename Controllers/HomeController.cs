using System.Diagnostics;
using LRTV.ContextModels;
using LRTV.Models;
using LRTV.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LRTV.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PlayersContext _context;

        public HomeController(ILogger<HomeController> logger, PlayersContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var teams = _context.Teams.OrderBy(t => t.Ranking).Take(5).ToList();
            var news = _context.News.OrderByDescending(n => n.Data).Take(5).ToList();
            var matches = _context.Matches.OrderByDescending(m => m.DateTime).Take(5).ToList();
            var topRatedPlayer = _context.Players.OrderByDescending(p => p.Rating).FirstOrDefault();

            var viewModel = new HomePageViewModel
            {
                Teams = teams,
                News = news,
                Matches = matches,
                Player = topRatedPlayer
            };

            return View(viewModel);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
