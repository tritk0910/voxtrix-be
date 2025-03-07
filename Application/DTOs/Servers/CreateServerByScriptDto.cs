using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Servers;

public class CreateServerByScriptDto
{
    public string Name { get; set; }
    public IFormFile Avatar { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ServerScript Script { get; set; }
}

public enum ServerScript
{
    Basic,
    Gaming,
    StudyGroup
}