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
        private readonly IMapper _mapper;
        

        public ProductRepository(ApplicationDbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Product_color_sizeDisplayDto> CreatePrduct_Size_color(Product_Size_Color_formDto prduct_size)
        {
            if (!_context.products.Where(x => x.Id == prduct_size.ProductID).Any())
                return  new Product_color_sizeDisplayDto (){ Message = $"No Product With Id {prduct_size.ProductID}" };

            

            foreach(var color_size in prduct_size.ProductColorSizes.Distinct())
            {
                if (!SizesConstants.SizesList.Contains(color_size.Size.ToUpper())) 
                {
                    var sizes = "";
                    foreach (var size in SizesConstants.SizesList) { sizes = sizes + " " + size; }
                    return new Product_color_sizeDisplayDto() { Message = $"only sizes {sizes}" }; 
                
                }

                _context.SaveChanges();
                
                if (await _context.Colors.Where(x => x.ProductId == prduct_size.ProductID && x.color == color_size.Color).FirstOrDefaultAsync() == null) {
                    
                    _context.Colors.Add(new Color { ProductId = prduct_size.ProductID, color = color_size.Color }); }  
                

                if(! _context.sizes.Where(x=>x.ProductId==prduct_size.ProductID && x.size==color_size.Size).Any())
                    _context.sizes.Add(new Size { ProductId=prduct_size.ProductID, size=color_size.Size });


                if (!_context.product_Color_Sizes.Where(x => x.ProductId == prduct_size.ProductID && x.Size == color_size.Size && x.Color == color_size.Color).Any())
                    _context.product_Color_Sizes.Add(new Product_Color_Size_Dto { ProductId = prduct_size.ProductID, Color = color_size.Color, Size = color_size.Size });
            }
            _context.SaveChanges();

            var productscolorsizes = _context.product_Color_Sizes.Where(x => x.ProductId == prduct_size.ProductID).ToList();
            var  datacolorsize = _mapper.Map<IEnumerable<Product_Color_Size>>(productscolorsizes);
            var data = new Product_color_sizeDisplayDto() { product_Color_Sizes = datacolorsize};
           
            return data;
            

        }

        public async Task<ProductDisplayDto> CreateProduct(ProductFormDto dto)
        {
            var product = new ProductDisplayDto();
            if (_context.categories.Find(dto.CategoryId) == null)
            {
                product.Message = $"No Category With id {dto.CategoryId}";
                return product;
            }
            if (_context.types.Find(dto.TypeId)==null)
            {
                product.Message = $"No Category With id {dto.TypeId}";
                return product;
            }
            if (!GenderConstant.GendersList.Contains(dto.Gender.ToUpper()))
            {
                product.Message = $"Gender IS Male or Female (M OR F)";
                return product;
            }
            if (dto.Gender.ToUpper() == "MALE")
                dto.Gender = "M";
            if (dto.Gender.ToUpper() == "FEMALE")
                dto.Gender = "F";

            foreach (var _size in dto.Sizes.Distinct())
            {

                if (!SizesConstants.SizesList.Contains(_size.ToUpper()))
                {
                    var sizes = "";
                    foreach (var size in SizesConstants.SizesList) { sizes = sizes + " " + size; }
                    return new ProductDisplayDto() { Message = $"only sizes {sizes}" };


                }
            }


            var data = _mapper.Map<Product>(dto);
            var images = ImageToByteArray(dto.Images);

            
            if(images == null)
            {
                product.Message = ".jpg and .png images And max length is 1mg";
                return product;
            }
           await _context.AddAsync(data);
            _context.SaveChanges();

             AddImage(data.Id, images);
            if (_context.Type_Categories.FirstOrDefault(x=>x.TypeId ==dto.TypeId && x.CategoryId == dto.CategoryId) == null)
            {
                _context.Type_Categories.Add(new Type_Category { CategoryId = dto.CategoryId , TypeId=dto.TypeId });
            }

            foreach(var color in dto.Colors.Distinct())
            {
                if (_context.Colors.Where(x => x.ProductId == data.Id && x.color == color).FirstOrDefault() == null)
                {
                    _context.Colors.Add(new Color { color = color, ProductId = data.Id });
                    
                }
            }
            foreach (var _size in dto.Sizes.Distinct())
            {
                if (_context.sizes.Where(x => x.ProductId == data.Id && x.size == _size).FirstOrDefault() == null)
                {
                    _context.sizes.Add(new Size { size = _size, ProductId = data.Id });
                    
                }
            }

            _context.SaveChanges();
            product = _mapper.Map<ProductDisplayDto>(data);
            product.Colors = dto.Colors;
            product.sizes = dto.Sizes;
            
            product.Images = _context.productImages.Where(x => x.productId == product.Id).Select(x=>new ImageDisplayDto { Id=x.Id , Image = x.Image}).ToList();
            product.Message = null;
            return product;

        }

        public async Task<DeleteDisplay> DeleteProduct(int id)
        {
            
            if (! _context.products.Where(x=>x.Id==id).Any())
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
            var Product = await _context.products.FirstOrDefaultAsync(x => x.Id == id);
            var product = new ProductDisplayDto();
            if (Product == null)
            {
                product.Message = $"No Product with id {id}";
                return product;
            }
                

            if (_context.categories.Find(dto.CategoryId) == null)
            {
                product.Message = $"No Category With id {dto.CategoryId}";
                return product;
            }
            if (_context.types.Find(dto.TypeId) == null)
            {
                product.Message = $"No Category With id {dto.TypeId}";
                return product;
            }
            if (!GenderConstant.GendersList.Contains(dto.Gender.ToUpper()))
            {
                product.Message = $"Gender IS Male or Female (M OR F)";
                return product;
            }
            if (dto.Gender.ToUpper() == "MALE")
                dto.Gender = "M";
            if (dto.Gender.ToUpper() == "FEMALE")
                dto.Gender = "F";

            foreach (var _size in dto.Sizes.Distinct())
            {

                if (!SizesConstants.SizesList.Contains(_size.ToUpper()))
                {
                    var sizes = "";
                    foreach (var size in SizesConstants.SizesList) { sizes = sizes + " " + size; }
                    return new ProductDisplayDto() { Message = $"only sizes {sizes}" };


                }
            }
            Product.Price = dto.Price;
            Product.CategoryId = dto.CategoryId;
            Product.TypeId = dto.TypeId;
            Product.Name = dto.Name;
            Product.Gender = dto.Gender.ToCharArray()[0];
           
            if (dto.AddedImages != null)
            {
                var images = ImageToByteArray(dto.AddedImages);
                if (images == null)
                {
                    product.Message = ".jpg and .png images And max length is 1mg";
                    return product;
                }

                AddImage(id, images);
            }
            if (_context.Type_Categories.FirstOrDefault(x => x.TypeId == dto.TypeId && x.CategoryId == dto.CategoryId) == null)
            {
                _context.Type_Categories.Add(new Type_Category { CategoryId = dto.CategoryId, TypeId = dto.TypeId });
            }

            _context.Colors.RemoveRange(_context.Colors.Where(x => x.ProductId == id).ToList());
            _context.sizes.RemoveRange(_context.sizes.Where(x => x.ProductId == id).ToList());
            _context.SaveChanges();
            foreach (var color in dto.Colors.Distinct())
            {
                if (_context.Colors.Where(x => x.ProductId == id && x.color == color).FirstOrDefault() == null)
                {
                    _context.Colors.Add(new Color { color = color, ProductId =id });

                }
            }
            foreach (var _size in dto.Sizes.Distinct())
            {
                if (_context.sizes.Where(x => x.ProductId == id && x.size == _size).FirstOrDefault() == null)
                {
                    _context.sizes.Add(new Size { size = _size, ProductId = id });

                }
            }

            foreach(var img in dto.DeletedImages)
            {
                _context.productImages.Remove(_context.productImages.Find(img));
            }


            _context.SaveChanges();
            product = _mapper.Map<ProductDisplayDto>(Product);
            product.Colors = dto.Colors;
            product.sizes = dto.Sizes;

            product.Images = _context.productImages.Where(x => x.productId == product.Id).Select(x => new ImageDisplayDto { Id = x.Id, Image = x.Image }).ToList();
            product.Message = null;
            return product;

        }

        private List<byte[]> ImageToByteArray(IEnumerable<IFormFile> ImageFiles)
        {
            var images = new List<byte[]>();
            using var datastream = new MemoryStream();
            foreach (IFormFile file in ImageFiles)
            {
                if (!ImageConstants.ImageExtention.Contains(Path.GetExtension(file.FileName).ToLower()) || ImageConstants.MaxLength < file.Length)
                    return new List<byte[]>();
                file.CopyTo(datastream);
                images.Add(datastream.ToArray());
                
            }
            return images;
        }
        private void AddImage(int productID , List<byte[]> images)
        {
            foreach (var image in images)
            {

                _context.productImages.Add(new ProductImage { productId = productID  , Image = image });



            }
        }

        public async Task<Product_color_sizeDisplayDto> UpatePrduct_Size_color(IEnumerable< Product_Color_Size> prduct_size, int id)
        {
            if (!_context.products.Where(x=>x.Id == id).Any())
                return new Product_color_sizeDisplayDto() { Message = $"No product with Id {id}" };

            _context.product_Color_Sizes.RemoveRange(_context.product_Color_Sizes.Where(x => x.ProductId == id).ToList());
            var data = new Product_Size_Color_formDto() { ProductID = id, ProductColorSizes = prduct_size };

            return await CreatePrduct_Size_color(data) ;
        }

        public async Task<IEnumerable< CategoryDisplayDto>> GetCategoryGender( char gender)
        {
            var category = await _context.products
                .Include(x => x.Category)
                .Where(x =>  x.Gender == gender)
                .Select(x => new CategoryDisplayDto { Id = x.CategoryId, Name = x.Category.Name })
                .ToListAsync();
            return category;
                
        }
    }
}