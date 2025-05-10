namespace Application.Interfaces;

public interface IGifService
{
    Task<string> Search(string query, int limit = 10, string pos = null);
    Task<string> GetRandom(string tag, int limit = 10);
    Task<string> GetTrending();
    Task<string> GetById(string id);
    Task<string> GetCategories(string tag, int limit = 10);
}