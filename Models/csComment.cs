using System;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;

namespace Models
{
    public class csComment : IComment, ISeed<csComment>
    {
        public virtual Guid CommentID { get; set; }
        public virtual string Comment { get; set; }

        public virtual IAttraction Attraction { get; set; } = null;
        
        public virtual IUser User { get; set; } = null;

        public override string ToString() => $"{Comment}";

        #region constructors
        public csComment() { }
        public csComment(csComment org)
        {
            this.Seeded = org.Seeded;

            this.CommentID = org.CommentID;
            this.Comment = org.Comment;
            this.User = org.User;
        }
        #endregion

        #region randomly seed this instance
        public bool Seeded { get; set; } = false;

        public virtual csComment Seed(csSeedGenerator sgen)
        {
            Seeded = true;
            CommentID = new Guid();
            Comment = sgen.Comments;
            return this;
        }
        #endregion
    }
}

