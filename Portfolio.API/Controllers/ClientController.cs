using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Portfolio.API.DTOS.Designs;
using Portfolio.API.DTOS;
using Portfolio.Core.Interfaces;
using Portfolio.Core.Models;
using Portfolio.API.DTOS.Client;
using Portfolio.API.Helper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Portfolio.API.Errors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Portfolio.API.Controllers
{
    
    public class ClientController :  APIBaseController
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;


        public ClientController(IUnitOfWork unitOfWork , IMapper mapper , IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _environment = environment;
        }





        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ClientReview>>> GetAllClient()
        {
            var data = await _unitOfWork.Repository<ClientReview>().GetAllAsync();
            var mappedData = _mapper.Map<IReadOnlyList<ClientReview>, IReadOnlyList<ClientToReturnDTO>>(data);
            var response = new PagedResponse<ClientToReturnDTO>
            {
                Data = mappedData,
                TotalCount = mappedData.Count
            };
            return Ok(response);
        }







        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> AddNewDesign([FromForm] ClientToCreateDTO clientDto)
        {
            try
            {
                string folderName = Path.Combine("images", "clients");
                var imagePath = FileHelper.SaveFile(clientDto.PictureUrl, _environment.WebRootPath, folderName);


                var client = _mapper.Map<ClientReview>(clientDto);


                client.PictureUrl = imagePath;
                await _unitOfWork.Repository<ClientReview>().AddAsync(client);
                var result = await _unitOfWork.CompleteAsync();
                if (result > 0)
                {
                    return Ok(new { status = 200 , message = "Client added successfully!", client });
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
        public async Task<ActionResult> DeleteClient(int id)
        {


            var oneClient = await _unitOfWork.Repository<ClientReview>().GetByIdAsync(id);  


            if (oneClient == null)
            {
                return NotFound(new ApiResponse(404, "There is no client with this ID"));
            }

            //string imageFileName = Path.GetFileName(oneClient.PictureUrl); 
            //var imagePath = Path.Combine("wwwroot/images/clients", imageFileName);
            //var fileHelper = new FileHelper();
            //var isDeleted = fileHelper.DeleteDocument(imagePath);

            //if (!isDeleted)
            //{
            //    return StatusCode(500, "An error occurred while deleting the client's image. Please check the server logs for details.");
            //}


            _unitOfWork.Repository<ClientReview>().Delete(oneClient);
            var result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
                return Ok(new { status = 200, message = "Client and image successfully deleted from your system" });
            }
            else
            {
                return StatusCode(500, "An error occurred while deleting the client");
            }



        }




    }
}
