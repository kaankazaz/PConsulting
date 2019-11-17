using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PConsulting.Data
{
    public class ArticleComment
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int ArticleId { get; set; }
        public string CommentText { get; set; } = "";
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
