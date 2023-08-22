using System;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;

namespace Models
{
    public class csPet : IPet, ISeed<csPet>
    {
        public virtual Guid PetId { get; set; }

        public virtual enAnimalKind Kind { get; set; }
        public virtual enAnimalMood Mood { get; set; }

        public virtual string Name { get; set; }

        public virtual IFriend Friend { get; set; }

        public override string ToString() => $"{Name} the {Mood} {Kind}";

        #region constructors
        public csPet() { }
        public csPet(csPet org)
        {
            this.Seeded = org.Seeded;

            this.PetId = org.PetId;
            this.Kind = org.Kind;
            this.Name = org.Name;
        }
        #endregion

        #region randomly seed this instance
        public bool Seeded { get; set; } = false;

        public virtual csPet Seed(csSeedGenerator sgen) => throw new NotImplementedException();
        #endregion
    }
}

