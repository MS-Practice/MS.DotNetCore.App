using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

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
            throw new NotImplementedException();
        }
    }
}