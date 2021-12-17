using Newtonsoft.Json;
using System;

namespace RestApiNet5.Data.DTOs
{
    public class DtoBase
    {
        [JsonProperty(Order = -100)]
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}