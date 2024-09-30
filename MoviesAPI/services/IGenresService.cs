namespace MoviesAPI.services
{
    public interface IGenresService
    {
        Task<IEnumerable<Genre>> GetALL();
        Task<Genre> GetById(byte Id);

        Task<Genre> ADD(Genre genre);   
        Genre Update(Genre genre);

        Genre Delete(Genre genre);

        Task<bool> IsValidGenre(byte id);
    }
}
