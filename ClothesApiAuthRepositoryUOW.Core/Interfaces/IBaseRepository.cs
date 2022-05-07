using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.Core.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);

        Task<IEnumerable<T>> GetData(Expression<Func<T, bool>> match = null, string[] Includes=null);
        Task<string> DeleteAsync(int id);

       

    }
}
