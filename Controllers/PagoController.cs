using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using Microsoft.AspNetCore.Mvc;
using grupo_rojo.Data;
using grupo_rojo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class PagoController : Controller
{
    private readonly PayPalService _payPalService;

    public PagoController(PayPalService payPalService)
    {
        _payPalService = payPalService;
    }

    public async Task<IActionResult> Create(decimal monto)
    {
        var returnUrl = Url.Action("Success", "Pago", null, Request.Scheme);
        var cancelUrl = Url.Action("Cancel", "Pago", null, Request.Scheme);
        var redirectUrl = await _payPalService.CreatePaymentAsync(monto, "USD", returnUrl, cancelUrl);
        return Redirect(redirectUrl);
    }

    public IActionResult Success()
    {
        return View();
    }

    public IActionResult Cancel()
    {
        return View();
    }
}