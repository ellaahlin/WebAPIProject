using System;
using System.Net;
using System.Reflection.Emit;

namespace Models
{
	public class csCity : ICity, ISeed<csCity>, IEquatable<csCity>
    {
        public virtual Guid CityID { get; set; }
        public virtual string CityName { get; set; }
        public virtual string Country { get; set; }

        public virtual List<IAttraction> Attractions { get; set; } = null;
        
        public override string ToString() => $"{CityName} in {Country}";

        #region constructors
        public csCity() { }
        public csCity(csCity org)
        {
            this.Seeded = org.Seeded;
            this.CityID = org.CityID;
            this.CityName = org.CityName;
            this.Country = org.Country;
        }
        #endregion

        public bool Equals(csCity other) => (other != null) ? ((CityID, CityName, Country) ==
            (other.CityID, other.CityName, other.Country)) : false;

        public override bool Equals(object obj) => Equals(obj as csCity);
        public override int GetHashCode() => (CityID, CityName, Country).GetHashCode();

        #region randomly seed this instance
        public bool Seeded { get; set; } = false;

        public virtual csCity Seed(csSeedGenerator sgen)
        {
            Seeded = true;
            CityID = Guid.NewGuid();
            CityName = sgen.City;
            Country = sgen.Country;

            return this;
        }
        #endregion
    }
}

