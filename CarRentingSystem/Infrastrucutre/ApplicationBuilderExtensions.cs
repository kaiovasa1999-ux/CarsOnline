namespace CarRentingSystem.Infrastrucutre
{
    using CarRentingSystem.Data;
    using CarRentingSystem.Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq;
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepDatabase(this IApplicationBuilder app)
        {
            using var scopedService = app.ApplicationServices.CreateScope();
            var data = scopedService.ServiceProvider.GetService<CarRentingDbContext>();
            data.Database.Migrate();
            SeedCarCategories(data);
            return app;
        }

        private static void SeedCarCategories(CarRentingDbContext data)
        {
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
    }
}
