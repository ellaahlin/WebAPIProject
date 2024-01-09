using System;
namespace Models
{
    public interface IComment
    {
        public Guid CommentID { get; set; }
        public string Comment { get; set; }

        // A comment is unique for an attraction and can only be written by one user
        public IAttraction Attraction { get; set; }

        public IUser User { get; set; }
    }
}


