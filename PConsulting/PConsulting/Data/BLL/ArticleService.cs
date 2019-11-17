using System;
using System.Linq;

namespace PConsulting.Data
{
    public class ArticleService : GenericService<Article>
    {
        public ArticleService(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        //Returns articles by approval state
        public IQueryable<Article> GetByApprovalState(bool isApproved)
        {
            return GetAll().Where(x => x.IsApproved == isApproved).OrderByDescending(x => x.PublishDate);
        }

        //Returns articles by approval state and limit
        public IQueryable<Article> GetByApprovalState(bool isApproved, int limit)
        {
            return GetByApprovalState(isApproved).Take(limit);
        }

        //Returns most liked articles by limit
        public IQueryable<Article> GetByLikes(int limit)
        {
            return GetByApprovalState(true).OrderByDescending(x => x.LikeCount).Take(limit);
        }

        //Set approval state of the article
        public bool SetApproval(int id, bool isApproved)
        {
            bool isSuccessful = false;
            var article = GetById(id);

            if (article != null)
            {
                article.IsApproved = isApproved;
                article.LastEditDate = DateTime.Now;

                isSuccessful = Update(article);
            }

            return isSuccessful;
        }
    }
}
