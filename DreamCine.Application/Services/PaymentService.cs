using DreamCine.Application.DTOs.Payment;
using DreamCine.Application.Interfaces;

namespace DreamCine.Application.Services
{
    public class PaymentService : IPaymentService
    {
        public async Task<bool> ProcessPaymentAsync(PaymentInfoDto paymentInfo, decimal amount)
        {
            await Task.Delay(1500);

            if (paymentInfo.ExpiryYear == DateTime.UtcNow.Year && paymentInfo.ExpiryMonth < DateTime.UtcNow.Month)
            {
                return false;
            }

            return true;
        }
    }
}
