using System.Security.Cryptography;
using System.Text;
using is_payment_system.Model.Enums;
using is_payment_system.Service;
using Microsoft.EntityFrameworkCore;

namespace is_payment_system.Model;

public class UserRepository
{
    public UserRepository()
    {
    }

    public List<User> Users
    {
        get
        {
            using var ctx = new PaymentSystemDbContext();
            return (from u in ctx.Users
                    select u).ToList();
        }
    }

    public int NextId
    {
        get
        {
            using var ctx = new PaymentSystemDbContext();
            var maxId = (from u in ctx.Users
                         select (int?)u.Id).Max() ?? 0;
            return maxId + 1;
        }
    }

    public void AddUser(User user)
    {
        using var ctx = new PaymentSystemDbContext();
        ctx.Users.Add(user);
        ctx.SaveChanges();
    }

    public User? RegisterUser(string firstName, string lastName, string email, string password)
    {
        using var ctx = new PaymentSystemDbContext();

        var existingUser = (from u in ctx.Users
                            where u.Email == email
                            select u).FirstOrDefault();

        if (existingUser != null)
        {
            return null;
        }

        var user = new User
        {
            Id = (from u in ctx.Users
                  select (int?)u.Id).Max() ?? 0 + 1,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = hashPass(password),
            Role = UserRole.USER
        };

        ctx.Users.Add(user);
        ctx.SaveChanges();

        return user;
    }

    public void DeleteUser(User user)
    {
        using var ctx = new PaymentSystemDbContext();
        ctx.Users.Remove(user);
        ctx.SaveChanges();
    }

    public bool ValidateUser(string name, string password)
    {
        using var ctx = new PaymentSystemDbContext();
        return (from u in ctx.Users
                where u.FirstName == name && u.Password == password
                select u).Any();
    }

    public User GetUserByNameAndPassword(string firstName, string lastName, string password)
    {
        using var ctx = new PaymentSystemDbContext();
        return (from u in ctx.Users
                where u.FirstName == firstName
                      && u.LastName == lastName
                      && u.Password == password
                select u).FirstOrDefault();
    }

    public User GetUserByID(int id)
    {
        using var ctx = new PaymentSystemDbContext();
        return (from u in ctx.Users
                where u.Id == id
                select u).FirstOrDefault();
    }

    public User FindUserByEmail(string email)
    {
        using var ctx = new PaymentSystemDbContext();
        return (from u in ctx.Users
                where u.Email == email
                select u).FirstOrDefault();
    }

    public bool DeleteUserById(int id)
    {
        using var ctx = new PaymentSystemDbContext();
        var user = (from u in ctx.Users
                    where u.Id == id
                    select u).FirstOrDefault();
        if (user == null)
        {
            return false;
        }

        ctx.Users.Remove(user);
        ctx.SaveChanges();
        return true;
    }

    public bool DeleteUserByEmail(string email)
    {
        using var ctx = new PaymentSystemDbContext();
        var user = (from u in ctx.Users
                    where u.Email == email
                    select u).FirstOrDefault();
        if (user == null)
        {
            return false;
        }

        ctx.Users.Remove(user);
        ctx.SaveChanges();
        return true;
    }

    public int DeleteUsersByRole(UserRole role)
    {
        using var ctx = new PaymentSystemDbContext();
        var users = (from u in ctx.Users
                     where u.Role == role
                     select u).ToList();

        if (users.Count == 0)
        {
            return 0;
        }

        ctx.Users.RemoveRange(users);
        ctx.SaveChanges();
        return users.Count;
    }

    private string hashPass(string pass)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(pass));
        return Convert.ToHexString(bytes);
    }
}