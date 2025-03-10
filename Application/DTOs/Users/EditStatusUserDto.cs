using System.Text.Json.Serialization;
using Domain.Entities;

namespace Application.DTOs.Users;

public class EditStatusUserDto
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserStatus Status { get; set; }
}