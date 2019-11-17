using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PConsulting.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;

namespace PConsulting.Controllers
{
    [Authorize(Roles = "Publisher")]
    public class ArticlesController : BaseController //Inherits from BaseController - Kaan KAZAZ
    {
        private ArticleService _articleService;

        public ArticlesController(ArticleService articleService)
        {
            _articleService = articleService;
        }

        #region Index - List
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public virtual ActionResult LoadArticles(String sort, String order, String search, Int32 limit, Int32 offset, String ExtraParam)
        {
            // Get entity fieldnames
            List<String> columnNames = typeof(Article).GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(p => p.Name).ToList();

            // Create a seperate list for searchable field names   
            List<String> searchFields = new List<String>(columnNames);

            // Exclude field Id for filtering 
            searchFields.Remove("Id");

            // Perform filtering
            //IQueryable items = SearchItems(countries.AsQueryable(), search, searchFields);
            IQueryable items = SearchItems(articles.AsQueryable(), search, searchFields);

            // Sort the filtered items and apply paging
            return Content(ItemsToJson(items, columnNames, sort, order, limit, offset), "application/json");
        }

        private IList<Article> articles
        {
            get
            {
                IList<Article> result = new List<Article>();

                var articles = _articleService.GetByApprovalState(true);

                if (articles != null)
                {
                    result = articles.ToList();
                }

                return result;
            }
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Article postedObject)
        {
            if (_articleService.Create(postedObject))
            {
                return RedirectToAction("Index");
            }

            return View(postedObject);


        }
        #endregion

        #region Update
        [HttpGet]
        public IActionResult Update(int Id)
        {
            var requestedObject = _articleService.GetById(Id);

            return View(requestedObject);
        }

        [HttpPost]
        public IActionResult Update(Article postedObject)
        {
            bool isSuccessful = false;

            if (postedObject.Id == 0)
            {
                isSuccessful = _articleService.Create(postedObject);
            }
            else
            {
                var originalArticle = _articleService.GetById(postedObject.Id);

                originalArticle.ArticleTitle = postedObject.ArticleTitle;
                originalArticle.Body = postedObject.Body;
                originalArticle.Author = postedObject.Author;
                originalArticle.LastEditDate = DateTime.Now;

                isSuccessful = _articleService.Update(originalArticle);
            }

            if (isSuccessful)
            {
                return RedirectToAction("Index");
            }

            return View(postedObject);
        }
        #endregion

        #region Delete
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            try
            {
                var objectToRemove = _articleService.GetById(Id);

                if (objectToRemove == null)
                {
                    return StatusCode(Convert.ToInt32(HttpStatusCode.NotFound));
                }

                return PartialView("_Delete", objectToRemove);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public IActionResult Delete(Article postedObject)
        {
            if (_articleService.SetApproval(postedObject.Id, false))
            {
                return Json(new { success = true, message = "Article removed successfully." });
            }
            else
            {
                return Json(new { success = false, message = "Error occured while removing article." });
            }
        }
        #endregion
    }
}