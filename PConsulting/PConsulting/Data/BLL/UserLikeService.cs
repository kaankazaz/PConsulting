using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PConsulting.Data
{
    public class UserLikeService : GenericService<UserLike>
    {
        public UserLikeService(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        //Check if user-article like exists
        public bool CheckUserLikeExists(string username, int articleId)
        {
            return GetAll().Any(x => x.Username == username && x.ArticleId == articleId);
        }


    }
}
