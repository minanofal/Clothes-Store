using AutoMapper;
using ClothesApiAuthRepositoryUOW.Core.Interfaces;
using ClothesApiAuthRepositoryUOW.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = ClothesApiAuthRepositoryUOW.Core.Models.Type;

namespace ClothesApiAuthRepositoryUOW.EF.Repositories
{
    public class UnitOFWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IBaseRepository<Category> Categories { get;  private set; }

        public IBaseRepository<Type> Types { get; private set; }

        public IProductRepository Products { get; private set; }

        

        public UnitOFWork(ApplicationDbContext context, IMapper _mapper)
        {
           
            _context = context;
            Products = new ProductRepository(_context,_mapper);
           Categories = new BaseRepository<Category>(_context);
            Types =new BaseRepository<Type>(_context);
            
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
