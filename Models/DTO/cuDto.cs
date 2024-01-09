using System;
using System.Diagnostics.Metrics;
using System.Net;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace Models.DTO
{
    //DTO is a DataTransferObject, can be instanstiated by the controller logic
    //and represents a, fully instatiable, subset of the Database models
    //for a specific purpose.

    //These DTO are simplistic and used to Update and Create objects in the database
    public class csAttractionCUdto
    {
        public virtual Guid? AttractionID { get; set; }
        public virtual string AttractionName { get; set; }
        public virtual List<Guid> CommentID { get; set; } = null;
        public virtual Guid CityID { get; set; }
        public virtual string Category { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }


        public csAttractionCUdto() { }
        public csAttractionCUdto(IAttraction org)
        { 
            AttractionID = org.AttractionID;
            AttractionName = org.AttractionName;
            Category = org.Category;
            Title = org.Title;
            Description = org.Description;
            CommentID = org.Comments?.Select(i => i.CommentID).ToList();
            CityID = org.City.CityID;
        }
    }

    public class csCityCUdto
    {
        public virtual Guid CityID { get; set; }

        public virtual string CityName { get; set; }
        public virtual string Country { get; set; }

        public csCityCUdto() { }
        public csCityCUdto(ICity org)
        {
            CityID = org.CityID;
            CityName = org.CityName;
            Country = org.Country;
        }
    }

    public class csCommentCUdto
    {
        public virtual Guid AttractionId { get; set; }
        public virtual Guid? CommentID { get; set; }
        public virtual string Comment { get; set; }

        public csCommentCUdto() { }
        public csCommentCUdto(IComment org)
        {
            AttractionId = org.Attraction.AttractionID;
            CommentID = org.CommentID;
            Comment = org.Comment;
        }
    }

    public class csUserCUdto
    {
        public virtual List<Guid> CommentID { get; set; } = null;
        public virtual Guid? UserID { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }

        public csUserCUdto() { }
        public csUserCUdto(IUser org)
        {
            CommentID = org.Comments.Select(i => i.CommentID).ToList();
            UserID = org.UserId;
            FirstName = org.FirstName;
            LastName = org.LastName;
            Email = org.Email;
        }
    }
}

