using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using eFitness.Domain.Entities;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Trainers.Commands.CreateTrainer;

public record CreateTrainerCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string Specialization,
    string? Bio,
    int YearsOfExperience,
    decimal HourlyRate) : IRequest<int>;

public class CreateTrainerCommandHandler : IRequestHandler<CreateTrainerCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public CreateTrainerCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<int> Handle(CreateTrainerCommand request, CancellationToken cancellationToken)
    {
        var emailInUse = await _context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);

        if (emailInUse)
        {
            throw new ConflictException("An account with this email already exists.");
        }

        var user = new User
        {
            Email = request.Email,
            PasswordHash = _passwordHasher.Hash(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Role = UserRole.Trainer,
            IsActive = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        var trainer = new Trainer
        {
            UserId = user.Id,
            Specialization = request.Specialization,
            Bio = request.Bio,
            YearsOfExperience = request.YearsOfExperience,
            HourlyRate = request.HourlyRate,
            IsAvailable = true
        };

        _context.Trainers.Add(trainer);
        await _context.SaveChangesAsync(cancellationToken);

        return trainer.Id;
    }
}
