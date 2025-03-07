using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Servers;

public class CreateServerDto
{
    public string Name { get; set; }
    public IFormFile Avatar { get; set; }
}