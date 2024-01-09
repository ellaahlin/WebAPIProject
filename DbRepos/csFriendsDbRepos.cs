using Configuration;
using Models;
using Models.DTO;
using DbModels;
using DbContext;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata;
using System.Net;

//DbRepos namespace is a layer to abstract the detailed plumming of
//retrieveing and modifying and data in the database using EFC.

//DbRepos implements database CRUD functionality using the DbContext
namespace DbRepos;

public class csAttractionsDbRepos
{
    private ILogger<csAttractionsDbRepos> _logger = null;
    // private csAttractionsDbRepos _repos = null;

    #region used before csLoginService is implemented
    private string _dblogin = "sysadmin";
    //private string _dblogin = "gstusr";
    //private string _dblogin = "usr";
    //private string _dblogin = "supusr";
    #endregion


    #region only for layer verification
    private Guid _guid = Guid.NewGuid();
    private string _instanceHello = null;

    static public string Hello { get; } = $"Hello from namespace {nameof(DbRepos)}, class {nameof(csAttractionsDbRepos)}";
    public string InstanceHello => _instanceHello;
    #endregion


    #region contructors
    public csAttractionsDbRepos()
    {
        _instanceHello = $"Hello from class {this.GetType()} with instance Guid {_guid}.";
    }
    public csAttractionsDbRepos(ILogger<csAttractionsDbRepos> logger):this()
    {
        _logger = logger;
        _logger.LogInformation(_instanceHello);
    }
    #endregion


    #region Admin repo methods
    //implementation using View
    public async Task<gstusrInfoAllDto> InfoAsync()
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            var _info = new gstusrInfoAllDto
            {
                Db = new gstusrInfoDbDto
                {
                    #region full seeding
                    nrSeededAttractions = await db.Attractions.Where(f => f.Seeded).CountAsync(),
                    nrUnseededAttractions = await db.Attractions.Where(f => !f.Seeded).CountAsync(),
                    nrAttractionsWithComment = await db.Attractions.Where(f => f.Comments != null).CountAsync(),

                    nrSeededCities = await db.Cities.Where(f => f.Seeded).CountAsync(),
                    nrUnseededCities = await db.Cities.Where(f => !f.Seeded).CountAsync(),

                    nrSeededComments = await db.Comments.Where(f => f.Seeded).CountAsync(),
                    nrUnseededComments = await db.Comments.Where(f => !f.Seeded).CountAsync(),

                    nrSeededUsers = await db.Users.Where(f => f.Seeded).CountAsync(),
                    nrUnseededUsers = await db.Users.Where(f => !f.Seeded).CountAsync(),
                    #endregion
                }
            };

            return _info;
        }
    }

    public async Task<adminInfoDbDto> SeedAsync(loginUserSessionDto usr, int nrOfItems)
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            var _seeder = new csSeedGenerator();

            var ucount = await db.Users.CountAsync(u => u.Seeded);
            if (ucount == 0)
            {
                //Start by generating the quotes table
                var _users = _seeder.ToList<csUserDbM>(nrOfItems);
                //First delete all existing users
                foreach (var u in db.Users)
                    db.Users.Remove(u);

                //add users
                for (int i = 1; i <= nrOfItems; i++)
                {
                    db.Users.Add(new csUserDbM
                    {
                        UserId = Guid.NewGuid(),
                        FirstName = _seeder.FirstName,
                        LastName = _seeder.LastName,
                        Email = _seeder.Email()
                    });
                }
                await db.SaveChangesAsync();
            }


            #region full seeding
            //Now _seededquotes is always the content of the Quotes table*/
            var _seededusers = await db.Users.ToListAsync();

            //Generate attractions and cities
            var _attractions = _seeder.ToList<csAttractionDbM>(nrOfItems);

            var _existingcities = await db.Cities.ToListAsync();
            var _cities = _seeder.ToListUnique<csCityDbM>(nrOfItems, _existingcities);

            for (int c = 0; c < nrOfItems; c++)
            {
                _attractions[c].CityDbM = (_seeder.Bool) ? _seeder.FromList(_cities) : _cities.FirstOrDefault();

                var _comments = new List<csCommentDbM>();
                for (int i = 0; i < _seeder.Next(0, 21); i++)
                {
                    //A Pet can only be owned by one friend
                    _comments.Add(new csCommentDbM().Seed(_seeder));
                }
                _attractions[c].CommentsDbM = _comments;
                _seededusers[c].CommentsDbM = _comments;
            }

            //Add the seeded items to EFC, ChangeTracker will now pick it up
            foreach (var f in _attractions)
            {
                db.Attractions.Add(f);
            }
            #endregion


            var _info = new adminInfoDbDto();

            #region full seed
            _info.nrSeededAttractions = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csAttractionDbM) && entry.State == EntityState.Added);
            _info.nrSeededCities = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csCityDbM) && entry.State == EntityState.Added);
            _info.nrSeededComments = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csCommentDbM) && entry.State == EntityState.Added);
            #endregion

            #region full seed
            await db.SaveChangesAsync();
            #endregion

            return _info;
        }
    }


    public async Task<adminInfoDbDto> RemoveSeedAsync(loginUserSessionDto usr, bool seeded)
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            #region full seeding
            db.Attractions.RemoveRange(db.Attractions.Where(f => f.Seeded == seeded));

            //db.Pets.RemoveRange(db.Pets.Where(f => f.Seeded == seeded)); //not needed when cascade delete

            db.Cities.RemoveRange(db.Cities.Where(f => f.Seeded == seeded));
            #endregion

            db.Comments.RemoveRange(db.Comments.Where(f => f.Seeded == seeded));

            var _info = new adminInfoDbDto();
            if (seeded)
            {
                //Explore the changeTrackerNr of items to be deleted

                #region full seeding
                _info.nrSeededAttractions = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csAttractionDbM) && entry.State == EntityState.Deleted);
                _info.nrSeededCities = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csCityDbM) && entry.State == EntityState.Deleted);
                _info.nrSeededComments = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csCommentDbM) && entry.State == EntityState.Deleted);
                #endregion
            }
            else
            {
                //Explore the changeTrackerNr of items to be deleted
                #region full seeding
                _info.nrUnseededAttractions = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csAttractionDbM) && entry.State == EntityState.Deleted);
                _info.nrUnseededCities = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csCityDbM) && entry.State == EntityState.Deleted);
                _info.nrUnseededComments = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csCommentDbM) && entry.State == EntityState.Deleted);
                #endregion

            }

            await db.SaveChangesAsync();
            return _info;
        }
    }

    public async Task<adminInfoDbDto> SeedUsersAsync(loginUserSessionDto usr, int nrOfItems)
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            var _seeder = new csSeedGenerator();

            //Generate attractions and cities
            var _users = _seeder.ToList<csUserDbM>(nrOfItems);

            var _info = new adminInfoDbDto();

            #region full seed
            _info.nrSeededUsers = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csUserDbM) && entry.State == EntityState.Added);
            #endregion

            #region full seed
            await db.SaveChangesAsync();
            #endregion

            return _info;
        }
    }

    #region Attractions repo methods
    public async Task<IAttraction> ReadAttractionAsync(loginUserSessionDto usr, Guid id, bool flat)
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            if (!flat)
            {
                //make sure the model is fully populated, try without include.
                //remove tracking for all read operations for performance and to avoid recursion/circular access
                var _query = db.Attractions.AsNoTracking().Include(i => i.CityDbM).Include(i => i.CommentsDbM)
                    .Where(i => i.AttractionID == id);

                return await _query.FirstOrDefaultAsync<IAttraction>();
            }
            else
            {
                //Not fully populated, compare the SQL Statements generated
                //remove tracking for all read operations for performance and to avoid recursion/circular access
                var _query = db.Attractions.AsNoTracking()
                    .Where(i => i.AttractionID == id);

                return await _query.FirstOrDefaultAsync<IAttraction>();
            }
        }
    }

    public async Task<List<IAttraction>> ReadAttractionsAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize, bool hasComment)
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            filter ??= "";
            if (!flat)
            {
                //make sure the model is fully populated, try without include.
                //remove tracking for all read operations for performance and to avoid recursion/circular access
                if (!hasComment) {
                    var _query = db.Attractions.AsNoTracking().Include(i => i.CityDbM);

                    return await _query

                        //Adding filter functionality
                        .Where(i => i.Seeded == seeded)

                        //Adding paging
                        .Skip(pageNumber * pageSize)
                        .Take(pageSize)

                        .ToListAsync<IAttraction>();
                }
                else
                {
                    var _query = db.Attractions.AsNoTracking().Include(i => i.CityDbM).Include(i => i.CommentsDbM);

                    return await _query

                        //Adding filter functionality
                        .Where(i => i.Seeded == seeded)

                        //Adding paging
                        .Skip(pageNumber * pageSize)
                        .Take(pageSize)

                        .ToListAsync<IAttraction>();
                }
            }
            else
            {
                //Not fully populated, compare the SQL Statements generated
                //remove tracking for all read operations for performance and to avoid recursion/circular access
                var _query = db.Attractions.AsNoTracking();

                return await _query

                    //Adding filter functionality
                    .Where(i => i.Seeded == seeded)

                    //Adding paging
                    .Skip(pageNumber * pageSize)
                    .Take(pageSize)

                    .ToListAsync<IAttraction>();
            }
        }

    }

    public async Task<List<IAttraction>> ReadAttractionsWithCommentsAsync(loginUserSessionDto usr, bool seeded, string filter, int pageNumber, int pageSize, bool hasComment)
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            filter ??= "";
            var query = db.Attractions.AsNoTracking();

            if (hasComment)
            {
                query = query.Include(i => i.CommentsDbM).Where(i => i.CommentsDbM.Any());
            }
            else
            {
                query = query.Include(i => i.CommentsDbM).Where(i => !i.CommentsDbM.Any());
            }

            query = query.Where(i => i.Seeded == seeded)
                         .Skip(pageNumber * pageSize)
                         .Take(pageSize);

            return await query.ToListAsync<IAttraction>();
        }
    }


    public async Task<IAttraction> DeleteAttractionAsync(loginUserSessionDto usr, Guid id)
       => throw new NotImplementedException();

    public async Task<IAttraction> UpdateAttractionAsync(loginUserSessionDto usr, csAttractionCUdto itemDto)
       => throw new NotImplementedException();

    public async Task<IAttraction> CreateAttractionAsync(loginUserSessionDto usr, csAttractionCUdto itemDto)
       => throw new NotImplementedException();
    #endregion


    #region Comment repo methods
    public async Task<IComment> ReadCommentAsync(loginUserSessionDto usr, Guid id, bool flat)
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            if (!flat)
            {
                //make sure the model is fully populated, try without include.
                //remove tracking for all read operations for performance and to avoid recursion/circular access
                var _query = db.Comments.AsNoTracking().Include(i => i.UserDbM).Include(i => i.AttractionDbM)
                    .Where(i => i.CommentID == id);

                return await _query.FirstOrDefaultAsync<IComment>();
            }
            else
            {
                //Not fully populated, compare the SQL Statements generated
                //remove tracking for all read operations for performance and to avoid recursion/circular access
                var _query = db.Comments.AsNoTracking()
                    .Where(i => i.CommentID == id);

                return await _query.FirstOrDefaultAsync<IComment>();
            }
        }
    }

    public async Task<List<IComment>> ReadCommentByUserAsync(loginUserSessionDto usr, Guid id, bool flat)
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            if (!flat)
            {
                //make sure the model is fully populated, try without include.
                //remove tracking for all read operations for performance and to avoid recursion/circular access
                var _query = db.Comments.AsNoTracking().Include(i => i.UserDbM).Include(i => i.AttractionDbM)
                    .Where(i => i.UserId == id);

                return await _query.ToListAsync<IComment>();
            }
            else
            {
                //Not fully populated, compare the SQL Statements generated
                //remove tracking for all read operations for performance and to avoid recursion/circular access
                var _query = db.Comments.AsNoTracking()
                    .Where(i => i.UserId == id);

                return await _query.ToListAsync<IComment>();
            }
        }
    }

    public async Task<List<IComment>> ReadCommentsAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            //remove tracking for all read operations for performance and to avoid recursion/circular access
            var _query = db.Comments.AsNoTracking();

            return await _query.ToListAsync<IComment>();
        }
    }

    public async Task<IComment> DeleteCommentAsync(loginUserSessionDto usr, Guid id)
       => throw new NotImplementedException();

    public async Task<IComment> UpdateCommentAsync(loginUserSessionDto usr, csCommentCUdto itemDto)
       => throw new NotImplementedException();

    public async Task<IComment> CreateCommentAsync(loginUserSessionDto usr, csCommentCUdto itemDto)
       => throw new NotImplementedException();
    #endregion


    #region City repo methods
    public async Task<ICity> ReadCityAsync(loginUserSessionDto usr, Guid id, bool flat)
       => throw new NotImplementedException();

    public async Task<List<ICity>> ReadCitiesAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            //remove tracking for all read operations for performance and to avoid recursion/circular access
            var _query = db.Cities.AsNoTracking();

            return await _query.ToListAsync<ICity>();
        }
    }

    public async Task<ICity> DeleteCityAsync(loginUserSessionDto usr, Guid id)
       => throw new NotImplementedException();

    public async Task<ICity> UpdateCityAsync(loginUserSessionDto usr, csCityCUdto itemDto)
       => throw new NotImplementedException();

    public async Task<ICity> CreateCityAsync(loginUserSessionDto usr, csCityCUdto itemDto)
       => throw new NotImplementedException();
    #endregion

    #region User repo methods
    public async Task<IUser> ReadUserAsync(loginUserSessionDto usr, Guid id, bool flat)
       => throw new NotImplementedException();

    public async Task<List<IUser>> ReadUsersAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            //remove tracking for all read operations for performance and to avoid recursion/circular access
            var _query = db.Users.AsNoTracking();

            return await _query.ToListAsync<IUser>();
        }
    }

    public async Task<IUser> DeleteUserAsync(loginUserSessionDto usr, Guid id)
       => throw new NotImplementedException();

    public async Task<IUser> UpdateUserAsync(loginUserSessionDto usr, csUserCUdto itemDto)
       => throw new NotImplementedException();

    public async Task<IUser> CreateUserAsync(loginUserSessionDto usr, csUserCUdto itemDto)
       => throw new NotImplementedException();
    #endregion
}
#endregion