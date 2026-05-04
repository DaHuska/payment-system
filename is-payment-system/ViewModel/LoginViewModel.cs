using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;
using is_payment_system.Logging;
using is_payment_system.Model;

public class LoginViewModel
{
    public string Username { get; set; }
    public string Password { get; set; }

    public ICommand LoginCommand { get; }

    public User? Login(string email, string password)
    {
        UserRepository repo = new UserRepository();

        var user = repo.Users.FirstOrDefault(u =>
            u.Email == email && u.Password == hashPass(password));

        return user;
    }

    private string hashPass(string pass)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(pass));
        return Convert.ToHexString(bytes);
    }
}