using System.Collections.Generic;
using is_payment_system.Model;
using is_payment_system.Model.Enums;

namespace is_payment_system.ViewModel
{
    public class UserManagementViewModel
    {
        private readonly UserRepository _userRepository;

        public UserManagementViewModel()
            : this(new UserRepository())
        {
        }

        public UserManagementViewModel(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void AddUser(User user)
        {
            _userRepository.AddUser(user);
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.Users;
        }

        public User FindUserById(int id)
        {
            return _userRepository.GetUserByID(id);
        }

        public User FindUserByEmail(string email)
        {
            return _userRepository.FindUserByEmail(email);
        }

        public User FindUserByCredentials(string firstName, string lastName, string password)
        {
            return _userRepository.GetUserByNameAndPassword(firstName, lastName, password);
        }

        public bool ValidateUser(string firstName, string password)
        {
            return _userRepository.ValidateUser(firstName, password);
        }

        public bool DeleteUserById(int id)
        {
            return _userRepository.DeleteUserById(id);
        }

        public bool DeleteUserByEmail(string email)
        {
            return _userRepository.DeleteUserByEmail(email);
        }

        public int DeleteUsersByRole(UserRole role)
        {
            return _userRepository.DeleteUsersByRole(role);
        }
    }
}
