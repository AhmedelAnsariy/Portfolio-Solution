using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Portfolio.API.DTOS.Client;
using Portfolio.API.DTOS;
using Portfolio.Core.Interfaces;
using Portfolio.Core.Models;
using Portfolio.Repository.Repositories;
using Portfolio.API.DTOS.Team;

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




    }



}
