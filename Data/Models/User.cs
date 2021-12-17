using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestApiNet5.Data.Models
{
    public class User : ModelBase
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [JsonIgnore]
        public string HashPassword { get; private set; }

        [Required]
        public string Role { get; set; }

        public void SetHashPassword(string password)
        {
            HashPassword = BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}