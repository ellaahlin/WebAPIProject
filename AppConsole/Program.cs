using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;


using Configuration;
using Models;
using Models.DTO;
using Services;

using DbContext;       
using DbModels;           
using DbRepos;
using Microsoft.Extensions.Logging;
using System.Data.Common;

//ConsoleAPP namespace is the top layer in the stack and contains the business
//, i.e. application logic. Using the other layers this layer can easily
//be switched from one type of application to another

//Once all the layers are setup with its own references. ConsoleApp will
//depend ONLY on Configuration, DbModels and Services.
//This allows the Application to be independed from any database implementation
namespace ConsoleApp;

class Program
{
    //used for seeding
    const int _nrSeeds = 1000;
    const int _nrUsers = 40;
    const int _nrSuperUsers = 40;

    //used when huge nr of data, read pages of _readerPageSize items, instead of all items
    const int _readerPageSize = 1000;

    static async Task Main(string[] args)
    {
        //Allows a Console App to use .NET Dependecy Injection pattern,
        //by runnign the App within a host
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        #region Dependency Injection of Logger
        //Add your own Services to use with DI
        builder.Services.AddSingleton<ILoggerProvider, csInMemoryLoggerProvider>();
        #endregion

        #region Dependency Injection
        //DI injects the DbRepos into csFriendService
        builder.Services.AddScoped<csAttractionsDbRepos>();

        //builder.Services.AddSingleton<IFriendsService, csFriendsServiceModel>();
        builder.Services.AddScoped<IAttractionService, csAttractionsServiceDb>();
        #endregion


        //Build the host
        using IHost host = builder.Build();

        #region To be removed for the real application
        Verification(host);
        #endregion

        #region seeding the model
        var attractionService = host.Services.CreateScope()
            .ServiceProvider.GetRequiredService<IAttractionService>();

        await AttractionServiceSnapshot(attractionService);
        await AttractionServiceInfo(attractionService);
        #endregion

        //Terminate the host and the Application properly
        await host.RunAsync();

        //Terminate the host and the Application properly
        await host.RunAsync();
    }

    #region used for basic verificaton
    private static void Verification(IHost host)
    {
        Console.WriteLine("Verification start");
        Console.WriteLine("------------------");

        //to verify the layers are accessible
        Console.WriteLine("\nLayer access:");
        Console.WriteLine(csAppConfig.Hello);
        Console.WriteLine(csAttraction.Hello);

        Console.WriteLine(csAttractionDbM.Hello);
        Console.WriteLine(csMainDbContext.Hello);
        Console.WriteLine(csAttractionsDbRepos.Hello);

        Console.WriteLine(csLoginService.Hello);
        Console.WriteLine(csJWTService.Hello);
        Console.WriteLine(csAttractionsServiceModel.Hello);
        Console.WriteLine(csAttractionsServiceDb.Hello);


        //to verify connection strings can be read from appsettings.json
        Console.WriteLine($"\nDbConnections:\nDbLocation: {csAppConfig.DbSetActive.DbLocation}" +
            $"\nDbServer: {csAppConfig.DbSetActive.DbServer}");
        Console.WriteLine("DbUserLogins in DbSet:");
        foreach (var item in csAppConfig.DbSetActive.DbLogins)
        {
            Console.WriteLine($"   DbUserLogin: {item.DbUserLogin}" +
                              $"\n   DbConnection: {item.DbConnection}\n   ConString: <secret>");
        }

        //to verify usersecret access
        Console.WriteLine($"\nUser secrets:\n{csAppConfig.SecretMessage}");

        Console.WriteLine($"\nDependency Injection:");
        Console.WriteLine($"Service Scope 1:");
        using (IServiceScope serviceScope = host.Services.CreateAsyncScope())
        {
            IServiceProvider provider = serviceScope.ServiceProvider;

            IAttractionService attractionsService1 = provider.GetRequiredService<IAttractionService>();
            Console.WriteLine($"\nService instance 1:\n{attractionsService1.InstanceHello}\n");

            IAttractionService attractionsService2 = provider.GetRequiredService<IAttractionService>();
            Console.WriteLine($"\nServices instance 2:\n{attractionsService2.InstanceHello}\n");
        }

        Console.WriteLine($"\nService Scope 2:");
        using (IServiceScope serviceScope = host.Services.CreateAsyncScope())
        {
            IServiceProvider provider = serviceScope.ServiceProvider;

            IAttractionService attractionsService1 = provider.GetRequiredService<IAttractionService>();
            Console.WriteLine($"\nService instance  1:\n{attractionsService1.InstanceHello}\n");

            IAttractionService attractionsService2 = provider.GetRequiredService<IAttractionService>();
            Console.WriteLine($"\nServices instance 2:\n{attractionsService2.InstanceHello}\n");
        }

        Console.WriteLine($"\nCustomer Logger, InMemoryLoggerProvider:");
        var customLoggerService = host.Services.CreateScope()
                    .ServiceProvider.GetRequiredService<ILoggerProvider>();
        foreach (var item in ((csInMemoryLoggerProvider)customLoggerService).Messages)
        {
            Console.WriteLine($"  -- {item}\n");
        }

        Console.WriteLine("\nVerification end");
        Console.WriteLine("------------------\n\n");
        Console.ReadKey();
    }
    #endregion

    #region used when seeding of model IFriend, IAddress, IPet, IQuote
    private static async Task AttractionServiceSnapshot(IAttractionService attractionService)
    {

        loginUserSessionDto _usr = new loginUserSessionDto { UserRole = "sysadmin" };

        var _info = await attractionService.RemoveSeedAsync(_usr, true);
        Console.WriteLine($"\n{_info.nrSeededAttractions} attractions removed");

        _info = await attractionService.SeedAsync(_usr, _nrSeeds);
        Console.WriteLine($"{_info.nrSeededAttractions} attractions seeded");

        var _list = await attractionService.ReadAttractionsAsync(_usr, true, false, null, 0, _readerPageSize);
        Console.WriteLine("\nFirst 5 attractions");
        _list.Take(5).ToList().ForEach(f => Console.WriteLine(f));

        Console.WriteLine("\nLast 5 attractions");
        _list.TakeLast(5).ToList().ForEach(f => Console.WriteLine(f));
    }

    private static async Task AttractionServiceInfo(IAttractionService attractionService)
    {
        var info = await attractionService.InfoAsync;
        Console.WriteLine($"\nAttractionServiceInfo:");
        Console.WriteLine($"Nr of seeded attractions: {info.Db.nrSeededAttractions}");
        Console.WriteLine($"Nr of unseeded attractions: {info.Db.nrUnseededAttractions}");
        Console.WriteLine($"Nr of attractions with comments: {info.Db.nrAttractionsWithComment}");

        Console.WriteLine($"Nr of comments: {info.Db.nrSeededComments}");
        Console.WriteLine($"Nr of unseeded comments: {info.Db.nrUnseededComments}");

        Console.WriteLine($"Nr of cities: {info.Db.nrSeededCities}");
        Console.WriteLine($"Nr of unseeded cities: {info.Db.nrSeededCities}");

        Console.WriteLine($"Nr of titles: {info.Db.nrSeededTitles}");
        Console.WriteLine($"Nr of unseeded titles: {info.Db.nrUnseededTitles}");
        Console.WriteLine();
    }
    #endregion

    #region used when seeding of model IUser
    private static async Task LoginServiceSnapshot(ILoginService loginService)
    {
        var _info = await loginService.SeedAsync(_nrUsers, _nrSuperUsers);
        Console.WriteLine($"{_info.NrUsers} users seeded");
        Console.WriteLine($"{_info.NrSuperUsers} superusers seeded");
    }
    #endregion

    #region used for login
    private static async Task LoginServiceLogin(ILoginService loginService)
    {
        var _usrCreds = new loginCredentialsDto { UserNameOrEmail = "user1", Password = "user1" };

        try
        {
            var _usr = await loginService.LoginUserAsync(_usrCreds);
            Console.WriteLine($"\n{_usr.UserName} logged in");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    #endregion
}

