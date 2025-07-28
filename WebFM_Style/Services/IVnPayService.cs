
using WebFM_Style.Models.ViewModel.ViewModel;

namespace WebFM_Style.Services
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        VnPaymentResponseModels PaymentExecute(IQueryCollection collections);
    }
}
