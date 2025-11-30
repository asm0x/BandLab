using System.ComponentModel.DataAnnotations;

namespace BandLab.API.Models;

public record CreateCommentModel([property: Required] string Content) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Content.Contains("***"))
            yield return new ValidationResult("Content of the comment can't contains \"***\" word");
    }
}