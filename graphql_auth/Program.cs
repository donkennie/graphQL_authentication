using graphql_auth.Repositories;
using graphql_auth.Types;
using graphql_auth.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;


string AllowedOrigin = "allowedOrigin";

var builder = WebApplication.CreateBuilder(args);

// configure strongly typed settings object
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(builder.Configuration.GetConnectionString("MongoDb")));
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddGraphQLServer()
        .AddAuthorization()
        //for inmemory subscription
        .AddInMemorySubscriptions()
        .AddQueryType<Query>()
        .AddMutationType<Mutations>()
        .AddSubscriptionType<Subscription>()
        .AddGlobalObjectIdentification()
// Registers the filter convention of MongoDB
 .AddMongoDbFiltering()
    //Registers the sorting convention of MongoDB
  .AddMongoDbSorting()
// Registers the projection convention of MongoDB
 .AddMongoDbProjections()
// Registers the paging providers of MongoDB
.AddMongoDbPagingProviders();


// CORS
builder.Services.AddCors(option =>
{
    option.AddPolicy(AllowedOrigin, builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});


//var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));

builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
               // var tokenSettings = builder.Services.Configure<JwtSettings>(options =>Configuration.GetSection("JwtSettings").Bind(options));
                var tokenSettings = builder.Configuration
                .GetSection("JwtSettings").Get<JwtSettings>();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                   // ValidateIssuerSigningKey = true,
                   // IssuerSigningKey = "key",
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("19032813938202")),
                    /*ValidIssuer = tokenSettings.Issuer,
                    ValidateIssuer = true,
                    ValidAudience = tokenSettings.Audience,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,*/
                };
            });

builder.Services
    .AddAuthorization(options =>
    {
        options.AddPolicy("roles-policy", policy =>
        {
            policy.RequireRole(new string[] { "admin", "super-admin" });
        });
        options.AddPolicy("claim-policy-1", policy =>
        {
            policy.RequireClaim("LastName");
        });
        options.AddPolicy("claim-policy-2", policy =>
        {
            policy.RequireClaim("LastName", new string[] { "Bommidi", "Test" });
        });
    });



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseAuthentication();
app.UseAuthorization();
app.UseCors(AllowedOrigin);
app.UseWebSockets();
app.MapGraphQL();
app.Run();
