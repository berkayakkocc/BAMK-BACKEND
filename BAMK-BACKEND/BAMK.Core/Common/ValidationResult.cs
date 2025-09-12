namespace BAMK.Core.Common
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<ValidationError> Errors { get; set; } = new();

        public ValidationResult()
        {
            IsValid = true;
        }

        public ValidationResult(List<ValidationError> errors)
        {
            IsValid = errors.Count == 0;
            Errors = errors;
        }

        public static ValidationResult Success()
        {
            return new ValidationResult();
        }

        public static ValidationResult Failure(List<ValidationError> errors)
        {
            return new ValidationResult(errors);
        }

        public static ValidationResult Failure(string propertyName, string errorMessage)
        {
            return new ValidationResult(new List<ValidationError>
            {
                new ValidationError(propertyName, errorMessage)
            });
        }

        public void AddError(string propertyName, string errorMessage)
        {
            Errors.Add(new ValidationError(propertyName, errorMessage));
            IsValid = false;
        }

        public void AddError(ValidationError error)
        {
            Errors.Add(error);
            IsValid = false;
        }
    }
}
