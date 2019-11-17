using PConsulting.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PConsulting.ViewModels
{
    public class ReadArticleVM
    {
        public Article Article { get; set; }
        public bool IsLikedBefore { get; set; }
        public List<ArticleComment> Comments { get; set; }

        [Required]
        [Display(Name = "Comment")]
        public string PostedComment { get; set; }
    }
}
