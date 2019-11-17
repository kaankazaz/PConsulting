using System.Linq;

namespace PConsulting.Data
{
    public class ArticleCommentService : GenericService<ArticleComment>
    {
        public ArticleCommentService(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        //Returns Comments by Article Id
        public IQueryable<ArticleComment> GetCommentsByArticleId(int Id)
        {
            return GetAll().Where(x => x.ArticleId == Id).OrderByDescending(x => x.CreateDate);
        }
    }
}
