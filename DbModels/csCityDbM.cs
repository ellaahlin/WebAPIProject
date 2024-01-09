using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;

using Configuration;
using Models;
using Models.DTO;
using System.Reflection.Emit;

namespace DbModels
{
    [Table("Cities", Schema = "supusr")]
    public class csCityDbM : csCity, ISeed<csCityDbM>, IEquatable<csCityDbM>
    {
        [Key]       // for EFC Code first
        public override Guid CityID { get; set; }

        [Required]
        public override string CityName { get; set; }

        [Required]
        public override string Country { get; set; }

        #region correcting the 1st migration error
        //0 or many friends can live on the same address
        [JsonIgnore]
        public virtual List<csAttractionDbM> AttractionDbMs { get; set; } = null;

        [NotMapped] //application layer can continue to read a List of Pets without code change
        public override List<IAttraction> Attractions { get => AttractionDbMs?.ToList<IAttraction>(); set => new NotImplementedException(); }
        #endregion

        public bool Equals(csCityDbM other) => (other != null) ? ((CityID, CityName, Country) ==
            (other.CityID, other.CityName, other.Country)) : false;

        public override bool Equals(object obj) => Equals(obj as csCityDbM);
        public override int GetHashCode() => (CityID, CityName, Country).GetHashCode();


        #region randomly seed this instance
        public override csCityDbM Seed(csSeedGenerator sgen)
        {
            base.Seed(sgen);
            return this;
        }
        #endregion

        #region Update from DTO
        public csCityDbM UpdateFromDTO(csCityCUdto org)
        { 
            CityName = org.CityName;
            Country = org.Country;

            //We will set this when DbM model is finished
            //FriendId = org.FriendId;

            return this;
        }
        #endregion

        #region constructors
        public csCityDbM() { }
        public csCityDbM(csCityCUdto org)
        {
            CityID = Guid.NewGuid();
            UpdateFromDTO(org);
        }
        #endregion
    }
}

