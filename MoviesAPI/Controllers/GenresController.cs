﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Dtos;
using MoviesAPI.services;
using System;


namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenresService _genresService;
        public GenresController(IGenresService genresService)
        {
       
            _genresService = genresService;
        }
        [HttpGet]
        public async Task <IActionResult>GetAllAsync()
        {
            var genres = await _genresService.GetALL();
            return Ok(genres);
        }
        [HttpPost]
        public async Task<IActionResult> createAsync(CreateGenreDtos dto)
        {
           var genre =new Genre{Name = dto.name};
           await _genresService.ADD(genre);
            return Ok(genre);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(byte id, [FromBody]CreateGenreDtos dto) { 
            var genre = await _genresService.GetById(id);
            if (genre == null) 
                return NotFound($"No genre was found with ID:{id}");
            genre.Name = dto.name;
          _genresService.Update(genre);
            return Ok(genre);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(byte id)
        {

            var genre = await _genresService.GetById(id);
            if (genre == null)
                return NotFound($"No genre was found with ID:{id}");

          
            _genresService.Delete(genre);
            return Ok(genre);
        }
    }
}
