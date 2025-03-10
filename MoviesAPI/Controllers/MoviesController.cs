﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MoviesAPI.Dtos;
using MoviesAPI.services;

namespace MoviesAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly ImoviesService _moviesService;
        private readonly IGenresService _genresService;


        private new  List<string>_allowedExtensions=new List<string> { ".jpg",".png",".jpeg"};
        private long _maxAllowedPosterSize = 1048576 ;
        public MoviesController(ImoviesService moviesService, IGenresService genresService, IMapper mapper)
        {

            _moviesService = moviesService;
            _genresService = genresService;
            _mapper = mapper;
        }



        [HttpGet]
        public async Task<IActionResult>GetAllAsync()
        {
            var movies = await _moviesService.GetAll();
            var data = _mapper.Map<IEnumerable<MovieDetailsDto>>(movies);
            return Ok(movies);
        }




        [HttpGet("{id}")]
        public async Task <IActionResult> GetByIdAsync(int id)
        {
            var movie = await _moviesService.GetById(id);

            if (movie == null)
                return NotFound();

            var dto = _mapper.Map<moviedetailsdto>(movie);
            return Ok(dto);
        }




        [HttpGet(template:"GetByGenreId")]
        
         public async Task <IActionResult> GetByGenreIdAsync(byte genreId)
        {
            var movies = await _moviesService.GetAll(genreId);
            var data = _mapper.Map<IEnumerable<moviedetailsdto>>(movies);

            return Ok(movies);
        }





        [HttpPost]
        public async Task <IActionResult> CreateAsync([FromForm]MovieDtos dto)
        {
            if (dto.Poster == null)
                return BadRequest("Poster is required");


            if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .jpg and .png images are allowed!");

            if (dto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest("MAX allowed size for poster is 1Mb");

            var isvalidGenre= await _genresService.IsValidGenre(dto.GenreId);
            if (!isvalidGenre)
                return BadRequest("Invalid genre ID!");

            using var dataStream = new MemoryStream();

            await dto.Poster.CopyToAsync(dataStream);

            var movie = _mapper.Map<Movie>(dto);
            movie.Poster = dataStream.ToArray();

            _moviesService.add(movie);



            return Ok(movie);
        }






        [HttpPut("{id}")]
        public async Task <IActionResult> UpdateAsync(int id, [FromForm] MovieDtos dto)
        
        {
            var isvalidGenre = await _genresService.IsValidGenre(dto.GenreId);

            if (!isvalidGenre)
                return BadRequest("Invalid genre ID!");

            var movie = await _moviesService.GetById(id);
            if (movie == null)
                return NotFound($"No movie was found with ID:{id}");

            if (dto.Poster != null)
            {
                if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only .jpg and .png images are allowed!");

                if (dto.Poster.Length > _maxAllowedPosterSize)
                    return BadRequest("MAX allowed size for poster is 1Mb");

                using var datastream = new MemoryStream();
                await dto.Poster.CopyToAsync(datastream);
                movie.Poster = datastream.ToArray();
            }

            movie.Title = dto.Title;
            movie.GenreId = dto.GenreId;
            movie.Year = dto.Year;
            movie.Storeline = dto.Storeline;
            movie.Rate = dto.Rate;


        _moviesService.Update(movie);

            return Ok(movie);
        }






        [HttpDelete("{id}")]
        public async Task <IActionResult>DeleteAsync(int id)
        {
            var movie = await _moviesService.GetById(id);
            if (movie == null)
                return NotFound($"No movie was found with ID:{id}");

            _moviesService.Delete(movie);
            return Ok(movie);

        }
    }
}
