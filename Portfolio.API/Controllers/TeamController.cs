using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Portfolio.API.DTOS.Client;
using Portfolio.API.DTOS;
using Portfolio.Core.Interfaces;
using Portfolio.Core.Models;
using Portfolio.Repository.Repositories;
using Portfolio.API.DTOS.Team;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Portfolio.API.Errors;
using Portfolio.API.Helper;

namespace Portfolio.API.Controllers
{



    public class TeamController : APIBaseController
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;


        public TeamController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _environment = environment;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<TeamMember>>> GetAllClient()
        {
            var data = await _unitOfWork.Repository<TeamMember>().GetAllAsync();

            var mappedData = _mapper.Map<IReadOnlyList<TeamMember>, IReadOnlyList<TeamToReturnDTO>>(data);

            var response = new PagedResponse<TeamToReturnDTO>
            {
                Data = mappedData,
                TotalCount = mappedData.Count
            };
            return Ok(response);
        }




        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> AddNewTeam([FromForm] TeamToCreateDTO teamCreateDto)
        {
            try
            {
                string folderName = Path.Combine("images", "Team");
                var imagePath = FileHelper.SaveFile(teamCreateDto.PictureUrl, _environment.WebRootPath, folderName);




                var oneteam = _mapper.Map<TeamMember>(teamCreateDto);


                oneteam.PictureUrl = imagePath;
                await _unitOfWork.Repository<TeamMember>().AddAsync(oneteam);
                var result = await _unitOfWork.CompleteAsync();
                if (result > 0)
                {
                    return Ok(new { status = 200, message = "Team added successfully!", oneteam });
                }
                else
                {
                    return BadRequest(new { message = "Failed to add the Team." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }



        }




        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTeamMember(int id)
        {


            var oneMember = await _unitOfWork.Repository<TeamMember>().GetByIdAsync(id);


            if (oneMember == null)
            {
                return NotFound(new ApiResponse(404, "There is no Member  with this ID"));
            }

            _unitOfWork.Repository<TeamMember>().Delete(oneMember);

            var result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
                return Ok(new { status = 200, message = "Team Member  successfully deleted from your system" });
            }
            else
            {
                return StatusCode(500, "An error occurred while deleting the Member");
            }



        }


    }



}
