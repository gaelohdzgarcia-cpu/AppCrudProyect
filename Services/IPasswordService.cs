namespace AppCrud.Services
{
    public interface IPasswordService
    {
        string HashPassword(string password);

        bool VerifyPassword(string password, string passwordHash);
    }
}