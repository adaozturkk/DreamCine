using DreamCine.Api.DTOs.Payment;

namespace DreamCine.Api.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> ProcessPaymentAsync(PaymentInfoDto paymentInfo, decimal amount);
    }
}
