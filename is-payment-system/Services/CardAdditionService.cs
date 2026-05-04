using System.Collections.Generic;
using System.Linq;
using is_payment_system.Model;

namespace is_payment_system.Services
{
    public class CardAdditionService : IEntityAdditionService<Card>
    {
        private List<Card> _cards;

        public CardAdditionService(List<Card> cards)
        {
            _cards = cards;
        }

        public bool Exists(Card card)
        {
            return _cards.Any(c => c.CardNumber == card.CardNumber);
        }
        
        public bool IsValidCard(Card card)
        {
            return !string.IsNullOrWhiteSpace(card.CardNumber) &&
                   card.CardNumber.Length == 16 &&
                   card.CVV.Length == 3 &&
                   card.CVV.All(char.IsDigit) &&
                   card.ExpirationDate > DateTime.Now;
        }

        public bool Add(Card card)
        {
            if (Exists(card) || !IsValidCard(card)) return false;
            
            _cards.Add(card);
            return true;
        }
        
        public List<Card> GetCardsExpiringSoon(int daysThreshold = 30)
        {
            return _cards.Where(c => c.ExpirationDate <= DateTime.Now.AddDays(daysThreshold) &&
                                     c.ExpirationDate > DateTime.Now)
                .OrderBy(c => c.ExpirationDate)
                .ToList();
        }

        public Card GetCardByNumber(string cardNumber)
        {
            return _cards.FirstOrDefault(c => c.CardNumber == cardNumber);
        }
    }
}