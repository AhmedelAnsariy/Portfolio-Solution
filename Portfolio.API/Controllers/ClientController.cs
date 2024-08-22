﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Portfolio.API.DTOS.Designs;
using Portfolio.API.DTOS;
using Portfolio.Core.Interfaces;
using Portfolio.Core.Models;
using Portfolio.API.DTOS.Client;
using Portfolio.API.Helper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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


        //[HttpPost]
        //public async Task<IActionResult> AddNewDesign( [FromForm] ClientToCreateDTO clientDto)
        //{
        //    try
        //    {
        //        // Save the image using the FileHelper class
        //        var imagePath = FileHelper.SaveFile(clientDto.PictureUrl, _environment.WebRootPath, "uploads");

        //        // Create a new Design object
        //        var client = new ClientReview
        //        {
        //            Name = clientDto.Name,
        //            Description = clientDto.Description,
        //            Company = clientDto.Company,
        //            PictureUrl = imagePath
        //        };

        //        //return Ok(new { message = "Design added successfully!" });

        //        _unitOfWork.Repository<ClientReview>().AddAsync(client);

        //        return Ok(client);

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}



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














    }
}