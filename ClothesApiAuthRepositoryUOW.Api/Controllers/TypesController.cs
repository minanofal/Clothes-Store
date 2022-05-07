using AutoMapper;
using ClothesApiAuthRepositoryUOW.Core.Dtos;
using ClothesApiAuthRepositoryUOW.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClothesApiAuthRepositoryUOW.Core.Models;
using Microsoft.AspNetCore.Authorization;

namespace ClothesApiAuthRepositoryUOW.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TypesController : ControllerBase
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IMapper _mapper;
        public TypesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitofwork = unitOfWork;
            _mapper = mapper;
        }
       

        [HttpGet("GetAllTypes")]
        public async Task<IActionResult> GetAllAsync()
        {
            var data = await _unitofwork.Types.GetData();
            var types = _mapper.Map<IEnumerable<TypeDisplayDto>>(data);

            return Ok(types);
        }

        [HttpGet("GetType/{Id}")]
        public async Task<IActionResult> GetAsync(int Id)
        {
            var data = await _unitofwork.Types.GetData(x => x.Id == Id);
            if (!data.Any())
                return NotFound($"Thers is No Category with ID {Id}");
            var type = _mapper.Map<TypeDisplayDto>(data.FirstOrDefault());

            return Ok(type);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("CreateType")]
        public async Task<IActionResult> CreateAsync([FromBody] TypeFormDto Dto)
        {
            if (Dto == null)
                return BadRequest("some Thing Went Wrong");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var type = _mapper.Map<Core.Models.Type>(Dto);
            await _unitofwork.Types.CreateAsync(type);
            _unitofwork.Complete();
            return Ok(_mapper.Map<TypeFormDto>(type));

        }
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateType/{Id}")]
        public async Task<IActionResult> UpdateAsync([FromBody] TypeFormDto dto, int Id)
        {
            if (dto == null || Id == null)
                return BadRequest("Some Thing Went Wrong");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = await _unitofwork.Types.GetData(x => x.Id == Id);
            if (!data.Any())
                return NotFound($"There is No category with id {Id}");

            var type = data.FirstOrDefault();
            type.Name = dto.Name;
            await _unitofwork.Types.UpdateAsync(type);
            _unitofwork.Complete();

            return Ok(_mapper.Map<TypeFormDto>(type));

        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteType/{Id}")]
        public async Task<IActionResult> DeleteAsync(int Id)
        {
            if (Id == null)
                return BadRequest("Id is Required");
            var message = await _unitofwork.Types.DeleteAsync(Id);
            if (_unitofwork.Complete() == 0)
                return BadRequest(message);
            return Ok(message);
        }

    }
}
