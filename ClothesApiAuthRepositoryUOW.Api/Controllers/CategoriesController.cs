using AutoMapper;
using ClothesApiAuthRepositoryUOW.Core.Consts;
using ClothesApiAuthRepositoryUOW.Core.Dtos;
using ClothesApiAuthRepositoryUOW.Core.Interfaces;
using ClothesApiAuthRepositoryUOW.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClothesApiAuthRepositoryUOW.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IMapper _mapper;
        public CategoriesController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitofwork = unitOfWork;
            _mapper = mapper;
        }
       

        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllAsync()
        {
            var data = await _unitofwork.Categories.GetData();
            var categories = _mapper.Map<IEnumerable<CategoryDisplayDto>>(data);

            return Ok(categories);
        }

        [HttpGet("GetCategory/{Id}")]
        public async Task<IActionResult> GetAsync(int Id)
        {
            var data = await _unitofwork.Categories.GetData(x=>x.Id==Id);
            if (!data.Any())
                return NotFound($"Thers is No Category with ID {Id}");
            var category = _mapper.Map<CategoryDisplayDto>(data.FirstOrDefault());

            return Ok(category);
        }
        [HttpGet("GetCategoryTypes/{Id}")]
        public async Task<IActionResult> GetTypesAsync(int Id)
        {
            var data = await _unitofwork.Categories.GetData(x => x.Id == Id, new[] { "Types" });
            if (!data.Any())
                return NotFound($"Thers is No Category with ID {Id}");
            var category = _mapper.Map<CatigoryTypesDisplayDto>(data.FirstOrDefault());

            return Ok(category);
        }
        [HttpGet("Category/{gender}")]
        public async Task<IActionResult> CategoryGender(  string gender)
        {
          
       
            if (!GenderConstant.GendersList.Contains(gender.ToUpper()))
                return NotFound($"Gender is Male Or Female (M,F)");

            char g = gender.ToUpper().ToCharArray()[0];

            var categories = await _unitofwork.Categories.GetCategoryByGender(g);
            if (!categories.Any())
                return NotFound("No categories in this Gender");
            return Ok(categories);

        }

        ///Admin
        [Authorize(Roles = "Admin")]
        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateAsync([FromBody] CategoryFormDto dto)
        {
            if (dto == null)
                return BadRequest("some Thing Went Wrong");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var category = _mapper.Map<Category>(dto);
            await _unitofwork.Categories.CreateAsync(category);
            _unitofwork.Complete();
            return Ok(_mapper.Map<CategoryFormDto>(dto));

        }
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateCategory/{Id}")]
        public async Task<IActionResult> UpdateAsync([FromBody] CategoryFormDto dto, int Id)
        {
            if (dto == null || Id == null)
                return BadRequest("Some Thing Went Wrong");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = await _unitofwork.Categories.GetData(x => x.Id == Id);
            if (!data.Any())
                return NotFound($"There is No category with id {Id}");

            var category = data.FirstOrDefault();
            category.Name = dto.Name;
            await _unitofwork.Categories.UpdateAsync(category);
            _unitofwork.Complete();

            return Ok(_mapper.Map<CategoryFormDto>(category));

        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteCategory/{Id}")]
        public async Task<IActionResult> DeleteAsync(int Id)
        {
            if (Id == null)
                return BadRequest("Id is Required");
            var message = await _unitofwork.Categories.DeleteAsync(Id);
            if (_unitofwork.Complete() == 0)
                return BadRequest(message);
            return Ok(message);
        }

    }
}
