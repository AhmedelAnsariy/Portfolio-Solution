using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Portfolio.API.DTOS;
using Portfolio.API.DTOS.Designs;
using Portfolio.Core.Interfaces;
using Portfolio.Core.Models;
using Portfolio.Core.Specifications.DesignSpec;

namespace Portfolio.API.Controllers
{

    public class DesignController : APIBaseController 
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DesignController(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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






    }
}
