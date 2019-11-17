using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PConsulting.Data;
using PConsulting.ViewModels;
using System.Diagnostics;
using System.Linq;

namespace PConsulting.Controllers
{
    public class HomeController : Controller
    {
        private ArticleService _articleService;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ArticleService articleService, UserManager<ApplicationUser> userManager)
        {
            _articleService = articleService;

            _userManager = userManager;
        }

        public IActionResult Index()
        {
            //Get latest approved 10 articles
            var articlesLatest = _articleService.GetByApprovalState(true, 10).ToList();

            //Get top 10 liked articles
            var articlesLiked = _articleService.GetByLikes(10).ToList();

            HomePageVM vm = new HomePageVM
            {
                LatestArticles = articlesLatest,
                MostLikedArticles = articlesLiked
            };

            return View(vm);
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
