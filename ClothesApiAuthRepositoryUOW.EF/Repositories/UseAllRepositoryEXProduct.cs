using ClothesApiAuthRepositoryUOW.Core.Interfaces;
using ClothesApiAuthRepositoryUOW.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.EF.Repositories
{
    public class UseAllRepositoryEXProduct : IUseAllRepositoryEXProduct
    {
        private readonly ApplicationDbContext _context;
        public ICategoryRepository Categories { get; private set; }
        public IImageRepository Images { get; private set; }
        public IBaseRepository<Type_Category> Type_Category { get; private set; }
        public IBaseRepository<Product_Color_Size_Dto> Poroduct_Color_sizes { get; private set; }
        public IBaseRepository<Color> Color { get; private set; }
        public IBaseRepository<Size> Size { get; private set; }
        public IBaseRepository<Core.Models.Type> Types { get; private set; }

        public UseAllRepositoryEXProduct(ApplicationDbContext context)
        {

            _context = context;
            Poroduct_Color_sizes = new BaseRepository<Product_Color_Size_Dto>(_context);
            Categories = new CategoryRepository(_context);
            Images = new ImageRepository(_context);
            Type_Category = new BaseRepository<Type_Category>(_context);
            Types = new BaseRepository<Core.Models.Type>(_context);
            Color = new BaseRepository<Color>(_context);
            Size = new BaseRepository<Size>(_context);
          

        }
    }
}
