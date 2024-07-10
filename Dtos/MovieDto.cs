namespace MoviesApi.Dtos
{
    public class MovieDto
    {
        [MaxLength(length:250)]
        public string Title { get; set; }
        public int year { get; set; }
        public double rate { get; set; }

        [MaxLength(length: 2500)]
        public string Storeline { get; set; }
        public int MyProperty { get; set; }
        public IFormFile? Poster { get; set; }
        public byte GenreId { get; set; }
    }
}
