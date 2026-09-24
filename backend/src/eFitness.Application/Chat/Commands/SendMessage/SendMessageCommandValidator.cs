using FluentValidation;

namespace eFitness.Application.Chat.Commands.SendMessage;

public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
{
    public SendMessageCommandValidator()
    {
        RuleFor(x => x.ConversationId).GreaterThan(0);
        RuleFor(x => x.Content).NotEmpty().MaximumLength(2000);
    }
}
