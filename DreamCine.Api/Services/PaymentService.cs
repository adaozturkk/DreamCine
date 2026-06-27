using DreamCine.Api.DTOs.Payment;
using DreamCine.Api.Interfaces;

namespace DreamCine.Api.Services
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
