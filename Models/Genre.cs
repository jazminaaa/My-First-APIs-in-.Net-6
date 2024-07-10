
namespace MoviesApi.Models
{
    public class Genre
    {
        public byte Id { get; set; }

        [MaxLength(length:100)]
        public string Name { get; set; }

        
    }
}
