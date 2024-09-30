namespace MoviesAPI.Dtos
{
    public class CreateGenreDtos
    {
        [MaxLength(100)]
        public string name { get; set; }
    }
}
