using System;
namespace Models.DTO
{
	public class gstusrInfoDbDto
	{
        public int nrSeededAttractions { get; set; } = 0;
        public int nrUnseededAttractions { get; set; } = 0;
        public int nrAttractionsWithComment { get; set; } = 0;

        public int nrSeededComments { get; set; } = 0;
        public int nrUnseededComments { get; set; } = 0;

        public int nrSeededCities { get; set; } = 0;
        public int nrUnseededCities { get; set; } = 0;

        public int nrSeededTitles { get; set; } = 0;
        public int nrUnseededTitles { get; set; } = 0;

        public int nrSeededDescriptions { get; set; } = 0;
        public int nrUnseededDescriptions { get; set; } = 0;

        public int nrSeededUsers { get; set; } = 0;
        public int nrUnseededUsers { get; set; } = 0;
    }

    public class gstusrInfoAttractionsDto
    {
        public string Country { get; set; } = null;
        public string City { get; set; } = null;
        public int NrAttractions { get; set; } = 0;
    }

    public class gstusrInfoCitiesDto
    {
        public string Country { get; set; } = null;
        public string City { get; set; } = null;
        public int NrCities { get; set; } = 0;
    }

    public class gstusrInfoCommentsDto
    {
        public string Comment { get; set; } = null;
        public int NrComments { get; set; } = 0;
    }

    public class gstusrInfoUsersDto
    {
        public string User { get; set; } = null;
        public int NrUsers { get; set; } = 0;
    }

    public class gstusrInfoAllDto
    {
        public gstusrInfoDbDto Db { get; set; } = null;
        public List<gstusrInfoAttractionsDto> Attractions { get; set; } = null;
        public List<gstusrInfoCitiesDto> Cities { get; set; } = null;
        public List<gstusrInfoCommentsDto> Comments { get; set; } = null;
        public List<gstusrInfoUsersDto> Users { get; set; } = null;
    }
}

