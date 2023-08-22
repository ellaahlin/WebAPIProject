using System;
namespace Models
{
    public class csQuote : IQuote, ISeed<csQuote>
    {
        public virtual Guid QuoteId { get; set; }
        public virtual string Quote { get; set; }
        public virtual string Author { get; set; }

        //One Quote can have many friends
        public virtual List<IFriend> Friends { get; set; } = null;

        #region constructors
        public csQuote() { }

        public csQuote(GoodQuote goodQuote)
        {
            QuoteId = Guid.NewGuid();
            Quote = goodQuote.Quote;
            Author = goodQuote.Author;
            Seeded = true;
        }

        #endregion

        #region randomly seed this instance
        public bool Seeded { get; set; } = false;

        public virtual csQuote Seed(csSeedGenerator sgen) => throw new NotImplementedException();
        #endregion
    }
}

