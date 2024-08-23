using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Portfolio.API.DTOS;
using Portfolio.API.DTOS.Career;
using Portfolio.API.DTOS.Contact;
using Portfolio.Core.Interfaces;
using Portfolio.Core.Models;

namespace Portfolio.API.Controllers
{
    
    public class CareerController : APIBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CareerController(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CareerToReturnDTO>>> GetAllCareers()
        {
            var data = await _unitOfWork.Repository<Career>().GetAllAsync();
            var mappedData = _mapper.Map<IReadOnlyList<CareerToReturnDTO>>(data);
            var response = new PagedResponse<CareerToReturnDTO>
            {
                Data = mappedData,
                TotalCount = mappedData.Count
            };
            return Ok(response);
        }




        [HttpPost]
        public async Task<ActionResult> AddCareer(CareerDTO model)
        {
            var mappedData = _mapper.Map<Career>(model);

            await _unitOfWork.Repository<Career>().AddAsync(mappedData);

            var result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
                return Ok(new { status = 200, message = "Career Added successfully" });
            }
            return BadRequest(new { status = 400, message = "Failed to Add  Career" });
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCareer(int id)
        {
            try
            {
                var career = await _unitOfWork.Repository<Career>().GetByIdAsync(id);
                if (career == null)
                {
                    return NotFound(new { message = "Message not found." });
                }
                _unitOfWork.Repository<Career>().Delete(career);

                var result = await _unitOfWork.CompleteAsync();
                if (result > 0)
                {
                    return Ok(new { status = 200, message = "Career deleted successfully!" });
                }
                else
                {
                    return BadRequest(new { message = "Failed to delete the Career." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the Career.", error = ex.Message });
            }
        }

    }
}
