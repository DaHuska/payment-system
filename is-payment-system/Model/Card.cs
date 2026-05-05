using System;
using System.Collections.Generic;
using System.Text;

namespace is_payment_system.Model
{
    public class Card
    {
        private int _id;
        private string _cardNumber;
        private string _cvv;
        private string _iban;
        private decimal _balance;
        private DateTime _createdDate;
        private DateTime _expirationDate;

        private int _userId;
        
        public Card()
        {
        }

        public int Id
        {
            get => _id;
            set => _id = value;
        }
        
        public string CardNumber
        {
            get => _cardNumber;
            set => _cardNumber = value;
        }
        
        public string CVV
        {
            get => _cvv;
            set => _cvv = value;
        }
        
        public string Iban
        {
            get => _iban;
            set => _iban = value ?? throw new ArgumentNullException(nameof(value));
        }
        
        public DateTime ExpirationDate
        {
            get => _expirationDate;
            set => _expirationDate = value;
        }

        public DateTime CreatedDate
        {
            get => _createdDate;
            set => _createdDate = value;
        }

        public decimal Balance
        {
            get => _balance;
            set => _balance = value;
        }

        public int UserId
        {
            get => _userId;
            set => _userId = value;
        }
    }
}
