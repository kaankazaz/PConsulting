using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PConsulting.Data
{
    public class Article
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Article Title")]
        public string ArticleTitle { get; set; } = "";

        [Required]
        public string Body { get; set; } = "";

        [Required]
        public string Author { get; set; } = "";

        public DateTime PublishDate { get; set; } = DateTime.Now;
        public DateTime LastEditDate { get; set; } = DateTime.Now;
        public int ViewCount { get; set; } = 0;
        public int LikeCount { get; set; } = 0;
        public bool IsApproved { get; set; } = true;
    }
}
