
using ClubsBack.Entities;
using ClubsBack.Repository;
using Microsoft.Extensions.Hosting;
using TwitterBackend;

namespace ClubsBack
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(Program).Assembly));

            var key = builder.Configuration["AuthOptions:Key"];
            var issuer = builder.Configuration["AuthOptions:Issuer"];
            var audience = builder.Configuration["AuthOptions:Audience"];
            
            builder.Services.AddTransient<AuthOptions>((_)=>new AuthOptions(issuer, audience, key));
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
            builder.Services.AddTransient<IClubs<Clubs>, ClubsRepository>();


            var app = builder.Build();
            app.UseCors("MyPolicy");
            // Configure the HTTP request pipeline.
            app.MapControllers();

            app.Run();
        }
    }
}
