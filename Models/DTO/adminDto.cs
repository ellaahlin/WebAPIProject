using System;
namespace Models.DTO
{
    public class adminInfoDbDto
    {
        public int nrSeededAttractions { get; set; } = 0;
        public int nrUnseededAttractions { get; set; } = 0;

        public int nrSeededComments { get; set; } = 0;
        public int nrUnseededComments { get; set; } = 0;

        public int nrSeededCities { get; set; } = 0;
        public int nrUnseededCities { get; set; } = 0;

        public int nrSeededUsers { get; set; } = 0;
        public int nrUnseededUsers { get; set; } = 0;
    }
}

