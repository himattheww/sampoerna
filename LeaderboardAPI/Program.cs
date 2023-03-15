using LeaderboardAPI.Entities;
using LeaderboardAPI.Repositories.Data;
using LeaderboardAPI.Repositories.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Net;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Console.WriteLine("Find me 1 ");
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();

Console.WriteLine("Connection JSON is " + builder.Configuration.GetConnectionString("DefaultConnection"));

try{
	string user = Environment.GetEnvironmentVariable("DB_USERNAME", EnvironmentVariableTarget.Machine);
	string password = Environment.GetEnvironmentVariable("DB_PASSWORD", EnvironmentVariableTarget.Machine);

	Console.WriteLine("Find Me Koneksi Environment Variable 1 : \n 1. DB_USERNAME : " + user + "\n 2. DB_PASSWORD : "+ password);

    string user2 = Environment.GetEnvironmentVariable("DB_USERNAME");
    string password2 = Environment.GetEnvironmentVariable("DB_PASSWORD");

    Console.WriteLine("Find Me Koneksi Environment Variable 2 : \n 1. DB_USERNAME : " + user2 + "\n 2. DB_PASSWORD : " + password2);
}
catch (Exception e) {
	Console.WriteLine("error envi"); 
    Console.WriteLine("{0} Exception caught.", e);
}

try
    { 
		builder.Services.AddEntityFrameworkMySql().AddDbContext<LeaderboardContext>(options =>
		{
			options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")));
		});
	}
catch (MySqlException e)
{
    Console.WriteLine("error sql");
    Console.WriteLine("{0} Exception caught.", e);
    Console.WriteLine($"Can not open connection ! ErrorCode: {e.ErrorCode} Error: {e.Message}");
}
catch  (Exception e)
    {
		Console.WriteLine("error sql"); 
        Console.WriteLine("{0} Exception caught.", e);
}


builder.Services.AddScoped<IToken, TokenRepository>();
builder.Services.AddScoped<IEmployee, EmployeeRepository>();
builder.Services.AddScoped<IWholesellerMapping, WholesellerMappingRepository>();
builder.Services.AddScoped<IAdmin, AdminRepository>();

builder.Services.AddCors();

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AnotherPolicy",
//        policy =>
//        {
//            policy.WithOrigins("http://localhost:3000")
//             .SetIsOriginAllowedToAllowWildcardSubdomains()
//             .AllowAnyHeader()
//             .AllowAnyMethod();
//        });
//});

builder.Services.AddSwaggerGen();
Console.WriteLine("Find me 2 ");
// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
});
Console.WriteLine("Find me 3 ");
//JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
Console.WriteLine("Find me 4 ");
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseSwagger();
//app.UseSwaggerUI();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
Console.WriteLine("Find me 5 ");
//app.UseHttpsRedirection();

//Error Message Token
app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
    {
        await context.Response.WriteAsJsonAsync(new 
        {
            statusCode = HttpStatusCode.Unauthorized, 
            message = "your token is expired",
            data = ""
        });
    }
    else
    {
        if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
        {
            await context.Response.WriteAsJsonAsync(new
            {
                statusCode = HttpStatusCode.Forbidden,
                message = "your token is forbidden",
                data = ""
            });
        }
    }
});
Console.WriteLine("Find me 6 ");
// Authentication & Authorization
app.UseAuthentication();
Console.WriteLine("Find me 7 ");
app.UseAuthorization();
Console.WriteLine("Find me 8 ");
app.MapControllers();
Console.WriteLine("Find me 9 ");
app.Run();
