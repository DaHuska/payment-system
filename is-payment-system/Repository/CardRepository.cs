using is_payment_system.Model;
using is_payment_system.Service;
using Microsoft.EntityFrameworkCore;

namespace is_payment_system.Repository;

public class CardRepository
{
    public CardRepository() {}

    public List<Card> Cards
    {
        get
        {
            using var ctx = new PaymentSystemDbContext();
            return (from c in ctx.Cards
                    select c).ToList();
        }
    }
    
    public void AddCard(Card card, int userId)
    {
        using var ctx = new PaymentSystemDbContext();
        var entry = ctx.Cards.Add(card);
        entry.Property("UserId").CurrentValue = userId;
        ctx.SaveChanges();
    }

    public Card FindCardByCardNumber(string cardNumber)
    {
        using var ctx = new PaymentSystemDbContext();
        return (from c in ctx.Cards
                where c.CardNumber == cardNumber
                select c).FirstOrDefault();
    }

    public Card FindCardByIBAN(string iban)
    {
        using var ctx = new PaymentSystemDbContext();
        return (from c in ctx.Cards
                where c.Iban == iban
                select c).FirstOrDefault();
    }

    public bool DeleteCardByNumber(string cardNumber)
    {
        using var ctx = new PaymentSystemDbContext();
        var card = (from c in ctx.Cards
                    where c.CardNumber == cardNumber
                    select c).FirstOrDefault();
        if (card == null)
        {
            return false;
        }

        ctx.Cards.Remove(card);
        ctx.SaveChanges();
        return true;
    }

    public int DeleteExpiredCards()
    {
        using var ctx = new PaymentSystemDbContext();
        var now = DateTime.Now;
        var expired = (from c in ctx.Cards
                       where c.ExpirationDate < now
                       select c).ToList();

        if (expired.Count == 0)
        {
            return 0;
        }

        ctx.Cards.RemoveRange(expired);
        ctx.SaveChanges();
        return expired.Count;
    }

    public int DeleteCardsByAccountIban(string iban)
    {
        using var ctx = new PaymentSystemDbContext();
        var cards = (from c in ctx.Cards
                     where c.Iban == iban
                     select c).ToList();

        if (cards.Count == 0)
        {
            return 0;
        }

        ctx.Cards.RemoveRange(cards);
        ctx.SaveChanges();
        return cards.Count;
    }
}
