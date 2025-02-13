using Microsoft.EntityFrameworkCore;

namespace PlatformService.Data
{
    public static class PreparationDb
    {
        public static void PrepPopulation(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var scopedServ = app.ApplicationServices.CreateScope())
            {
                SeedData(scopedServ.ServiceProvider.GetService<AppDbContext>() 
                    ?? throw new Exception("PreparationDb.PrepPopulation NullReferenceException!"), env);
            }
        }

        private static void SeedData(AppDbContext context, IWebHostEnvironment env)
        {
            if (env.IsProduction())
            {
                Console.WriteLine("--> Trying to apply migration...");
                try
                {
                    context.Database.Migrate();

                }
                catch (Exception e)
                {
                    Console.WriteLine($"--> Could not run migration: {e.Message}");
                }
            }
            if (!context.Platforms.Any())
            {
                Console.WriteLine("--> Seeding Data...");

                context.Platforms.AddRange(
                        new Models.Platform { Name="Dot Net", Publisher = "Microsoft", Cost = "Free"},
                        new Models.Platform { Name = "Postgre", Publisher = "PostgrePublisher", Cost = "Free" },
                        new Models.Platform { Name = "Kubernetes", Publisher = "Cloud Native", Cost = "Free" });

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}