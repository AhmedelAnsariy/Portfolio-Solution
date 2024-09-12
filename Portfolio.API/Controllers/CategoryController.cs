using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Portfolio.API.DTOS;
using Portfolio.API.DTOS.Category;
using Portfolio.API.DTOS.Designs;
using Portfolio.API.Errors;
using Portfolio.API.Helper;
using Portfolio.Core.Interfaces;
using Portfolio.Core.Models;

namespace Portfolio.API.Controllers
{
   
    public class CategoryController : APIBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryController(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }




        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CategoryToReturnDto>>> GetAllCategories()
        {
            var data = await _unitOfWork.Repository<Category>().GetAllAsync();



            var mappedData = _mapper.Map<IReadOnlyList<Category>, IReadOnlyList<CategoryToReturnDto>>(data);


            var response = new PagedResponse<CategoryToReturnDto>
            {
                Data = mappedData,
                TotalCount = mappedData.Count
            };
            return Ok(response);
            

        }


        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResult> AddNewCategory(CategoryToAddDTO model)
        {
            try
            {
                var category = _mapper.Map<Category>(model);
                await _unitOfWork.Repository<Category>().AddAsync(category);
                var result = await _unitOfWork.CompleteAsync();
                if (result > 0)
                {
                    return Ok(new { status = 200, message = "Category added successfully!" });
                }
                else
                {
                    return BadRequest(new { message = "Failed to add the client." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOneCategory(int id)
        {
            var oneCategory = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
            if (oneCategory == null)
            {
                return NotFound(new ApiResponse(404, "There is no Category with this ID"));
            }
            _unitOfWork.Repository<Category>().Delete(oneCategory);
            var result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
                return Ok(new { status = 200, message = "Category successfully deleted from your system" });
            }
            else
            {
                return StatusCode(500, "An error occurred while deleting the client");
            }
        }




    }
}
