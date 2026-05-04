using is_payment_system.Model;
using is_payment_system.Service;
using Microsoft.EntityFrameworkCore;

namespace is_payment_system.Repository;

public class MerchantRepository
{
    public MerchantRepository()
    {
    }

    public List<Merchant> Merchants
    {
        get
        {
            using var ctx = new PaymentSystemDbContext();
            return (from m in ctx.Merchants
                select m).ToList();
        }
    }

    public int NextId
    {
        get
        {
            using var ctx = new PaymentSystemDbContext();
            var maxId = (from m in ctx.Merchants
                select (int?)m.Id).Max() ?? 0;
            return maxId + 1;
        }
    }

    public void AddMerchant(Merchant merchant)
    {
        using var ctx = new PaymentSystemDbContext();
        ctx.Merchants.Add(merchant);
        ctx.SaveChanges();
    }

    public Merchant FindMerchantById(int id)
    {
        using var ctx = new PaymentSystemDbContext();
        return (from m in ctx.Merchants
            where m.Id == id
            select m).FirstOrDefault();
    }

    public Merchant FindMerchantByEmail(string email)
    {
        using var ctx = new PaymentSystemDbContext();
        return (from m in ctx.Merchants
            where m.Email == email
            select m).FirstOrDefault();
    }

    public bool DeleteMerchantById(int id)
    {
        using var ctx = new PaymentSystemDbContext();
        var merchant = (from m in ctx.Merchants
            where m.Id == id
            select m).FirstOrDefault();

        if (merchant == null)
            return false;

        ctx.Merchants.Remove(merchant);
        ctx.SaveChanges();
        return true;
    }
}