using System.Linq;

namespace PConsulting.Data
{
    public interface IGenericService<T> where T : class
    {
        IQueryable<T> GetAll();
        T GetById(int id);
        bool Create(T obj);
        bool Update(T obj);
        bool Delete(int id);
    }
}
