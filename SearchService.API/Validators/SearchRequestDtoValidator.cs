using FluentValidation;
using SearchService.API.DTO;

namespace SearchService.API.Validators;

public class SearchRequestDtoValidator : AbstractValidator<SearchRequestDto>
{
    public SearchRequestDtoValidator()
    {
        RuleFor(x => x.ServiceName)
            .NotEmpty().WithMessage("ServiceName is required.");

        RuleFor(x => x.UserLocation)
            .NotNull().WithMessage("UserLocation is required.");

        RuleFor(x => x.UserLocation.Lat)
            .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");

        RuleFor(x => x.UserLocation.Lng)
            .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.");
    }
}