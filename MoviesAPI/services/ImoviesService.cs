namespace MoviesAPI.services
{
    public interface ImoviesService
    {
        Task<IEnumerable<Movie>> GetAll(byte genreId=0);
        Task<Movie> GetById(int id);  
        Task<Movie> add(Movie  movie);
        Movie Update(Movie movie);

        Movie Delete(Movie movie);


    }
}
