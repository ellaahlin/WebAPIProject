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
    public class csFriendsServiceModel : IFriendsService
    {
        private ILogger<csFriendsServiceModel> _logger = null;
        private object _locker = new object();

        #region only for layer verification
        private Guid _guid = Guid.NewGuid();
        private string _instanceHello;

        public string InstanceHello => _instanceHello;

        static public string Hello { get; } = $"Hello from namespace {nameof(Services)}, class {nameof(csFriendsServiceModel)}" +

            // added after project references is setup
            $"\n   - {csFriendDbRepos.Hello}" +
            $"\n   - {csMainDbContext.Hello}";
        #endregion

        #region constructors
        public csFriendsServiceModel(ILogger<csFriendsServiceModel> logger)
        {
            _logger = logger;

            //only for layer verification
            _instanceHello = $"Hello from class {this.GetType()} with instance Guid {_guid}. " +
                $"Will use ModelOnly, no repo.";

            _logger.LogInformation(_instanceHello);
        }
        #endregion

        private List<csFriend> _friends = new List<csFriend>();

        public Task<adminInfoDbDto> RemoveSeedAsync(loginUserSessionDto usr, bool seeded) => Task.Run(() =>
        {
            lock (_locker) { return RemoveSeed(usr, seeded); }
        });
        public adminInfoDbDto RemoveSeed(loginUserSessionDto usr, bool seeded)
            => throw new NotImplementedException();


        public Task<adminInfoDbDto> SeedAsync(loginUserSessionDto usr, int nrOfItems) => Task.Run(() =>
        {
            lock (_locker) { return Seed(usr, nrOfItems); }
        });
        public adminInfoDbDto Seed(loginUserSessionDto usr, int nrOfItems)
           => throw new NotImplementedException();

        //In order to make ReadAsync it has to return a deep copy of _friends.
        //Otherwise another Task could seed or removeseed on the list while first
        //Task is working on the list. Use copy constructor pattern
        public Task<List<IFriend>> ReadFriendsAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize) => Task.Run(() =>
        {
            lock (_locker) {

                //to create a a copy is simple using linq and copy constructor pattern
                var list = (_friends != null) ? _friends.Select(f => new csFriend(f)).ToList<IFriend>() : null;
                return list;
            }
        });
        public List<IFriend> ReadFriends(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize) => _friends.ToList<IFriend>();


        public Task<gstusrInfoAllDto> InfoAsync => Task.Run(() =>
        {
            lock (_locker) { return Info; }
        });
        public gstusrInfoAllDto Info
           => throw new NotImplementedException();


        #region not implemented
        public Task<IFriend> ReadFriendAsync(loginUserSessionDto usr, Guid id, bool flat) => throw new NotImplementedException();
        public Task<IFriend> DeleteFriendAsync(loginUserSessionDto usr, Guid id) => throw new NotImplementedException();
        public Task<IFriend> UpdateFriendAsync(loginUserSessionDto usr, csFriendCUdto item) => throw new NotImplementedException();
        public Task<IFriend> CreateFriendAsync(loginUserSessionDto usr, csFriendCUdto item) => throw new NotImplementedException();

        public Task<List<IAddress>> ReadAddressesAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize) => throw new NotImplementedException();
        public Task<IAddress> ReadAddressAsync(loginUserSessionDto usr, Guid id, bool flat) => throw new NotImplementedException();
        public Task<IAddress> DeleteAddressAsync(loginUserSessionDto usr, Guid id) => throw new NotImplementedException();
        public Task<IAddress> UpdateAddressAsync(loginUserSessionDto usr, csAddressCUdto item) => throw new NotImplementedException();
        public Task<IAddress> CreateAddressAsync(loginUserSessionDto usr, csAddressCUdto item) => throw new NotImplementedException();

        public Task<List<IQuote>> ReadQuotesAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize) => throw new NotImplementedException();
        public Task<IQuote> ReadQuoteAsync(loginUserSessionDto usr, Guid id, bool flat) => throw new NotImplementedException();
        public Task<IQuote> DeleteQuoteAsync(loginUserSessionDto usr, Guid id) => throw new NotImplementedException();
        public Task<IQuote> UpdateQuoteAsync(loginUserSessionDto usr, csQuoteCUdto item) => throw new NotImplementedException();
        public Task<IQuote> CreateQuoteAsync(loginUserSessionDto usr, csQuoteCUdto item) => throw new NotImplementedException();

        public Task<List<IPet>> ReadPetsAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize) => throw new NotImplementedException();
        public Task<IPet> ReadPetAsync(loginUserSessionDto usr, Guid id, bool flat) => throw new NotImplementedException();
        public Task<IPet> DeletePetAsync(loginUserSessionDto usr, Guid id) => throw new NotImplementedException();
        public Task<IPet> UpdatePetAsync(loginUserSessionDto usr, csPetCUdto item) => throw new NotImplementedException();
        public Task<IPet> CreatePetAsync(loginUserSessionDto usr, csPetCUdto item) => throw new NotImplementedException();
        #endregion
    }
}

