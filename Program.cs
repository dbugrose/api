using System.Text;
using api.Services.Context;
using api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<FriendRequestsService>();
builder.Services.AddScoped<TodoService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<HealthService>();
builder.Services.AddScoped<StatsService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

var secretKey = builder.Configuration["JWT:Key"] ?? "superSecretKey@345superSecretKey@345";
var signingCredentials = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
// Add authentication services to the app
builder.Services.AddAuthentication(options =>
{
    // Set the default authentication scheme/ behaviour to JWT Bearer
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    // Set the default challenge scheme (what to use when authentication fails)
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Configure JWT Bearer authentication options
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Check if the token's issuer is valid
        ValidateAudience = true, // Check if the token's audience is valid
        ValidateLifetime = true, // Ensure the token hasn't expired
        ValidateIssuerSigningKey = true, // Check the token's signature is valid

        // The expected issuer (the API that created the token)
        ValidIssuer = "slaythemonster2526dor-ghhnbvgkercbd0gx.westus3-01.azurewebsites.net/",

        // The expected audience (who the token is intended for)
        ValidAudience = "slaythemonster2526dor-ghhnbvgkercbd0gx.westus3-01.azurewebsites.net/",

        // The key used to sign the token (must match the one used to create it)
        IssuerSigningKey = signingCredentials
    };
});


var connectionString = builder.Configuration.GetConnectionString("SlayTheMonster");
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();