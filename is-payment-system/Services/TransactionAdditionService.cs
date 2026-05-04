using System;
using System.Collections.Generic;
using System.Linq;
using is_payment_system.Model;

namespace is_payment_system.Services
{
    public class TransactionAdditionService : IEntityAdditionService<Transaction>
    {
        private List<Transaction> _transactions;

        public TransactionAdditionService(List<Transaction> transactions)
        {
            _transactions = transactions;
        }

        public bool Add(Transaction entity)
        {
            throw new NotImplementedException();
        }

        public bool Exists(Transaction transaction)
        {
            return _transactions.Any(t => t.Id == transaction.Id);
        }
        
        // public bool IsValidTransaction(Transaction transaction)
        // {
        //     bool senderExists = _bankAccounts.Any(a => a.IBan == transaction.SenderAccountId);
        //     bool receiverExists = _bankAccounts.Any(a => a.IBan == transaction.ReceiverAccountId);
        //     
        //     return transaction.Amount > 0 &&
        //            transaction.SenderAccountId != transaction.ReceiverAccountId &&
        //            senderExists &&
        //            receiverExists;
        // }

        // public bool Add(Transaction transaction)
        // {
        //     if (!IsValidTransaction(transaction)) return false;
        //     
        //     transaction.Timestamp = DateTime.Now;
        //     _transactions.Add(transaction);
        //     return true;
        // }
        
        // public List<Transaction> GetTransactionsByAccount(string accountIban)
        // {
        //     return _transactions.Where(t => t.SenderAccountId == accountIban ||
        //                                     t.ReceiverAccountId == accountIban)
        //         .OrderByDescending(t => t.Timestamp)
        //         .ToList();
        // }

        public List<Transaction> GetTransactionsByDateRange(DateTime from, DateTime to)
        {
            return _transactions.Where(t => t.Timestamp >= from && t.Timestamp <= to)
                .OrderBy(t => t.Timestamp)
                .ToList();
        }
    }
}