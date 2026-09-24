using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Members.Commands.UpdateMember;

public record UpdateMemberCommand(
    int Id,
    string? PhoneNumber,
    DateTime? DateOfBirth,
    string? Gender,
    string? Address,
    string? City,
    string? EmergencyContactName,
    string? EmergencyContactPhone,
    int? AssignedTrainerId) : IRequest;

public class UpdateMemberCommandHandler : IRequestHandler<UpdateMemberCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateMemberCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await _context.Members
            .Include(m => m.User)
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (member is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Member), request.Id);
        }

        if (request.AssignedTrainerId is not null)
        {
            var trainerExists = await _context.Trainers.AnyAsync(t => t.Id == request.AssignedTrainerId, cancellationToken);
            if (!trainerExists)
            {
                throw new NotFoundException(nameof(Domain.Entities.Trainer), request.AssignedTrainerId);
            }
        }

        member.User.PhoneNumber = request.PhoneNumber;
        member.DateOfBirth = request.DateOfBirth;
        member.Gender = request.Gender;
        member.Address = request.Address;
        member.City = request.City;
        member.EmergencyContactName = request.EmergencyContactName;
        member.EmergencyContactPhone = request.EmergencyContactPhone;
        member.AssignedTrainerId = request.AssignedTrainerId;
        member.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
