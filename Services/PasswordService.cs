using Microsoft.AspNetCore.Identity;

namespace AppCrud.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordHasher<object> _passwordHasher = new();

        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null!, password);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            try
            {
                var result = _passwordHasher.VerifyHashedPassword(
                    null!,
                    passwordHash,
                    password);

                return result == PasswordVerificationResult.Success;
            }
            catch (FormatException)
            {
                // La contraseña todavía está guardada en texto normal.
                return password == passwordHash;
            }
        }
    }
}