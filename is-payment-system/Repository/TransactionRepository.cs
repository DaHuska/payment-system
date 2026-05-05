using is_payment_system.Model;
using is_payment_system.Model.Enums;
using is_payment_system.Service;
using Microsoft.EntityFrameworkCore;

namespace is_payment_system.Repository;

public class TransactionRepository
{
    public TransactionRepository() {}

    public List<Transaction> Transactions
    {
        get
        {
            using var ctx = new PaymentSystemDbContext();
            return (from t in ctx.Transactions
                    select t).ToList();
        }
    }
    
    public void AddTransaction(Transaction transaction)
    {
        using var ctx = new PaymentSystemDbContext();
        ctx.Transactions.Add(transaction);
        ctx.SaveChanges();
    }

    public Transaction FindTransactionBySenderId(int id)
    {
        using var ctx = new PaymentSystemDbContext();
        return (from t in ctx.Transactions
                where t.Sender == id
                select t).FirstOrDefault();
    }

    public Transaction FindTransactionByReceiverId(int id)
    {
        using var ctx = new PaymentSystemDbContext();
        return (from t in ctx.Transactions
                where t.Recipient == id
                select t).FirstOrDefault();
    }

    
    public bool DeleteTransactionById(int id)
    {
        using var ctx = new PaymentSystemDbContext();
        var transaction = (from t in ctx.Transactions
                           where t.Id == id
                           select t).FirstOrDefault();
        if (transaction == null)
        {
            return false;
        }

        ctx.Transactions.Remove(transaction);
        ctx.SaveChanges();
        return true;
    }
    
    public Transaction FindTransactionById(int id)
    {
        using var ctx = new PaymentSystemDbContext();
        return (from t in ctx.Transactions
            where t.Id == id
            select t).FirstOrDefault();
    }

    public int DeleteTransactionsBySender(int senderAccountId)
    {
        using var ctx = new PaymentSystemDbContext();
        var transactions = (from t in ctx.Transactions
                            where t.Sender == senderAccountId
                            select t).ToList();

        if (transactions.Count == 0)
        {
            return 0;
        }

        ctx.Transactions.RemoveRange(transactions);
        ctx.SaveChanges();
        return transactions.Count;
    }

    public int DeleteOldTransactions(DateTime beforeDate)
    {
        using var ctx = new PaymentSystemDbContext();
        var transactions = (from t in ctx.Transactions
                            where t.Timestamp < beforeDate
                            select t).ToList();

        if (transactions.Count == 0)
        {
            return 0;
        }

        ctx.Transactions.RemoveRange(transactions);
        ctx.SaveChanges();
        return transactions.Count;
    }
    
    public int CountPaymentsByUser(int userId)
    {
        using var ctx = new PaymentSystemDbContext();
    
        return (from t in ctx.Transactions
            where t.Sender == userId
            select t).Count();
    }
    
    public int CountPendingPaymentsByUser(int userId)
    {
        using var ctx = new PaymentSystemDbContext();
    
        return (from t in ctx.Transactions
            where t.Sender == userId && t.Status == 0
            select t).Count();
    }
}
