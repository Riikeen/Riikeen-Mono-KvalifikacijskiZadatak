using Microsoft.EntityFrameworkCore;
using ProjectService;
using ProjectService.Automapper;
using ProjectService.Database;
using ProjectService.VehicleService;
using System.Configuration;

namespace Mono_testni_zadatak
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<DatabaseContext>(option =>
                option.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseContext")));
            builder.Services.AddScoped<IVehicleMakeService ,VehicleMakeService>();
            builder.Services.AddScoped<IVehicleModelService ,VehicleModelService>();
            builder.Services.AddAutoMapper(typeof(Automapper));


            var app = builder.Build();
            //automatic migrations at startup
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                dbContext.Database.Migrate();

                DatabaseInitializer.SeedData(scope.ServiceProvider);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=VehicleMake}/{action=Index}/{id?}");

            app.Run();
        }
    }
}