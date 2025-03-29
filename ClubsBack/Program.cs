
using ClubsBack.Entities;
using ClubsBack.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using TwitterBackend;

namespace ClubsBack
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(Program).Assembly));
            IConfigurationSection authConfiguration = builder.Configuration.GetSection("AuthOptions");
            
            AuthOptions authOptions = new AuthOptions(
             authConfiguration["ISSUER"] ?? throw new Exception("ISSUER is null!"),
             authConfiguration["AUDIENCE"] ?? throw new Exception("AUDIENCE is null!"),
             authConfiguration["KEY"]) ?? throw new Exception("KEY is null!");
            builder.Services.AddSingleton<AuthOptions>(authOptions);

            
            builder.Services.AddTransient<JwtCreator>();
            builder.Services.AddControllers();

            string conn = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddSingleton<DBconnect>(new DBconnect(conn));
            builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            builder.Services.AddTransient<IRepository<Users>, UsersRepository>();
            builder.Services.AddTransient<IClubs, ClubsRepository>();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authOptions.Issuer, //???????? ????????
                    ValidateAudience = true,
                    ValidAudience = authOptions.Audience,//???????? ?????????
                    ValidateLifetime = true,
                    IssuerSigningKey = authOptions.GetSymmetricKey,
                    ValidateIssuerSigningKey = true
                };
            });
            builder.Services.AddAuthorization();
            var app = builder.Build();
            app.UseCors("MyPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            // Configure the HTTP request pipeline.
            app.MapControllers();

            app.Run();
        }
    }
}
