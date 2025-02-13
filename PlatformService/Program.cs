using PlatformService.Data;
using PlatformService.Repositories;
using Microsoft.EntityFrameworkCore;
using PlatformService.MappingProfiles;
using PlatformService.AsyncDataServices;
using PlatformService.SyncDataServices.Http;
using PlatformService.SyncDataServices.Grpc;

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
            builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
            builder.Services.AddGrpc();

            if (builder.Environment.IsProduction())
            {
                Console.WriteLine("--> Using SqlServer Db");
                builder.Services.AddDbContext<AppDbContext>(opt =>
                {
                    opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn"));
                });
            }
            else
            {
                Console.WriteLine("--> Using InMem Db");
                builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
            }
            builder.Services.AddAutoMapper(typeof(PlatformProfile));
            var app = builder.Build();

            Console.WriteLine($"CommandService endpoint: {builder.Configuration["CommandService"]}");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
            app.MapGrpcService<GrpcPlatformService>();
            app.MapGet("Protos/platforms.proto", async context =>
            {
                await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
            });

            PreparationDb.PrepPopulation(app, builder.Environment);

            app.Run();
        }
    }
}