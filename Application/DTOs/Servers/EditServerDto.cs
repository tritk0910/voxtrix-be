using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Servers;

public class EditServerDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public IFormFile Avatar { get; set; }
    public bool ResetAvatar { get; set; }
}