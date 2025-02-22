using CommandsService.Data;
using Microsoft.EntityFrameworkCore;
using CommandsService.EventProcessing;
using CommandsService.AsyncDataServices;
using CommandsService.SyncDataServices.Grpc;

namespace CommandsService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddScoped<ICommandRepo, CommandRepo>();
            builder.Services.AddScoped<IPlatformDataClient, PlatformDataClient>();
            builder.Services.AddSingleton<IEventProcessor, EventProcessor>();

            builder.Services.AddControllers();
            builder.Services.AddHostedService<MessageBusSubscriber>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            PrepDb.PrepPopulation(app);

            app.Run();
        }
    }
}