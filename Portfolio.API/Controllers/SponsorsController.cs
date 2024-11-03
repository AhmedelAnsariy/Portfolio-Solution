using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Portfolio.API.DTOS.Client;
using Portfolio.API.DTOS;
using Portfolio.Core.Interfaces;
using Portfolio.Core.Models;
using Portfolio.Repository.Repositories;
using System;
using Portfolio.API.DTOS.Sponsor;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Portfolio.API.Helper;
using Portfolio.API.Errors;

namespace Portfolio.API.Controllers
{
   
    public class SponsorsController : APIBaseController
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public SponsorsController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _environment = environment;
        }



        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Sponspr>>> GetAllSponsors()
        {
            var data = await _unitOfWork.Repository<Sponspr>().GetAllAsync();


            var mappedData = _mapper.Map<IReadOnlyList<Sponspr>, IReadOnlyList<SponsorToReturnDTO>>(data);
            var response = new PagedResponse<SponsorToReturnDTO>
            {
                Data = mappedData,
                TotalCount = mappedData.Count
            };
            return Ok(response);
        }



        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> AddNewSponsor([FromForm] SponsorToCreateDTO sponsorDto)
        {
            try
            {
                string folderName = Path.Combine("images", "Sponsors");
                var imagePath = FileHelper.SaveFile(sponsorDto.PictureUrl, _environment.WebRootPath, folderName);


                var sponsor = _mapper.Map<Sponspr>(sponsorDto);


                sponsor.PictureUrl = imagePath;
                await _unitOfWork.Repository<Sponspr>().AddAsync(sponsor);
                var result = await _unitOfWork.CompleteAsync();
                if (result > 0)
                {
                    return Ok(new { status = 200, message = "Sponsor added successfully!", sponsor });
                }
                else
                {
                    return BadRequest(new { message = "Failed to add the Sponsor." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSponsor(int id)
        {


            var oneSponsor = await _unitOfWork.Repository<Sponspr>().GetByIdAsync(id);


            if (oneSponsor == null)
            {
                return NotFound(new ApiResponse(404, "There is no Sponspr with this ID"));
            }



            //string imageFileName = Path.GetFileName(oneSponsor.PictureUrl); 
            //var imagePath = Path.Combine("wwwroot/images/Sponsors", imageFileName);
            //var fileHelper = new FileHelper();
            //var isDeleted = fileHelper.DeleteDocument(imagePath);

            //if (!isDeleted)
            //{
            //    return StatusCode(500, "An error occurred while deleting the client's image. Please check the server logs for details.");
            //}


            _unitOfWork.Repository<Sponspr>().Delete(oneSponsor);
            var result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
                return Ok(new { status = 200, message = "Sponsor successfully deleted from your system" });
            }
            else
            {
                return StatusCode(500, "An error occurred while deleting the Sponsor");
            }



        }


    }
}
