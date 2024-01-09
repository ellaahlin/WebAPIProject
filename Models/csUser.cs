using System;
using System.Diagnostics.Metrics;

namespace Models
{
	public class csUser : IUser, ISeed<csUser>, IEquatable<csUser>
    {
        public virtual Guid UserId { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual List<IComment> Comments { get; set; } = null;

        public override string ToString() => $"{FirstName} {LastName}";

        #region constructors
        public csUser() { }
        public csUser(csUser org)
        {
            this.Seeded = org.Seeded;
            this.UserId = org.UserId;
            this.FirstName = org.FirstName;
            this.LastName = org.LastName;
            this.Email = org.Email;
        }
        #endregion

        public bool Equals(csUser other) => (other != null) ? ((UserId, FirstName, LastName, Email) ==
            (other.UserId, other.FirstName, other.LastName, other.Email)) : false;

        public override bool Equals(object obj) => Equals(obj as csUser);
        public override int GetHashCode() => (UserId, FirstName, LastName, Email).GetHashCode();

        #region randomly seed this instance
        public bool Seeded { get; set; } = false;

        public virtual csUser Seed(csSeedGenerator sgen)
        {
            Seeded = true;
            UserId = Guid.NewGuid();
            FirstName = sgen.FirstName;
            LastName = sgen.LastName;
            Email = sgen.Email();

            return this;
        }
        #endregion
    }
}

