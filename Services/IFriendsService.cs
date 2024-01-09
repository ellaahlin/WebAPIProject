using System;
using Models;
using Models.DTO;

namespace Services
{
    public interface IAttractionService
	{
        //To test the overall layered structure
        public string InstanceHello { get; }

        //For inital test only, so a limited set on non-async methods in this example
        public gstusrInfoAllDto Info { get; }
        public adminInfoDbDto RemoveSeed(loginUserSessionDto usr, bool seeded);
        public adminInfoDbDto Seed(loginUserSessionDto usr, int nrOfItems);

        public List<IAttraction> ReadAttractions(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize);


        //Full set of async methods
        public Task<gstusrInfoAllDto> InfoAsync { get; }
        public Task<adminInfoDbDto> SeedAsync(loginUserSessionDto usr, int nrOfItems);
        public Task<adminInfoDbDto> RemoveSeedAsync(loginUserSessionDto usr, bool seeded);

        public Task<List<IAttraction>> ReadAttractionsAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize, bool hasComment);
        public Task<List<IAttraction>> ReadAttractionsWithCommentsAsync(loginUserSessionDto usr, bool seeded, string filter, int pageNumber, int pageSize, bool hasComment);

        public Task<IAttraction> ReadAttractionAsync(loginUserSessionDto usr, Guid id, bool flat);
        public Task<IAttraction> DeleteAttractionAsync(loginUserSessionDto usr, Guid id);
        public Task<IAttraction> UpdateAttractionAsync(loginUserSessionDto usr, csAttractionCUdto item);
        public Task<IAttraction> CreateAttractionAsync(loginUserSessionDto usr, csAttractionCUdto item);

        public Task<List<IComment>> ReadCommentsAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize);
        public Task<IComment> ReadCommentAsync(loginUserSessionDto usr, Guid id, bool flat);
        public Task<List<IComment>> ReadCommentByUserAsync(loginUserSessionDto usr, Guid id, bool flat);
        public Task<IComment> DeleteCommentAsync(loginUserSessionDto usr, Guid id);
        public Task<IComment> UpdateCommentAsync(loginUserSessionDto usr, csCommentCUdto item);
        public Task<IComment> CreateCommentAsync(loginUserSessionDto usr, csCommentCUdto item);

        public Task<List<ICity>> ReadCitiesAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize);
        public Task<ICity> ReadCityAsync(loginUserSessionDto usr, Guid id, bool flat);
        public Task<ICity> DeleteCityAsync(loginUserSessionDto usr, Guid id);
        public Task<ICity> UpdateCityAsync(loginUserSessionDto usr, csCityCUdto item);
        public Task<ICity> CreateCityAsync(loginUserSessionDto usr, csCityCUdto item);

        public Task<List<IUser>> ReadUsersAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize);
        public Task<IUser> ReadUserAsync(loginUserSessionDto usr, Guid id, bool flat);
        public Task<IUser> DeleteUserAsync(loginUserSessionDto usr, Guid id);
        public Task<IUser> UpdateUserAsync(loginUserSessionDto usr, csUserCUdto item);
        public Task<IUser> CreateUserAsync(loginUserSessionDto usr, csUserCUdto item);
    }
}

