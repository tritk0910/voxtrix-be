using Application.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class GifService(HttpClient httpClient, IConfiguration configuration) : IGifService
{
    public async Task<string> Search(string query, int limit = 10, string pos = null)
    {
        var parameters = new Dictionary<string, string>
        {
            ["q"] = query,
            ["key"] = configuration["TenorApiKey"],
            ["limit"] = limit.ToString(),
            ["media_filter"] = "mp4",
            ["pos"] = pos
        };
        var url = QueryHelpers.AddQueryString("https://tenor.googleapis.com/v2/search", parameters);
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
    public Task<string> GetById(string id)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetRandom(string tag, int limit = 10)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetTrending()
    {
        var parameters = new Dictionary<string, string>
        {
            ["key"] = configuration["TenorApiKey"],
            ["type"] = "featured"
        };
        var url = QueryHelpers.AddQueryString("https://tenor.googleapis.com/v2/categories", parameters);
        var response = await httpClient.GetStringAsync(url);

        // Parse the original JSON
        var originalData = System.Text.Json.JsonDocument.Parse(response);
        var tags = originalData.RootElement.GetProperty("tags");

        // Create new transformed object
        var transformedData = new
        {
            categories = tags.EnumerateArray().Select(tag => new
            {
                name = tag.GetProperty("searchterm").GetString(),
                src = tag.GetProperty("image").GetString()
            })
        };

        // Serialize back to JSON
        return System.Text.Json.JsonSerializer.Serialize(transformedData);
    }

    public Task<string> GetCategories(string tag, int limit = 10)
    {
        throw new NotImplementedException();
    }
}