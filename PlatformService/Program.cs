using PlatformService.Data;
using Microsoft.EntityFrameworkCore;
using PlatformService.Repositories;
using PlatformService.MappingProfiles;
using PlatformService.SyncDataServices.Http;

namespace PlatformService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
            builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
            builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
            builder.Services.AddAutoMapper(typeof(PlatformProfile));
            var app = builder.Build();

            Console.WriteLine($"CommandService endpoint: {builder.Configuration["CommandService"]}");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            PreparationDb.PrepPopulation(app);

            app.Run();
        }
    }
}