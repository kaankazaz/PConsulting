using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace PConsulting.Data
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        protected IGenericRepository<T> _db;

        public GenericService(ApplicationDbContext _context)
        {
            _db = new GenericRepository<T>(_context);
        }

        public bool Create(T obj)
        {
            try
            {
                return _db.Create(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(int id)
        {
            return _db.Delete(id);
        }

        public IQueryable<T> GetAll()
        {
            var results = _db.GetAll().AsNoTracking();
            return results;
        }

        public T GetById(int id)
        {
            var result = _db.GetById(id);
            return result;
        }

        public bool Update(T obj)
        {
            return _db.Update(obj);
        }
    }
}
