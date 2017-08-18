using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomModelBindingSample.Validators
{
    public class ClassicMovieAttribute : ValidationAttribute, IClientModelValidator
    {
        private int _year;
        public ClassicMovieAttribute(int Year)
        {
            _year = Year;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //your business logic code
            return ValidationResult.Success;
        }
        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            MergeAttribute(context.Attributes, "data-val", "true");
        }

        private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }

            attributes.Add(key, value);
            return true;
        }

        private string GetErrorMessage()
        {
            return "Classic movies must have a release year earlier than " + _year;
        }
    }
}
