using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PConsulting.Data;
using PConsulting.ViewModels;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PConsulting.Controllers
{
    public class HomeController : Controller
    {
        private ArticleService _articleService;
        private UserLikeService _userLikeService;
        private ArticleCommentService _articleCommentService;

        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ArticleService articleService, UserManager<ApplicationUser> userManager, UserLikeService userLikeService, ArticleCommentService articleCommentService)
        {
            _articleService = articleService;
            _userLikeService = userLikeService;
            _articleCommentService = articleCommentService;

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

        [HttpGet]
        public async Task<IActionResult> Read(int Id)
        {
            //Get article and increment view count
            var requestedArticle = _articleService.GetById(Id);
            requestedArticle.ViewCount++;
            _articleService.Update(requestedArticle);

            //Get comments of article
            var comments = _articleCommentService.GetCommentsByArticleId(Id).ToList();

            //Get logged in user
            var user = await _userManager.GetUserAsync(HttpContext.User);

            bool isLikedBefore = false;
            if (user != null)
            {
                string username = user.UserName;
                isLikedBefore = _userLikeService.CheckUserLikeExists(username, Id);
            }

            ReadArticleVM ravm = new ReadArticleVM { Article = requestedArticle, IsLikedBefore = isLikedBefore, Comments = comments };

            return View(ravm);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Read(int Id, ReadArticleVM comment)
        {
            //Get logged in user
            var user = await _userManager.GetUserAsync(HttpContext.User);
            string username = user.UserName;

            ArticleComment ac = new ArticleComment { CommentText = comment.PostedComment, ArticleId = Id, Username = username };

            _articleCommentService.Create(ac);

            return RedirectToAction("Read", new { Id = Id });
        }

        [Authorize]
        public async Task<JsonResult> Like(int Id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            string username = user.UserName;

            var requestedArticle = _articleService.GetById(Id);
            int likeCount = requestedArticle.LikeCount;

            bool result = false;
            if (!_userLikeService.CheckUserLikeExists(username, Id))
            {
                UserLike ul = new UserLike { Username = username, ArticleId = Id };
                requestedArticle.LikeCount++;
                if (_articleService.Update(requestedArticle))
                {
                    likeCount++;
                }

                result = _userLikeService.Create(ul);
            }

            return Json(new { result = result, likecount = likeCount });
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
