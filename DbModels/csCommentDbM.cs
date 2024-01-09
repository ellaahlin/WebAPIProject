using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;

using Configuration;
using Models;
using Models.DTO;


namespace DbModels
{
    [Table("Comments", Schema = "supusr")]
    public class csCommentDbM : csComment, ISeed<csCommentDbM>, IEquatable<csCommentDbM>
    {
        [Key]       // for EFC Code first
        public override Guid CommentID { get; set; }

        [Required]
        public override string Comment { get; set; }
        
        [JsonIgnore]
        public virtual Guid AttractionID { get; set; }

        [JsonIgnore]
        [ForeignKey("AttractionID")]     //create own Foreign Key step 2
        public virtual csAttractionDbM AttractionDbM { get; set; } = null;    //This is implemented in the database table        

        [NotMapped] //application layer can continue to read an Address without code change
        public override IAttraction Attraction { get => AttractionDbM; set => new NotImplementedException(); }

        [JsonIgnore]
        public virtual Guid UserId { get; set; }

        [NotMapped]
        public override IUser User { get => UserDbM; set => new NotImplementedException(); }

        [JsonIgnore]
        [ForeignKey("UserId")]
        public virtual csUserDbM UserDbM { get; set; } = null;
        

        #region implementing IEquatable

        public bool Equals(csCommentDbM other) => (other != null) ?((CommentID, Comment) ==
            (other.CommentID, other.Comment)) : false;

        public override bool Equals(object obj) => Equals(obj as csCommentDbM);
        public override int GetHashCode() => (CommentID, Comment).GetHashCode();

        #endregion

        #region randomly seed this instance
        public override csCommentDbM Seed(csSeedGenerator sgen)
        {
            base.Seed(sgen);
            return this;
        }
        #endregion

        #region Update from DTO
        public csCommentDbM UpdateFromDTO(csCommentCUdto org)
        {
            if (org == null) return null;
            Comment = org.Comment;

            return this;
        }
        #endregion

        #region constructors
        public csCommentDbM() { }
        public csCommentDbM(csCommentCUdto org)
        {
            CommentID = Guid.NewGuid();
            UpdateFromDTO(org);
        }
        #endregion
    }
}

