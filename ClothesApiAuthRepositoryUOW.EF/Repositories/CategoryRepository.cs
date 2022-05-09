using ClothesApiAuthRepositoryUOW.Core.Dtos;
using ClothesApiAuthRepositoryUOW.Core.Interfaces;
using ClothesApiAuthRepositoryUOW.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.EF.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context; 
        }

        public async Task<IEnumerable<CategoryDisplayDto>> GetCategoryByGender(char gender)
        {
            var category = await _context.products
              .Include(x => x.Category)
              .Where(x => x.Gender == gender)
              .Select(x => new CategoryDisplayDto { Id = x.CategoryId, Name = x.Category.Name })
              .ToListAsync();
            return category;
        }
    }
}
