using DreamCine.Application.DTOs.Payment;

namespace DreamCine.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> ProcessPaymentAsync(PaymentInfoDto paymentInfo, decimal amount);
    }
}
