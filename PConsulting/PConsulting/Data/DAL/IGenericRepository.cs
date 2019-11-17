using System;
using System.Linq;
using System.Linq.Expressions;

namespace PConsulting.Data
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T GetById(int id);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        bool Create(T obj);
        bool Update(T obj);
        bool Delete(int id);
    }
}
