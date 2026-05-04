using System;
using System.Collections.Generic;
using System.Linq;
using is_payment_system.Model;
using is_payment_system.Model.Enums;

namespace is_payment_system.Services
{
    public class UserAdditionService : IEntityAdditionService<User>
    {
        private List<User> _users;

        public UserAdditionService(List<User> users)
        {
            _users = users;
        }

        public bool Exists(User user)
        {
            return _users.Any(u => u.Email == user.Email);
        }
        
        public bool IsValidUser(User user)
        {
            return !string.IsNullOrWhiteSpace(user.FirstName) &&
                   !string.IsNullOrWhiteSpace(user.LastName) &&
                   user.Email.Contains("@") &&
                   (user.Role == UserRole.USER || user.Role == UserRole.ADMIN);
        }

        public bool Add(User user)
        {
            if (Exists(user) || !IsValidUser(user)) return false;
            
            _users.Add(user);
            return true;
        }
        
        public List<User> GetUsersByRole(UserRole role)
        {
            return _users.Where(u => u.Role == role).ToList();
        }

        public User GetUserByEmail(string email)
        {
            return _users.FirstOrDefault(u => u.Email == email);
        }
    }
}