using System;
using is_payment_system.Model.Enums;

namespace is_payment_system.Model
{
    public class Transaction
    {
        private int _id;
        private decimal _amount;
        private DateTime _timestamp;
        private TransactionStatus _status;

        // TODO: DB tables relation
        private int _senderId;
        private int _recipientId;
        
        public Transaction()
        {
        }

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public decimal Amount
        {
            get => _amount;
            set => _amount = value;
        }

        public DateTime Timestamp
        {
            get => _timestamp;
            set => _timestamp = value;
        }

        public TransactionStatus Status
        {
            get => _status;
            set => _status = value;
        }

        public int Sender
        {
            get => _senderId;
            set => _senderId = value;
        }

        public int Recipient
        {
            get => _recipientId;
            set => _recipientId = value;
        }
    }
}