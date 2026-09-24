using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using eFitness.Domain.Entities;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Auth.Commands.Register;

public record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string? PhoneNumber) : IRequest<int>;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<int> Handle(RegisterCommand request, CancellationToken cancellationToken)
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
            Role = UserRole.Client,
            IsActive = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        var member = new Member
        {
            UserId = user.Id,
            JoinDate = DateTime.UtcNow
        };

        _context.Members.Add(member);
        await _context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
