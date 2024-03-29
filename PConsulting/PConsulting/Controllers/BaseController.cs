﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PConsulting.Controllers
{
    public class BaseController : Controller
    {
        protected virtual IQueryable SearchItems(IQueryable items, String search, List<String> columnNames)
        {
            // Apply filtering to all visible column names
            if (search != null && search.Length > 0)
            {
                StringBuilder sb = new StringBuilder();

                // create dynamic Linq expression
                foreach (String fieldName in columnNames)
                {
                    sb.AppendFormat("({0} == null ? false : {0}.ToString().IndexOf(@0, @1) >=0) or {1}", fieldName, Environment.NewLine);
                }

                String searchExpression = sb.ToString();
                // remove last "or" occurrence
                searchExpression = searchExpression.Substring(0, searchExpression.LastIndexOf("or"));

                // Apply filtering, 
                items = items.Where(searchExpression, search, StringComparison.OrdinalIgnoreCase);
            }

            return items;
        }

        protected String ItemsToJson(IQueryable items, List<String> columnNames, String sort, String order, Int32 limit, Int32 offset)
        {
            try
            {
                // where clause is set, count total records
                Int32 count = items.Count();

                // Skip requires sorting, so make sure there is always sorting
                String sortExpression = "";

                if (sort != null && sort.Length > 0)
                {
                    sortExpression += String.Format("{0} {1}", sort, order);
                }

                // show all records if limit is not set
                if (limit == 0)
                {
                    limit = count;
                }

                // Prepare json structure
                var result = new
                {
                    total = count,
                    rows = items.OrderBy(sortExpression).Skip(offset).Take(limit).Select("new (" + String.Join(",", columnNames) + ")")
                };

                return JsonConvert.SerializeObject(result, Formatting.None, new JsonSerializerSettings() { MetadataPropertyHandling = MetadataPropertyHandling.Ignore });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}