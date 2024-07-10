using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MoviesApi.Controllers
{
    [Route(template: "api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private new List<string> _allowedExtensions = new List<string> { ".jpg",".png"} ;
        private long _maxAllowedPosterSize = 1048576;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync() 
        {
            var movies  = await _context.Movies
                .OrderByDescending(x => x.Rate)
                .Include(m => m.Genre)
                .select(m => new MovieDetailsDto
                {
                    Id = m.Id,
                    GenreId = m.GenreId,
                    GenreName = m.GenreName,
                    Poster = m.Poster,
                    rate = m.Rate,
                    Storeline = m.Storeline,
                    Title = m.Title,
                    year = m.Year,

                })
                .ToListAsync();
            return Ok(movies);
        }



        [HttpGet(template: "{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movie = await _context.Movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == id);

            if(movie == null)
                return NotFound();


             
            var dto = new MovieDetailsDto
            {

                Id = movie.Id,
                GenreId = movie.GenreId,
                GenreName = movie.Genre?.Name,
                Poster = movie.Poster,
                rate = movie.Rate,
                Storeline = movie.Storeline,
                Title = movie.Title,
                year = movie.Year,
                 
            };

            return Ok(dto);
        }

        [HttpGet (template: "GetByGenreId")]
        public async Task<IActionResult> GetByGenreIdAsync (byte GenreId)
        {
            var movies = await _context.Movies
               .where(m=> m.GenreId == GenreId)
               .OrderByDescending(x => x.Rate)
               .Include(m => m.Genre)
               .select(m => new MovieDetailsDto
               {

                   Id = m.Id,
                   GenreId = m.GenreId,
                   GenreName = m.GenreName,
                   Poster = m.Poster,
                   rate = m.Rate,
                   Storeline = m.Storeline,
                   Title = m.Title,
                   year = m.Year,

               })
               .ToListAsync();
            return Ok(movies);

        }
         

        [HttpPost]

        public async Task<IActionResult> creatAsync([FromForm]MovieDto Dto)

        {
            if (Dto.Poster == null)
                return BadRequest(error: "poster is required!");

            if(!_allowedExtensions.Contains(Path.GetExtension(Dto.Poster.FileName).ToLower()))
                return BadRequest(error: "only .png and .jpg images are allowed!!");
            if (Dto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest(error: "max allowed size for poster is 1MB!");

            var isValidGenre = await _context.Genres.AnyAsync(g=> g.id == Dto.GenreId);

            if (!isValidGenre)
                return BadRequest(error: "invalid genre id");

            using var dataStream = new MemoryStream();

                await Dto.Poster.CopyToAsync(dataStream);

            var Movie = new Movie
            {
                GenreId = Dto.GenreId,
                Title = Dto.Title,
                Poster = dataStream.ToArray(),
                Rate = Dto.rate,
                Storeline = Dto.Storeline,
                Year = Dto.year,
            };
            await _context.AddAsync(Movie);
            _context.SaveChages();
            return Ok(Movie);

        }

        [HttpPut]
        public async Task<IActionResult> updateAsync (int id, [FromForm]MovieDto Dto)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
                return NotFound(value: $"no movie was found with id {id}");


            var isValidGenre = await _context.Genres.AnyAsync(g => g.id == Dto.GenreId);

            if (!isValidGenre)
                return BadRequest(error: "invalid genre id !");

            if(Dto.Poster != null)
            {

                if (!_allowedExtensions.Contains(Path.GetExtension(Dto.Poster.FileName).ToLower()))
                    return BadRequest(error: "only .png and .jpg images are allowed!!");
                if (Dto.Poster.Length > _maxAllowedPosterSize)
                    return BadRequest(error: "max allowed size for poster is 1MB!");

                using var dataStream = new MemoryStream();

                await Dto.Poster.CopyToAsync(dataStream);

                movie.Poster = dataStream.ToArray();
            }

            movie.titre = Dto.Title;
            movie.GenreId = Dto .GenreId;
            movie.Year = Dto.Year;
            movie.Storieline = Dto.Storeline;
            movie.Rate = Dto.rate;

            _context.SaveChanges();
            return Ok(movie);


        }

        [HttpDelete (template: "{id}")] 
        public async Task<IActionResult> deleteAsync (int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)  
                return NotFound (Value: $"No Movies found with ID {id}");
                
                _context.Remove(movie);
            _context.SaveChanges();

            return Ok(movie);
        }
    }
}
