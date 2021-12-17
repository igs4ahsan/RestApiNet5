using Microsoft.EntityFrameworkCore;

namespace RestApiNet5.Database.Seeders
{
    public static class DatabaseSeeder
    {
        public static void RunDatabaseSeeder(this ModelBuilder modelBuilder)
        {
            UserSeeder.Run(modelBuilder);
        }
    }
}