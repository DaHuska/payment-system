using System.Collections.Generic;
using System.Text;
using System;
using is_payment_system.Model.Enums;

namespace is_payment_system.Model
{
    public class User
    {
        private int _id;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _password;
        private UserRole _role;
        private DateTime _dateCreated;
        private bool _isActive;

        //TODO: DB tables relations
        private List<int> _cards;
        private List<int> _transactions;
        private List<int> _merchants;
        
        public User()
        {
        }
        
        public int Id
        {
            get => _id;
            set => _id = value;
        }
        
        public string FirstName
        {
            get => _firstName;
            set => _firstName = value;
        }
        
        public string LastName
        {
            get => _lastName;
            set => _lastName = value;
        }
        
        public string Email
        {
            get => _email;
            set => _email = value;
        }
        
        public string Password
        {
            get => _password;
            set => _password = value;
        }
        
        public UserRole Role
        {
            get => _role;
            set => _role = value;
        }

        public DateTime DateCreated
        {
            get => _dateCreated;
            set => _dateCreated = value;
        }
        
        public bool IsActive
        {
            get => _isActive;
            set => _isActive = value;
        }

        public List<int> Cards
        {
            get => _cards;
            set => _cards = value;
        }

        public List<int> Transactions
        {
            get => _transactions;
            set => _transactions = value;
        }

        public List<int> Merchants
        {
            get => _merchants;
            set => _merchants = value;
        }
    }
}