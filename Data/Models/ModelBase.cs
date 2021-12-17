using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RestApiNet5.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json;

namespace RestApiNet5.Data.Models
{
    public abstract class ModelBase
    {
        [ValidateNever]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty(Order = -100)]
        public string Id { get; private set; }

        public DateTime CreatedAt { get; private set; }
 
        public DateTime UpdatedAt { get; private set; }

        public void TrackAndUpdateTimestamps()
        {
            if (CreatedAt == default)
            {
                CreatedAt = DateTime.Now;
            }

            UpdatedAt = DateTime.Now;

            Validate();
        }

        public void SetId(string id)
        {
            Id = id;
        }

        public void CloneIdentity(ModelBase old)
        {
            this.SetId(old.Id);
            this.CreatedAt = old.CreatedAt;
            this.UpdatedAt = old.UpdatedAt;
        }

        public bool IsValid(out ICollection<ValidationResult> errors)
        {
            errors = new List<ValidationResult>();
            var context = new ValidationContext(this, serviceProvider: null, items: null);
            return Validator.TryValidateObject(this, context, errors, validateAllProperties: true);
        }

        public void Validate()
        {
            if (!this.IsValid(out var errors))
            {
                throw new DbRecordValidationFailedException(errors);
            }
        }
    }
}
