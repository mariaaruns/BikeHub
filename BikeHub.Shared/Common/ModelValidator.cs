
using System.ComponentModel.DataAnnotations;

namespace BikeHub.Shared.Common;

public static class ModelValidator
{
    public static (bool IsValid, List<ValidationResult> Errors) Validate<T>(T model)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(model, serviceProvider: null, items: null);

        bool isValid = Validator.TryValidateObject(model, context, validationResults, validateAllProperties: true);

        return (isValid, validationResults);
    }

}