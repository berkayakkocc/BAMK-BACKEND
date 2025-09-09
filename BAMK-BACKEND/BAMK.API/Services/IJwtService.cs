namespace BAMK.API.Services
{
    public interface IJwtService
    {
        string GenerateToken(object user);
        bool ValidateToken(string token);
        string GetUserIdFromToken(string token);
    }
}
