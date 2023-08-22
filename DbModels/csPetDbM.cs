using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Configuration;
using Models;
using Models.DTO;

namespace DbModels
{
    public class csPetDbM : csPet, ISeed<csPetDbM>
    {
        [Key]       // for EFC Code first
        public override Guid PetId { get; set; }

        #region randomly seed this instance
        public override csPetDbM Seed(csSeedGenerator sgen)
        {
            base.Seed(sgen);
            return this;
        }
        #endregion

        #region Update from DTO
        public csPetDbM UpdateFromDTO(csPetCUdto org)
        {
            if (org == null) return null;

            Kind = org.Kind;
            Mood = org.Mood;
            Name = org.Name;

            //We will set this when DbM model is finished
            //FriendId = org.FriendId;

            return this;
        }
        #endregion

        #region constructors
        public csPetDbM() { }
        public csPetDbM(csPetCUdto org)
        {
            PetId = Guid.NewGuid();
            UpdateFromDTO(org);
        }
        #endregion
    }
}

