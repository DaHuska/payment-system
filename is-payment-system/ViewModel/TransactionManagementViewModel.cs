using System;
using System.Collections.Generic;
using System.Linq;
using is_payment_system.Model;
using is_payment_system.Repository;

namespace is_payment_system.ViewModel
{
    public class TransactionManagementViewModel
    {
        private readonly TransactionRepository _transactionRepository;

        public TransactionManagementViewModel()
            : this(new TransactionRepository())
        {
        }

        public TransactionManagementViewModel(TransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public void AddTransaction(Transaction transaction)
        {
            _transactionRepository.AddTransaction(transaction);
        }

        public List<Transaction> GetAllTransactions()
        {
            return _transactionRepository.Transactions;
        }

        public Transaction FindTransactionById(int id)
        {
            return _transactionRepository.Transactions.FirstOrDefault(t => t.Id == id);
        }

        public Transaction FindTransactionBySender(int senderId)
        {
            return _transactionRepository.FindTransactionBySenderId(senderId);
        }

        public Transaction FindTransactionByReceiver(int receiverId)
        {
            return _transactionRepository.FindTransactionByReceiverId(receiverId);
        }

        public bool DeleteTransactionById(int id)
        {
            return _transactionRepository.DeleteTransactionById(id);
        }

        public int DeleteTransactionsBySender(int senderId)
        {
            return _transactionRepository.DeleteTransactionsBySender(senderId);
        }

        public int DeleteOldTransactions(DateTime beforeDate)
        {
            return _transactionRepository.DeleteOldTransactions(beforeDate);
        }
    }
}