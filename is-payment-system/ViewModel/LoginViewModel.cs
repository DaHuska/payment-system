using System.Security.Cryptography;
using System.Text;
using is_payment_system.Model;

namespace is_payment_system.ViewModel;

public class LoginViewModel
{
    private UserRepository _userRepository;

    public LoginViewModel()
    {
        _userRepository = new UserRepository();
    }

    public User? Login(string email, string password)
    {
        var user = _userRepository.Users.FirstOrDefault(u =>
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