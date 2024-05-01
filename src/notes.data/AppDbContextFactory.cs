// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Design;
// using Microsoft.Extensions.Configuration;
// using notes.data.Settings;
// using System.IO;
//
// namespace notes.data
// {
//     public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
//     {
//         public AppDbContext CreateDbContext(string[] args)
//         {
//             var configuration = new ConfigurationBuilder()
//                 .SetBasePath(Directory.GetCurrentDirectory())
//                 .AddJsonFile("appsettings.json")
//                 .Build();
//
//             var appSettingsSection = configuration.GetSection("DbConnectionOptions");
//             var dbOptions = appSettingsSection.Get<DbConnectionOptions>();
//
//             var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
//             optionsBuilder.UseNpgsql(dbOptions.GetConnectionString());
//
//             return new AppDbContext(optionsBuilder.Options);
//         }
//     }
// }
