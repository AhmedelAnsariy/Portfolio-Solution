using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Portfolio.API.DTOS;
using Portfolio.API.DTOS.Designs;
using Portfolio.API.Errors;
using Portfolio.API.Helper;
using Portfolio.Core.Interfaces;
using Portfolio.Core.Models;
using Portfolio.Core.Specifications.DesignSpec;

namespace Portfolio.API.Controllers
{

    public class DesignController : APIBaseController 
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;


        public DesignController(IUnitOfWork unitOfWork , IMapper mapper , IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _environment = environment;
        }


        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Design>>> GetAllDesing()
        {

            var spec = new DesignWithCategorySpec();


            var data = await _unitOfWork.Repository<Design>().GetAllWithSpecAsync(spec);

            var mappedData = _mapper.Map<IReadOnlyList<Design>, IReadOnlyList<DesignToReturnDto>>(data);


            var response = new PagedResponse<DesignToReturnDto>
            {
                Data = mappedData,
                TotalCount = mappedData.Count
            };
            return Ok(response);



        }



        [HttpGet("{id}")]
        public async Task<ActionResult<Design> > GetDesignById(int id )
        {

            var spec = new DesignWithCategorySpec(id);


            var data = await _unitOfWork.Repository<Design>().GetWithSpecByIdAsync(spec);

            if(data is null)
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Design Not Found"));
            }


            var mappedData = _mapper.Map<Design, DesignToReturnDto>(data);


            return Ok(mappedData);



        }






        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResult> AddNewDesign([FromForm] DesignToAddDTO model)
        {
            try
            {
                string folderName = Path.Combine("images", "Designs");
                var imagePath = FileHelper.SaveFile(model.PictureUrl, _environment.WebRootPath, folderName);


                var design = _mapper.Map<Design>(model);



                design.PictureUrl = imagePath;
                await _unitOfWork.Repository<Design>().AddAsync(design);
                var result = await _unitOfWork.CompleteAsync();
                if (result > 0)
                {
                    return Ok(new { status = 200, message = "Design added successfully!" });
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
        public async Task<ActionResult> DeleteDesign (int id)
        {
            try
            {
                var design = await _unitOfWork.Repository<Design>().GetByIdAsync(id);
                if (design == null)
                {
                    return NotFound(new { message = "Design not found." });
                }

                string imageFileName = Path.GetFileName(design.PictureUrl);
                var imagePath = Path.Combine("wwwroot/images/Designs", imageFileName);
                var fileHelper = new FileHelper();
                var isDeleted = fileHelper.DeleteDocument(imagePath);

                if (!isDeleted)
                {
                    return StatusCode(500, "An error occurred while deleting the client's image. Please check the server logs for details.");
                }


                _unitOfWork.Repository<Design>().Delete(design);
                var result = await _unitOfWork.CompleteAsync();
                if (result > 0)
                {
                    return Ok(new { status = 200, message = "Design deleted successfully!" });
                }
                else
                {
                    return BadRequest(new { message = "Failed to delete the design." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the design.", error = ex.Message });
            }
        }

    }
}
