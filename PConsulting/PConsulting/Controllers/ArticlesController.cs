﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PConsulting.Data;
using System;
using System.Collections.Generic;
using System.Linq;
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

    }
}