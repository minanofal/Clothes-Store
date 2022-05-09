using AutoMapper;
using ClothesApiAuthRepositoryUOW.Core.Consts;
using ClothesApiAuthRepositoryUOW.Core.Dtos;
using ClothesApiAuthRepositoryUOW.Core.Interfaces;
using ClothesApiAuthRepositoryUOW.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.EF.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        private readonly IUseAllRepositoryEXProduct _useAllRepositoryEXProduct;
        private readonly IMapper _mapper;
        //private readonly IImageRepository _Images;
        //private readonly IBaseRepository<Type_Category> _Type_Category;
        //private readonly ICategoryRepository _Category;
        //private readonly IBaseRepository<Core.Models.Type> _Type;
        //private readonly IBaseRepository<Color> _color;
        //private readonly IBaseRepository<Size> _Size;
        //private readonly IBaseRepository<Product_Color_Size_Dto> _Poroduct_Color_sizes;

        //public ProductRepository(ApplicationDbContext context, IMapper mapper, IImageRepository unitOfWork, 
        //    IBaseRepository<Type_Category> Type_Category , ICategoryRepository Category,  IBaseRepository<Core.Models.Type> Type
        //    , IBaseRepository<Color> color , IBaseRepository<Size> size , IBaseRepository<Product_Color_Size_Dto> Poroduct_Color_sizes)
        //{
        //    _context = context;
        //    _mapper = mapper;
        //    _Images = unitOfWork;
        //    _Type_Category = Type_Category;
        //    _Type = Type;
        //    _Category = Category;
        //    _color = color;
        //    _Size = size;
        //    _Poroduct_Color_sizes = Poroduct_Color_sizes;
        //}

        public ProductRepository(ApplicationDbContext context, IMapper mapper, IUseAllRepositoryEXProduct useAllRepositoryEXProduct)
        {
            _context = context;
            _mapper = mapper;
            _useAllRepositoryEXProduct = useAllRepositoryEXProduct;
        }

        public async Task<Product_color_sizeDisplayDto> CreatePrduct_Size_color(Product_Size_Color_formDto prduct_size)
        {
            if (!_context.products.AsNoTracking().Where(x => x.Id == prduct_size.ProductID).Any())
                return  new Product_color_sizeDisplayDto (){ Message = $"No Product With Id {prduct_size.ProductID}" };

            var check = SizesConstants.CheckSize(prduct_size.ProductColorSizes.Select(x => x.Size).ToList());

            if (check != null)
                return new Product_color_sizeDisplayDto() { Message = check };
            

            foreach(var color_size in prduct_size.ProductColorSizes.Distinct())
            {
                
                
                if (!await _useAllRepositoryEXProduct.Color.Any(x => x.ProductId == prduct_size.ProductID && x.color == color_size.Color) ) {
                    
                   await _useAllRepositoryEXProduct.Color.CreateAsync(new Color { ProductId = prduct_size.ProductID, color = color_size.Color }); }  
                

                if(! await _useAllRepositoryEXProduct.Size.Any(x => x.ProductId == prduct_size.ProductID && x.size == color_size.Size))
                    await _useAllRepositoryEXProduct.Size.CreateAsync(new Size { ProductId=prduct_size.ProductID, size=color_size.Size });


                if (!await _useAllRepositoryEXProduct.Poroduct_Color_sizes.Any(x => x.ProductId == prduct_size.ProductID && x.Size == color_size.Size && x.Color == color_size.Color))
                    await _useAllRepositoryEXProduct.Poroduct_Color_sizes.CreateAsync(new Product_Color_Size_Dto { ProductId = prduct_size.ProductID, Color = color_size.Color, Size = color_size.Size });
            }
            _context.SaveChanges();

            var productscolorsizes =await _useAllRepositoryEXProduct.Poroduct_Color_sizes.GetData(x => x.ProductId == prduct_size.ProductID);
            var  datacolorsize = _mapper.Map<IEnumerable<Product_Color_Size>>(productscolorsizes);
            var data = new Product_color_sizeDisplayDto() { product_Color_Sizes = datacolorsize};
           
            return data;
            

        }

        public async Task<ProductDisplayDto> CreateProduct(ProductFormDto dto)
        {
             
            var product = await CheckValidProducts(dto.CategoryId, dto.TypeId, dto.Gender, dto.Sizes, dto.Images); ;
            if (product.Message != null)
                return product;
           
            dto.Gender = dto.Gender.ToUpper().ToArray()[0].ToString();
            var data = _mapper.Map<Product>(dto);
            await _context.AddAsync(data);
            _context.SaveChanges();

            var images = ImageConstants.ImageToByteArray(dto.Images);
            _useAllRepositoryEXProduct.Images.CreateProductsImages(images, data.Id);
            
            if (!await _useAllRepositoryEXProduct.Type_Category.Any(x => x.TypeId == dto.TypeId && x.CategoryId == dto.CategoryId))
            {
               await _useAllRepositoryEXProduct.Type_Category.CreateAsync(new Type_Category { CategoryId = dto.CategoryId , TypeId=dto.TypeId });
            }

            foreach(var color in dto.Colors.Distinct())
            {
                if (! await _useAllRepositoryEXProduct.Color.Any(x => x.ProductId == data.Id && x.color == color))
                {
                    await _useAllRepositoryEXProduct.Color.CreateAsync(new Color { color = color, ProductId = data.Id });
                    
                }
            }
            foreach (var _size in dto.Sizes.Distinct())
            {
                if( ! await _useAllRepositoryEXProduct.Size.Any(x => x.ProductId == data.Id && x.size == _size))
                {
                   await _useAllRepositoryEXProduct.Size.CreateAsync(new Size { size = _size, ProductId = data.Id });
                    
                }
            }


            _context.SaveChanges();
            product = _mapper.Map<ProductDisplayDto>(data);
           

            var Images = await _useAllRepositoryEXProduct.Images.GetData(x => x.productId == product.Id);
            var CategoryName = await _useAllRepositoryEXProduct.Categories.GetData(x => x.Id == data.CategoryId);
            var TypeName = await _useAllRepositoryEXProduct.Types.GetData(x => x.Id == data.TypeId);

            product.CategoryName =  CategoryName.SingleOrDefault().Name;
            product.TypeName = TypeName.SingleOrDefault().Name;
            product.Colors = dto.Colors;
            product.sizes = dto.Sizes;
            product.Images = _mapper.Map<IEnumerable<ImageDisplayDto>>(Images);
            product.Message = null;
            return product;

        }

        public async Task<DeleteDisplay> DeleteProduct(int id)
        {
            
            if (! _context.products.AsNoTracking().Where(x=>x.Id==id).Any())
                return new DeleteDisplay { Delete = false , Message =$"NO Product with Id {id}" };

            _context.products.Remove(_context.products.Where(x=>x.Id==id).FirstOrDefault());
            _context.SaveChanges();
            return new DeleteDisplay { Delete = true, Message = $"Product with Id {id} Deleted Successfully" };

        }

        public async Task<IEnumerable<ProductDisplayDto>> GetAllProducts(Expression<Func<ProductDisplayDto, bool>> match = null)
        {
            var products = _context.products
                    .Include(x => x.Category)
                    .Include(x => x.Type)
                    .Include(x => x.Images)
                    .Include(x => x.sizes)
                    .Include(x => x.colors)
                    .Include(x => x.Product_Color_Sizes)
                    .Select(x => new ProductDisplayDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Gender = x.Gender,
                        Price = x.Price,
                        CategoryId = x.CategoryId,
                        CategoryName = x.Category.Name,
                        TypeId = x.TypeId,
                        TypeName = x.Type.Name,
                        Colors = x.colors.Select(x => x.color).ToList(),
                        sizes = x.sizes.Select(x => x.size).ToList(),
                        Images = x.Images.Select(x => new ImageDisplayDto {Id =x.Id, Image=x.Image }).ToList(),
                        product_Color_Sizes = x.Product_Color_Sizes.Select(x => new Product_Color_Size { Color = x.Color, Size = x.Size }).ToList()


                    });

            if (match == null)
                return products.ToList();
            return products.Where(match).ToList();
        }

        public async Task<ProductDisplayDto> UpdateProduct(EditeProductFormDto dto, int id)
        {
            var Product = await _context.products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            var product = new ProductDisplayDto();
            if (Product == null)
            {
                product.Message = $"No Product with id {id}";
                return product;
            }

            product= await CheckValidProducts(dto.CategoryId, dto.TypeId, dto.Gender, dto.Sizes, dto.AddedImages);
            if (product.Message != null)
                return product;

           
            Product.Price = dto.Price;
            Product.CategoryId = dto.CategoryId;
            Product.TypeId = dto.TypeId;
            Product.Name = dto.Name;
            Product.Gender = dto.Gender.ToUpper().ToArray()[0] ;
           
            if (dto.AddedImages != null)
            {
                var images = ImageConstants.ImageToByteArray(dto.AddedImages);
                _useAllRepositoryEXProduct.Images.CreateProductsImages(images,id);
            }
            if (! await _useAllRepositoryEXProduct.Type_Category.Any(x => x.TypeId == dto.TypeId && x.CategoryId == dto.CategoryId))
            {
                await _useAllRepositoryEXProduct.Type_Category.CreateAsync(new Type_Category { CategoryId = dto.CategoryId, TypeId = dto.TypeId });
            }
            if (dto.DeletedImages != null)
            {
                foreach (var img in dto.DeletedImages)
                {
                   await _useAllRepositoryEXProduct.Images.DeleteAsync(img);
                }
            }
            _useAllRepositoryEXProduct.Color.DeleteAsyncmatch(x => x.ProductId == id);
            _useAllRepositoryEXProduct.Size.DeleteAsyncmatch(x => x.ProductId == id);
            _useAllRepositoryEXProduct.Poroduct_Color_sizes.DeleteAsyncmatch(x => x.ProductId == id);
            _context.SaveChanges();

            foreach (var color in dto.Colors.Distinct())
            {
                if (! await _useAllRepositoryEXProduct.Color.Any(x => x.ProductId == id && x.color == color))
                {
                    _useAllRepositoryEXProduct.Color.CreateAsync(new Color { color = color, ProductId =id });

                }
            }
            foreach (var _size in dto.Sizes.Distinct())
            {
                if (! await _useAllRepositoryEXProduct.Size.Any(x => x.ProductId == id && x.size == _size))
                {
                    await _useAllRepositoryEXProduct.Size.CreateAsync(new Size { size = _size, ProductId = id });

                }
            }
           


          


            product = _mapper.Map<ProductDisplayDto>(Product);
            var Images = await _useAllRepositoryEXProduct.Images.GetData(x => x.productId ==id);
            var CategoryName = await _useAllRepositoryEXProduct.Categories.GetData(x => x.Id == dto.CategoryId);
            var TypeName = await _useAllRepositoryEXProduct.Types.GetData(x => x.Id == dto.TypeId);

            product.CategoryName = CategoryName.SingleOrDefault().Name;
            product.TypeName = TypeName.SingleOrDefault().Name;
            product.Images = _mapper.Map<IEnumerable<ImageDisplayDto>>(Images);
            product.Colors = dto.Colors;
            product.sizes = dto.Sizes;
            product.Message = null;
            _context.SaveChanges();
            return product;

        }

        public async Task<Product_color_sizeDisplayDto> UpatePrduct_Size_color(IEnumerable< Product_Color_Size> prduct_size, int id)
        {
            if (!_context.products.AsNoTracking().Where(x=>x.Id == id).Any())
                return new Product_color_sizeDisplayDto() { Message = $"No product with Id {id}" };

            _context.product_Color_Sizes.RemoveRange(_context.product_Color_Sizes.Where(x => x.ProductId == id).ToList());
            var data = new Product_Size_Color_formDto() { ProductID = id, ProductColorSizes = prduct_size };

            return await CreatePrduct_Size_color(data) ;
        }
        private  async  Task<ProductDisplayDto> CheckValidProducts(int CategoryId , int TypeId , string Gender , IEnumerable<string> sizes , IEnumerable<IFormFile> ImageFiles)
        {
            var product = new ProductDisplayDto();
            if (! await _useAllRepositoryEXProduct.Categories.Any(x => x.Id == CategoryId))
            {
                product.Message = $"No Category With id {CategoryId} ";
            }
            if (! await _useAllRepositoryEXProduct.Types.Any(x => x.Id == TypeId))
            {
                product.Message += $"No Type With id {TypeId} ";  
            }
            product.Message = product.Message == null ? product.Message = GenderConstant.CheckGender(Gender) : product.Message += " " + GenderConstant.CheckGender(Gender);
            product.Message= product.Message == null ? product.Message =  SizesConstants.CheckSize(sizes) : product.Message += " "+ SizesConstants.CheckSize(sizes) ;
            if( ImageFiles !=null)
                product.Message = product.Message == null ?product.Message= ImageConstants.CheckImageValid(ImageFiles) : product.Message +=" "+ ImageConstants.CheckImageValid(ImageFiles);
            return product;
        }
    }
}