namespace CarRentingSystem.Infrastrucutre
{
    using CarRentingSystem.Data;
    using CarRentingSystem.Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using static WebConstants;
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);
            SeedCarCategories(services);
            SeedAdministrator(services);

            return app;
        }
        private static void MigrateDatabase(IServiceProvider service)
        {
            var data = service.GetRequiredService<CarRentingDbContext>();
            data.Database.Migrate();
        }

        private static void SeedCarCategories(IServiceProvider service)
        {
            var data = service.GetRequiredService<CarRentingDbContext>();

            if (data.Categories.Any())
            {
                return;
            }
            data.Categories.AddRange(new[]
            {
                new Category{Name = "Sport"},
                new Category{Name = "Economy"},
                new Category{Name = "Electric"},
                new Category{Name = "Mid Size"},
                new Category{Name = "Luxury"},
                new Category{Name = "Van"},
                new Category{Name = "Suv"},
            });
            data.SaveChanges();
        }
        private static void SeedAdministrator(IServiceProvider service)
        {
            var userManager = service.GetRequiredService<UserManager<User>>();
            var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
            Task.Run(async () =>
            {
                if(await roleManager.RoleExistsAsync(AdministratorRoleName))
                {
                    return;
                }
                var role = new IdentityRole { Name = AdministratorRoleName };
                await roleManager.CreateAsync(role);

                var adminEmail = "Admin@crs.com";
                var adminFullName = "Veselin Valkanov";
                var adminPassowr = "123456";

                var user = new User
                {
                    Email = adminEmail,
                    UserName = adminEmail,
                    FullName = adminFullName,
                };
                await userManager.CreateAsync(user, adminPassowr);
                await userManager.AddToRoleAsync(user, role.Name);
            })
                .GetAwaiter()
                .GetResult();
        }
    }
}
