using eFitness.Domain.Entities;
using eFitness.Domain.Enums;

namespace eFitness.Application.Payments;

public record PaymentDto(
    int Id,
    int MemberId,
    string MemberName,
    decimal Amount,
    DateTime PaymentDate,
    PaymentMethod Method,
    PaymentStatus Status,
    PaymentPurpose Purpose,
    string? TransactionReference)
{
    public static PaymentDto FromEntity(Payment payment) => new(
        payment.Id,
        payment.MemberId,
        $"{payment.Member.User.FirstName} {payment.Member.User.LastName}",
        payment.Amount,
        payment.PaymentDate,
        payment.Method,
        payment.Status,
        payment.Purpose,
        payment.TransactionReference);
}
