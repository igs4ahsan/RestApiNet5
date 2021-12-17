using RestApiNet5.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace RestApiNet5.Database.Seeders
{
    public static class UserSeeder
    {
        public static void Run(ModelBuilder modelBuilder)
        {
            var user = new User()
            {
                Name = "Ahsan Tariq",
                Email = "ahsantariq17130@gmail.com",
                Role = "Administrator"
            };

            user.SetId(Guid.NewGuid().ToString());
            user.SetHashPassword("password");
            user.TrackAndUpdateTimestamps();

            modelBuilder.Entity<User>().HasData(user);
        }
    }
}