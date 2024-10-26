namespace PlatformService.Data
{
    public static class PreparationDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var scopedServ = app.ApplicationServices.CreateScope())
            {
                SeedData(scopedServ.ServiceProvider.GetService<AppDbContext>() 
                    ?? throw new Exception("PreparationDb.PrepPopulation NullReferenceException!"));
            }
        }

        private static void SeedData(AppDbContext context)
        {
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