namespace MoviesApi.Dtos
{
    public class MovieDetailsDto
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public int year { get; set; }
        public double rate { get; set; }

        public string Storeline { get; set; }
        
        public byte[] Poster { get; set; }
        public byte GenreId { get; set; }
        public String GenreName { get; set; }
    }
}
