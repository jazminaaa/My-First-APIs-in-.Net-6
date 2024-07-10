using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GrenresController : ControllerBase
    {
        private readonly ApplicationDbContext _context ;

             public GrenresController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet] 
        public async Task<IActionResult> GetAllAsync()
        {
            var genres = await _context.Genres.OrderBy(g=>g.Name).ToListAsync();
            return Ok(genres);
        }
        [HttpPost]
        public async Task<IActionResult> CreatAsync ([FromBody] CreateGenreDto Dto)
        {
            
            var genre = new Genre { Name =Dto.Name};
            
            await _context.Genres.AddAsync(genre);
            _context.SaveChanges();
            return Ok(genre);
        }
        [HttpPut(template:"{id}")]
        //api/genres/1
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] GenreDto dto)
        {
            var genre =await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);
            if(genre == null) 
                return NotFound(value:$"no genre was found with Id:{id}");
            genre.Name = dto.Name;
            _context.SaveChanges();
            return Ok(genre);
        }

        [HttpDelete(template: "{id}")]
        public async Task<IActionResult> DeleteAsync (int id)
        {
            var genre = await _context.Genres.SingleOrDefaultAsync(genre=> genre.Id == id);
            if (genre == null)
                return NotFound(value: $"no genre was found with id :{id}");
                
            _context.Genres.Remove(genre);
            _context.SaveChanges();
            return Ok(genre);
        }


    }
}