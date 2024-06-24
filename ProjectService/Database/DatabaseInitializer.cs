using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectService.Database;
using ProjectService.Entities;
using System.Linq;

namespace ProjectService
{
    public static class DatabaseInitializer
    {
        public static void SeedData(IServiceProvider serviceProvider)
        {
            using (var context = new DatabaseContext(
                serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()))
            {
                // Look for any existing data.
                if (context.VehicleMake.Any())
                {
                    return;   // Data was already seeded.
                }

                // Seed initial data.
                context.VehicleMake.AddRange(
                    new VehicleMake { Name = "Toyota", Abrv = "TYT" },
                    new VehicleMake { Name = "Ford", Abrv = "FRD" },
                    new VehicleMake { Name = "Chevrolet", Abrv = "CHV" },
                    new VehicleMake { Name = "Honda", Abrv = "HND" },
                    new VehicleMake { Name = "BMW", Abrv = "BMW" },
                    new VehicleMake { Name = "Mercedes-Benz", Abrv = "MBZ" },
                    new VehicleMake { Name = "Nissan", Abrv = "NSN" },
                    new VehicleMake { Name = "Audi", Abrv = "AUD" },
                    new VehicleMake { Name = "Hyundai", Abrv = "HYU" },
                    new VehicleMake { Name = "Volkswagen", Abrv = "VW" }
                );


                context.SaveChanges();
            }
        }
    }
}
