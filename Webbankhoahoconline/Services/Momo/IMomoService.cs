using Webbankhoahoconline.Models;
using Webbankhoahoconline.Models.Momo;

namespace Webbankhoahoconline.Services.Momo
{
    public interface IMomoService
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentMomo(OrderInfoModel model);
        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
    }
}
