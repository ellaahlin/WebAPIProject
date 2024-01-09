using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using Configuration;
using Models;
using Models.DTO;

using DbModels;
using DbContext;
using DbRepos;
using Services;
using System.Linq;
using System.Collections.Generic;
using System.Net;

//Service namespace is an abstraction of using services without detailed knowledge
//of how the service is implemented.
//Service is used by the application layer using interfaces. Thus, the actual
//implementation of a service can be application dependent without changing code
//at application
namespace Services
{
    //IFriendsService ensures application layer can access csFriendsServiceModel
    //Friends model (without database) OR access csFriendsServiceDbRepos
    //FriendsDbM model (with database)class csFriendsServiceDbRepos without
    //without any code change
    public class csAttractionsServiceModel : IAttractionService
    {
        private csAttractionsServiceModel _repo = null;
        private ILogger<csAttractionsServiceModel> _logger = null;
        private object _locker = new object();

        #region only for layer verification
        private Guid _guid = Guid.NewGuid();
        private string _instanceHello;

        public string InstanceHello => _instanceHello;

        static public string Hello { get; } = $"Hello from namespace {nameof(Services)}, class {nameof(csAttractionsServiceModel)}" +

            // added after project references is setup
            $"\n   - {csAttractionsDbRepos.Hello}" +
            $"\n   - {csMainDbContext.Hello}";
        #endregion

        #region constructors
        public csAttractionsServiceModel(ILogger<csAttractionsServiceModel> logger)
        {
            _logger = logger;

            //only for layer verification
            _instanceHello = $"Hello from class {this.GetType()} with instance Guid {_guid}. " +
                $"Will use ModelOnly, no repo.";

            _logger.LogInformation(_instanceHello);
        }
        #endregion

        private List<csAttraction> _attractions = new List<csAttraction>();

        public Task<adminInfoDbDto> RemoveSeedAsync(loginUserSessionDto usr, bool seeded) => Task.Run(() =>
        {
            lock (_locker) { return RemoveSeed(usr, seeded); }
        });
        public adminInfoDbDto RemoveSeed(loginUserSessionDto usr, bool seeded)
        {
            //A bit of Linq refresh
            var _info = new adminInfoDbDto();
            _info.nrSeededAttractions = _attractions.Count(i => i.Seeded == seeded);
            _info.nrSeededComments = _attractions.Where(i => i.Seeded == seeded && i.Comments != null).Distinct().Count();
            _info.nrSeededCities = _attractions.Where(i => i.Seeded == seeded && i.City != null).Distinct().Count();

            //actually remove
            _attractions.RemoveAll(f => f.Seeded == seeded);

            return _info;
        }


        public Task<adminInfoDbDto> SeedAsync(loginUserSessionDto usr, int nrOfItems) => Task.Run(() =>
        {
            lock (_locker) { return Seed(usr, nrOfItems); }
        });
        public adminInfoDbDto Seed(loginUserSessionDto usr, int nrOfItems)
        {
            var _seeder = new csSeedGenerator();

            _attractions = _seeder.ToList<csAttraction>(nrOfItems);

            #region extending the seeding
            var _comments = _seeder.ToList<csComment>(nrOfItems);
            var _cities = _seeder.ToList<csCity>(nrOfItems);



            //Now _seededquotes is always the content of the Quotes table

            var _info = new adminInfoDbDto();
            _info.nrSeededAttractions = _attractions.Count();
            _info.nrSeededComments = _comments.Where(i => i.Comment != null).Distinct().Count();
            _info.nrSeededCities = _cities.Count();


            return _info;
        }

        //In order to make ReadAsync it has to return a deep copy of _friends.
        //Otherwise another Task could seed or removeseed on the list while first
        //Task is working on the list. Use copy constructor pattern
        public Task<List<IAttraction>> ReadAttractionsAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize, bool hasComment) => Task.Run(() =>
        {
            lock (_locker) {

                //to create a a copy is simple using linq and copy constructor pattern
                var list = (_attractions != null) ? _attractions.Select(a => new csAttraction(a)).ToList<IAttraction>() : null;
                return list;
            }
        });

        public Task<List<IAttraction>> ReadAttractionsWithCommentsAsync(loginUserSessionDto usr, bool seeded, string filter, int pageNumber, int pageSize, bool hasComment) => Task.Run(() =>
        {
            lock (_locker)
            {

                //to create a a copy is simple using linq and copy constructor pattern
                var list = (_attractions != null) ? _attractions.Select(a => new csAttraction(a)).ToList<IAttraction>() : null;
                return list;
            }
        });
        public List<IAttraction> ReadAttractions(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize) => _attractions.ToList<IAttraction>();


        public Task<gstusrInfoAllDto> InfoAsync => Task.Run(() =>
        {
            lock (_locker) { return Info; }
        });
        public gstusrInfoAllDto Info => new gstusrInfoAllDto
        {
            Db = new gstusrInfoDbDto
            {
                nrSeededAttractions = _attractions.Count(i => i.Seeded),
                nrUnseededAttractions = _attractions.Count(i => !i.Seeded),
                nrAttractionsWithComment = _attractions.Count(f => f.Comments == null),

                nrSeededComments = _attractions.Where(i => i.Seeded && i.Comments != null).Distinct().Count(),
                nrUnseededComments = _attractions.Where(i => !i.Seeded && i.Comments != null).Distinct().Count(),

                nrSeededCities = _attractions.Where(i => i.Seeded && i.City != null).Distinct().Count(),
                nrUnseededCities = _attractions.Where(i => !i.Seeded && i.City != null).Distinct().Count(),
            }
        };

        #region not implemented
        public Task<IAttraction> ReadAttractionAsync(loginUserSessionDto usr, Guid id, bool flat) => throw new NotImplementedException();
        public Task<IAttraction> DeleteAttractionAsync(loginUserSessionDto usr, Guid id) => throw new NotImplementedException();
        public Task<IAttraction> UpdateAttractionAsync(loginUserSessionDto usr, csAttractionCUdto item) => throw new NotImplementedException();
        public Task<IAttraction> CreateAttractionAsync(loginUserSessionDto usr, csAttractionCUdto item) => throw new NotImplementedException();

        public Task<List<IComment>> ReadCommentsAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize) => throw new NotImplementedException();
        public Task<IComment> ReadCommentAsync(loginUserSessionDto usr, Guid id, bool flat) => throw new NotImplementedException();
        public Task<List<IComment>> ReadCommentByUserAsync(loginUserSessionDto usr, Guid id, bool flat) => throw new NotImplementedException();
        public Task<IComment> DeleteCommentAsync(loginUserSessionDto usr, Guid id) => throw new NotImplementedException();
        public Task<IComment> UpdateCommentAsync(loginUserSessionDto usr, csCommentCUdto item) => throw new NotImplementedException();
        public Task<IComment> CreateCommentAsync(loginUserSessionDto usr, csCommentCUdto item) => throw new NotImplementedException();

        public Task<List<ICity>> ReadCitiesAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize) => throw new NotImplementedException();
        public Task<ICity> ReadCityAsync(loginUserSessionDto usr, Guid id, bool flat) => throw new NotImplementedException();
        public Task<ICity> DeleteCityAsync(loginUserSessionDto usr, Guid id) => throw new NotImplementedException();
        public Task<ICity> UpdateCityAsync(loginUserSessionDto usr, csCityCUdto item) => throw new NotImplementedException();
        public Task<ICity> CreateCityAsync(loginUserSessionDto usr, csCityCUdto item) => throw new NotImplementedException();

        public Task<List<IUser>> ReadUsersAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize) => throw new NotImplementedException();
        public Task<IUser> ReadUserAsync(loginUserSessionDto usr, Guid id, bool flat) => throw new NotImplementedException();
        public Task<IUser> DeleteUserAsync(loginUserSessionDto usr, Guid id) => throw new NotImplementedException();
        public Task<IUser> UpdateUserAsync(loginUserSessionDto usr, csUserCUdto item) => throw new NotImplementedException();
        public Task<IUser> CreateUserAsync(loginUserSessionDto usr, csUserCUdto item) => throw new NotImplementedException();
        #endregion
    }
}
#endregion