using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PConsulting.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Create(T obj)
        {
            try
            {
                _context.Add(obj);
                _context.SaveChanges();

                _context.Entry<T>(obj).State = EntityState.Detached;
                return true;
            }
            catch (System.Exception ex)
            {
                //LOG HERE - Kaan KAZAZ
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var obj = _context.Set<T>().Find(id);
                _context.Remove<T>(obj);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception ex)
            {
                //LOG HERE - Kaan KAZAZ
                return false;
            }
        }

        public IQueryable<T> GetAll()
        {
            DbSet<T> set = _context.Set<T>();
            return set;
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var query = GetAll().Where(predicate);
            return includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public T GetById(int id)
        {
            var obj = _context.Set<T>().Find(id);
            return obj;
        }

        public bool Update(T obj)
        {
            try
            {
                var changedEntriesCopy = _context.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added ||
                                e.State == EntityState.Modified ||
                                e.State == EntityState.Deleted)
                    .ToList();

                foreach (var _entry in changedEntriesCopy)
                {
                    _entry.State = EntityState.Detached;
                }

                _context.SaveChanges();

                _context.Update<T>(obj);

                _context.SaveChanges();
                _context.Entry<T>(obj).State = EntityState.Detached;
                return true;
            }
            catch (System.Exception ex)
            {
                //LOG HERE - Kaan KAZAZ
                return false;
            }
        }
    }
}
