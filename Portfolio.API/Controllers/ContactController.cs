using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Portfolio.API.DTOS;
using Portfolio.API.DTOS.Contact;
using Portfolio.API.DTOS.Designs;
using Portfolio.API.Helper;
using Portfolio.Core.Interfaces;
using Portfolio.Core.Models;

namespace Portfolio.API.Controllers
{
   
    public class ContactController : APIBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ContactController(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ContactToReturnDTO>>> GetAllMessages()
        {
            var data = await _unitOfWork.Repository<Contact>().GetAllAsync();

            var mappedData = _mapper.Map<IReadOnlyList<ContactToReturnDTO>>(data);

            var response = new PagedResponse<ContactToReturnDTO>
            {
                Data = mappedData,
                TotalCount = mappedData.Count
            };
            return Ok(response);
        }














        [HttpPost]
        public async Task<ActionResult> AddMessage(ContactDTO model)
        {
            var mappedData = _mapper.Map<Contact>(model);
            await _unitOfWork.Repository<Contact>().AddAsync(mappedData);
            var result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
                return Ok(new { status = 200, message = "Message sent successfully" });
            }
            return BadRequest(new { status = 400, message = "Failed to send message" });
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            try
            {
                var message = await _unitOfWork.Repository<Contact>().GetByIdAsync(id);
                if (message == null)
                {
                    return NotFound(new { message = "Message not found." });
                }
                _unitOfWork.Repository<Contact>().Delete(message);

                var result = await _unitOfWork.CompleteAsync();
                if (result > 0)
                {
                    return Ok(new { status = 200, message = "Message deleted successfully!" });
                }
                else
                {
                    return BadRequest(new { message = "Failed to delete the Message." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the Message.", error = ex.Message });
            }
        }



    }
}
