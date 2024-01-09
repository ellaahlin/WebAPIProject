using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

using Models;
using Models.DTO;

namespace DbModels
{
    [Table("Users", Schema = "supusr")]
    public class csUserDbM : csUser, IEquatable<csUserDbM>, ISeed<csUserDbM>
    {
        [Key]     
        public override Guid UserId { get; set; }

        [Required]
        public override string FirstName { get; set; }

        [Required]
        public override string LastName { get; set; }

        [Required]
        public override string Email { get; set; }

        [NotMapped] //application layer can continue to read a Comment without code change
        public override List<IComment> Comments { get => CommentsDbM?.ToList<IComment>(); set => new NotImplementedException(); }

        [JsonIgnore]     //create own Foreign Key step 2
        public virtual List<csCommentDbM> CommentsDbM { get; set; } = null;    //This is implemented in the database table

        public bool Equals(csUserDbM other) => (other != null) ? ((UserId, FirstName, LastName, Email) ==
            (other.UserId, other.FirstName, other.LastName, other.Email)) : false;

        public override bool Equals(object obj) => Equals(obj as csUserDbM);
        public override int GetHashCode() => (UserId, FirstName, LastName, Email).GetHashCode();

        #region randomly seed this instance
        public override csUserDbM Seed(csSeedGenerator sgen)
        {
            base.Seed(sgen);
            return this;
        }
        #endregion

        #region Update from DTO
        public csUserDbM UpdateFromDTO(csUserCUdto org)
        {
            FirstName = org.FirstName;
            LastName = org.LastName;
            Email = org.Email;

            //We will set this when DbM model is finished
            //FriendId = org.FriendId;

            return this;
        }
        #endregion

        #region constructors
        public csUserDbM() { }
        public csUserDbM(csUserCUdto org)
        {
            UserId = Guid.NewGuid();
            UpdateFromDTO(org);
        }
        #endregion
    }
}

