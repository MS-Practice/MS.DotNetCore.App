using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Globalization;

namespace ModelsValidation.Demo
{
    public class ClassicMovieAttribute : ValidationAttribute,IClientModelValidator
    {
        private int _year;

        public ClassicMovieAttribute(int Year)
        {
            _year = Year;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Movie movie = (Movie)validationContext.ObjectInstance;
            if (movie.Genre == Genre.Classic && movie.ReleaseDate.Year > _year)
                return new ValidationResult(GetErrorMessage());
            return ValidationResult.Success;
        }

        private string GetErrorMessage()
        {
            return $"Classic movies must have a release year earlier than {_year}.";
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-classicmovie", GetErrorMessage());
            var year = _year.ToString(CultureInfo.InvariantCulture);
            MergeAttribute(context.Attributes, "data-val-classicmovie-year", year);
        }

        private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
                return false;
            attributes.Add(key, value);
            return true;
        }

    }
}