﻿using Configuration;
using DbContext;
using DbRepos;
using Services;

var builder = WebApplication.CreateBuilder(args);

#region Insert standard WebApi services
// NOTE: global cors policy needed for JS and React frontends
builder.Services.AddCors();

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
#endregion
//Add the JWT token service
/* builder.Services.AddJwtTokenService();

//Swagger to support JWT
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
});
*/
#region Dependency Inject Custom logger
builder.Services.AddSingleton<ILoggerProvider, csInMemoryLoggerProvider>();
#endregion

#region Dependency Inject FriendsService
//Services are typically added as Scoped as one scope is a Web client request
//- Transient objects are always different in the IndexModel and in the middleware.
//- Scoped objects are the same for a given request but differ across each new request.
//- Singleton objects are the same for every request.

//DI injects the DbRepos into csFriendService
//Services are typically added as Scoped as one scope is a Web client request
builder.Services.AddScoped<csAttractionsDbRepos>();

//WebController have a matching constructor, so service must be created
//Services are typically added as Scoped as one scope is a Web client request
builder.Services.AddScoped<IAttractionService, csAttractionsServiceDb>();
#endregion

#region Dependency Inject LoginService
//LoginService, scoped is fine as I will use the JWT bearer token to keep track of logged user
builder.Services.AddScoped<ILoginService, csLoginService>();
builder.Services.AddScoped<csLoginDbRepos>();
#endregion

var app = builder.Build();

#region Configure the HTTP request pipeline
//In this example always use Swagger 
//if (app.Environment.IsDevelopment())
//{

app.UseSwagger();
app.UseSwaggerUI();

//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion