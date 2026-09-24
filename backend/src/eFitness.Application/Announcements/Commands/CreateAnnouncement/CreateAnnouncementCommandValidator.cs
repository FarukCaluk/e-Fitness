using FluentValidation;

namespace eFitness.Application.Announcements.Commands.CreateAnnouncement;

public class CreateAnnouncementCommandValidator : AbstractValidator<CreateAnnouncementCommand>
{
    public CreateAnnouncementCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Body).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.Segment).IsInEnum();
    }
}
