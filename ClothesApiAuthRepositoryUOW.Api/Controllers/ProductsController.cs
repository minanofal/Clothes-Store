using AutoMapper;
using ClothesApiAuthRepositoryUOW.Core.Consts;
using ClothesApiAuthRepositoryUOW.Core.Dtos;
using ClothesApiAuthRepositoryUOW.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClothesApiAuthRepositoryUOW.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IMapper _mapper;
        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitofwork = unitOfWork;
            _mapper = mapper;
        }


        ///GetMeThods
        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _unitofwork.Products.GetAllProducts());
        }

        [HttpGet("GetProduct/{Id}")]
        public async Task<IActionResult> GetAsync(int Id)
        {
            if (Id == null)
                return BadRequest("ID is Required");
            var product = await _unitofwork.Products.GetAllProducts(x => x.Id == Id);
            if (!product.Any())
                return NotFound($"There is no product with id {Id}");

            return Ok(product);
        }

        [HttpGet("GetCategory/{Id}")]
        public async Task<IActionResult> GetCategoryProduct(int Id)
        {
            var category = await _unitofwork.Categories.GetData(x => x.Id == Id);
            if (!category.Any())
                return NotFound($"Thers is No Category with ID {Id}");
            var products = await _unitofwork.Products.GetAllProducts(x => x.CategoryId == Id);
            if (!products.Any())
                return NotFound($"No Products in Category {category.FirstOrDefault().Name}");
            return Ok(products);
        }

        [HttpGet("Type/{Id}")]
        public async Task<IActionResult> GetAsyncProducts(int Id)
        {
            var data = await _unitofwork.Types.GetData(x => x.Id == Id);
            if (!data.Any())
                return NotFound($"Thers is No Category with ID {Id}");

            var products = await _unitofwork.Products.GetAllProducts(x => x.TypeId == Id);
            if (!products.Any())
                return NotFound($"No Product in Type {data.First().Name}");
            return Ok(products);
        }

        [HttpGet("Category/Type/{CId}/{TId}")]
        public async Task<IActionResult> GetCategoryTypeProduct(int CId, int TId)
        {
            var category = await _unitofwork.Categories.GetData(x => x.Id == CId);
            if (!category.Any())
                return NotFound($"Thers is No Category with ID {CId}");
            var type = await _unitofwork.Types.GetData(x => x.Id == TId);
            if (!category.Any())
                return NotFound($"Thers is No Type with ID {CId}");
            var products = await _unitofwork.Products.GetAllProducts(x => x.CategoryId == CId && x.TypeId == TId);
            if (!products.Any())
                return NotFound($"No Products in Category {category.FirstOrDefault().Name} And Type {type.FirstOrDefault().Name}");
            return Ok(products);
        }

        [HttpGet("Category/{gender}/{Id}")]
        public async Task<IActionResult> GetCategoryGenderProduct(int Id , string gender)
        {
            var category = await _unitofwork.Categories.GetData(x => x.Id == Id);
            if (!category.Any())
                return NotFound($"Thers is No Category with ID {Id}");
           

            if (!GenderConstant.GendersList.Contains(gender.ToUpper()))
                return NotFound($"Gender is Male Or Female (M,F)");

            char g = gender.ToUpper().ToCharArray()[0];

            var products = await _unitofwork.Products.GetAllProducts(x => x.CategoryId == Id && x.Gender == g);
            if (!products.Any())
                return NotFound($"No prodcuts  {category.First().Name} ");
            return Ok(products);
        }

        [HttpGet("Type/{gender}/{Id}")]
        public async Task<IActionResult> GetTypeGenderProduct(int Id, string gender)
        {
            var types = await _unitofwork.Types.GetData(x => x.Id == Id);
            if (!types.Any())
                return NotFound($"Thers is No types with ID {Id}");


            if (!GenderConstant.GendersList.Contains(gender.ToUpper()))
                return NotFound($"Gender is Male Or Female (M,F)");

            char g = gender.ToUpper().ToCharArray()[0];

            var products = await _unitofwork.Products.GetAllProducts(x => x.TypeId == Id && x.Gender == g);
            if (!products.Any())
                return NotFound($"No types  {types.First().Name} ");
            return Ok(products);
        }

        [HttpGet("Category/type/{gender}/{CId}/{TId}")]
        public async Task<IActionResult> GetCategoryGenderTypeProduct(int CId,int TId, string gender)
        {
            var category = await _unitofwork.Categories.GetData(x => x.Id == CId);
            if (!category.Any())
                return NotFound($"Thers is No Category with ID {CId}");

            var data = await _unitofwork.Types.GetData(x => x.Id == TId);
            if (!data.Any())
                return NotFound($"Thers is No Category with ID {TId}");
            if (!GenderConstant.GendersList.Contains(gender.ToUpper()))
                return NotFound($"Gender is Male Or Female (M,F)");

            char g = gender.ToUpper().ToCharArray()[0];

            var products = await _unitofwork.Products.GetAllProducts(x => x.CategoryId == CId && x.Gender == g && x.TypeId==TId);

            if (!products.Any())
                return NotFound($"No prodcuts  {category.First().Name} {data.First().Name} ");
            return Ok(products);
        }







        ///DeleteMehods
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteProduct/{Id}")]
        public async Task<IActionResult> DelteAsync(int Id)
        {
            if (Id == null)
                return BadRequest("ID is Required");
            var data = await _unitofwork.Products.DeleteProduct(Id);
            if (!data.Delete)
                return NotFound(data.Message);


            return Ok(data.Message);
        }

        ///EditMethods
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateProduct/{Id}")]
        public async Task<IActionResult> UpdateAsync([FromForm] EditeProductFormDto dto , int Id)
        {
            if (Id == null)
                return BadRequest();
            if (dto == null)
                return BadRequest("Data is required");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var data = await _unitofwork.Products.UpdateProduct(dto,Id);
            if (data.Message != null)
                return BadRequest(data.Message);
            return Ok(data);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateProductColorSize/{Id}")]
        public async Task<IActionResult> UpdateColorSize([FromBody] IEnumerable<Product_Color_Size> dto, int Id)
        {
            if (dto == null)
                return BadRequest("Null");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = await _unitofwork.Products.UpatePrduct_Size_color(dto, Id);
            if (data.Message != null)
                return BadRequest(data.Message);
            return Ok(data);
        }


        /////PostMehods
        [Authorize(Roles = "Admin")]
        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateAsync([FromForm] ProductFormDto dto)
        {
            if (dto == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cat = await _unitofwork.Products.CreateProduct(dto);
            if (cat.Message != null)
                return BadRequest(cat.Message);
            _unitofwork.Complete();
            return Ok(cat);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("AddProductColorSize")]
        public async Task<IActionResult> CreateColorSize([FromBody] Product_Size_Color_formDto dto)
        {
            if (dto == null)
                return BadRequest("Null");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = await _unitofwork.Products.CreatePrduct_Size_color(dto);
            if (data.Message != null)
                return BadRequest(data.Message);
            return Ok(data);
        }


    }
}
