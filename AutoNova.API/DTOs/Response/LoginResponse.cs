namespace AutoNova.API.DTOs.Response;

public class LoginResponse
{
    public string Token      { get; set; } = string.Empty;
    public string Role       { get; set; } = string.Empty;
    public string Name       { get; set; } = string.Empty;
    public string Email      { get; set; } = string.Empty;
    public Guid   UserId     { get; set; }
    public DateTime Expiration { get; set; }
}
