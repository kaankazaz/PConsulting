using PConsulting.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PConsulting.ViewModels
{
    public class HomePageVM
    {
        public List<Article> LatestArticles { get; set; }
        public List<Article> MostLikedArticles { get; set; }
    }
}
