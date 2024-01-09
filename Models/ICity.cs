using System;
namespace Models
{
	public interface ICity { 
	
        public Guid CityID { get; set; }
        public string CityName { get; set; }
        public string Country { get; set; }

        // A city can have many different attractions
        public List<IAttraction> Attractions { get; set; }
    }
}

