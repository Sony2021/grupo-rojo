using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace grupo_rojo.Models
{
    public interface IPayPalService
    {
        Task<string> GetAccessTokenAsync();
        Task<string> CreatePaymentAsync(decimal total, string currency, string returnUrl, string cancelUrl);
    }
}