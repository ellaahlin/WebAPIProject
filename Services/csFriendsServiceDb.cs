using System;
using DbContext;
using DbRepos;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO;

namespace Services
{
    public class csAttractionsServiceDb : IAttractionService
    {
        private csAttractionsDbRepos _repo = null;
        private ILogger<csAttractionsServiceDb> _logger = null;

        #region only for layer verification
        private Guid _guid = Guid.NewGuid();
        private string _instanceHello;

        public string InstanceHello => _instanceHello;
 
        static public string Hello { get; } = $"Hello from namespace {nameof(Services)}, class {nameof(csAttractionsDbRepos)}" +

            // added after project references is setup
            $"\n   - {csAttractionsDbRepos.Hello}" +
            $"\n   - {csMainDbContext.Hello}";
        #endregion

        #region constructors
        public csAttractionsServiceDb(ILogger<csAttractionsServiceDb> logger)
        {
            //only for layer verification
            _instanceHello = $"Hello from class {this.GetType()} with instance Guid {_guid}. ";

            _logger = (ILogger<csAttractionsServiceDb>)logger;
            _logger.LogInformation(_instanceHello);
        }

        public csAttractionsServiceDb(csAttractionsDbRepos repo, ILogger<csAttractionsServiceDb> logger)
        {
            //only for layer verification
            _instanceHello = $"Hello from class {this.GetType()} with instance Guid {_guid}. " +
                $"Will use repo {repo.GetType()}.";

            _logger = logger;
            _logger.LogInformation(_instanceHello);

            _repo = repo;

        }
        #endregion

        #region Simple 1:1 calls in this case, but as Services expands, this will no longer be the case
        public Task<gstusrInfoAllDto> InfoAsync => _repo.InfoAsync();

        public Task<adminInfoDbDto> SeedAsync(loginUserSessionDto usr, int nrOfItems) => _repo.SeedAsync(usr, nrOfItems);
        public Task<adminInfoDbDto> RemoveSeedAsync(loginUserSessionDto usr, bool seeded) => _repo.RemoveSeedAsync(usr, seeded);

        public Task<List<IAttraction>> ReadAttractionsAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize, bool hasComment) => _repo.ReadAttractionsAsync(usr, seeded, flat, filter, pageNumber, pageSize, hasComment);
        public Task<List<IAttraction>> ReadAttractionsWithCommentsAsync(loginUserSessionDto usr, bool seeded, string filter, int pageNumber, int pageSize, bool hasComment) => _repo.ReadAttractionsWithCommentsAsync(usr, seeded, filter, pageNumber, pageSize, hasComment);

        public Task<IAttraction> ReadAttractionAsync(loginUserSessionDto usr, Guid id, bool flat) => _repo.ReadAttractionAsync(usr, id, flat);
        public Task<IAttraction> DeleteAttractionAsync(loginUserSessionDto usr, Guid id) => _repo.DeleteAttractionAsync(usr, id);
        public Task<IAttraction> UpdateAttractionAsync(loginUserSessionDto usr, csAttractionCUdto item) => _repo.UpdateAttractionAsync(usr, item);
        public Task<IAttraction> CreateAttractionAsync(loginUserSessionDto usr, csAttractionCUdto item) => _repo.CreateAttractionAsync(usr, item);

        public Task<List<IComment>> ReadCommentsAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize) => _repo.ReadCommentsAsync(usr, seeded, flat, filter, pageNumber, pageSize);
        public Task<IComment> ReadCommentAsync(loginUserSessionDto usr, Guid id, bool flat) => _repo.ReadCommentAsync(usr, id, flat);
        public Task<List<IComment>> ReadCommentByUserAsync(loginUserSessionDto usr, Guid id, bool flat) => _repo.ReadCommentByUserAsync(usr, id, flat);
        public Task<IComment> DeleteCommentAsync(loginUserSessionDto usr, Guid id) => _repo.DeleteCommentAsync(usr, id);
        public Task<IComment> UpdateCommentAsync(loginUserSessionDto usr, csCommentCUdto item) => _repo.UpdateCommentAsync(usr, item);
        public Task<IComment> CreateCommentAsync(loginUserSessionDto usr, csCommentCUdto item) => _repo.CreateCommentAsync(usr, item);

        public Task<List<ICity>> ReadCitiesAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize) => _repo.ReadCitiesAsync(usr, seeded, flat, filter, pageNumber, pageSize);
        public Task<ICity> ReadCityAsync(loginUserSessionDto usr, Guid id, bool flat) => _repo.ReadCityAsync(usr, id, flat);
        public Task<ICity> DeleteCityAsync(loginUserSessionDto usr, Guid id) => _repo.DeleteCityAsync(usr, id);
        public Task<ICity> UpdateCityAsync(loginUserSessionDto usr, csCityCUdto item) => _repo.UpdateCityAsync(usr, item);
        public Task<ICity> CreateCityAsync(loginUserSessionDto usr, csCityCUdto item) => _repo.CreateCityAsync(usr, item);

        public Task<List<IUser>> ReadUsersAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize) => _repo.ReadUsersAsync(usr, seeded, flat, filter, pageNumber, pageSize);
        public Task<IUser> ReadUserAsync(loginUserSessionDto usr, Guid id, bool flat) => _repo.ReadUserAsync(usr, id, flat);
        public Task<IUser> DeleteUserAsync(loginUserSessionDto usr, Guid id) => _repo.DeleteUserAsync(usr, id);
        public Task<IUser> UpdateUserAsync(loginUserSessionDto usr, csUserCUdto item) => _repo.UpdateUserAsync(usr, item);
        public Task<IUser> CreateUserAsync(loginUserSessionDto usr, csUserCUdto item) => _repo.CreateUserAsync(usr, item);
        #endregion

        #region The non-Async methods are not implemented using DbRepos
        public gstusrInfoAllDto Info => throw new NotImplementedException();

        public adminInfoDbDto Seed(loginUserSessionDto usr, int nrOfItems) => throw new NotImplementedException();
        public adminInfoDbDto RemoveSeed(loginUserSessionDto usr, bool seeded) => throw new NotImplementedException();

        public List<IAttraction> ReadAttractions(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize) => throw new NotImplementedException();
        #endregion
    }
}
