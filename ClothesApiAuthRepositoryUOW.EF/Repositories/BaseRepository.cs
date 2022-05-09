using ClothesApiAuthRepositoryUOW.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.EF.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Any(Expression<Func<T, bool>> match)
        {
            return _context.Set<T>().Any(match);
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _context.AddAsync(entity);
            _context.SaveChanges();
            return entity;
        }
        public async Task<string> DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
                return $"No Item With Id{id}";
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
            return "Item Delted Successfully";
            
        }

        public void DeleteAsyncmatch(Expression<Func<T, bool>> match)
        {
            var entity = _context.Set<T>().Where(match).ToList();
            _context.RemoveRange(entity);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<T>> GetData(Expression<Func<T, bool>> match = null,string[] Includes= null)
        {
            IQueryable<T> query =  _context.Set<T>();

            if (match == null&&Includes==null)
                return   query.ToList();
            if (match != null && Includes == null)
                return query.Where(match).ToList();
            if (match != null && Includes != null)
                foreach (var include in Includes)
                    query = query.Include(include);
            return query.ToList();
                




        }

        public async Task<T> UpdateAsync(T entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}
