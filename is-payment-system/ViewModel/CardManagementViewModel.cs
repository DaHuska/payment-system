using System.Collections.Generic;
using is_payment_system.Model;
using is_payment_system.Repository;

namespace is_payment_system.ViewModel
{
    public class CardManagementViewModel
    {
        private readonly CardRepository _cardRepository;

        public CardManagementViewModel()
            : this(new CardRepository())
        {
        }

        public CardManagementViewModel(CardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }
        
        public void AddCard(Card card, int userId)
        {
            _cardRepository.AddCard(card, userId);
        }

        public List<Card> GetAllCards()
        {
            return _cardRepository.Cards;
        }

        public Card FindCardByCardNumber(string cardNumber)
        {
            return _cardRepository.FindCardByCardNumber(cardNumber);
        }

        public Card FindCardByIban(string iban)
        {
            return _cardRepository.FindCardByIBAN(iban);
        }

        public bool DeleteCardByNumber(string cardNumber)
        {
            return _cardRepository.DeleteCardByNumber(cardNumber);
        }

        public int DeleteExpiredCards()
        {
            return _cardRepository.DeleteExpiredCards();
        }

        public int DeleteCardsByIban(string iban)
        {
            return _cardRepository.DeleteCardsByAccountIban(iban);
        }
    }
}
