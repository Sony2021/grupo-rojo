using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using grupo_rojo.Models;
using Microsoft.Data.SqlClient;

public class PagoController : Controller
{
    private readonly IPayPalService _payPalService;
    private readonly IConfiguration _configuration;

    public PagoController(IPayPalService payPalService, IConfiguration configuration)
    {
        _payPalService = payPalService;
        _configuration = configuration;
    }

    // Acción para iniciar el pago
    public async Task<IActionResult> Create(decimal monto)
    {
        var returnUrl = Url.Action("Success", "Pago", null, Request.Scheme);
        var cancelUrl = Url.Action("Cancel", "Pago", null, Request.Scheme);
        var redirectUrl = await _payPalService.CreatePaymentAsync(monto, "USD", returnUrl, cancelUrl);
        return Redirect(redirectUrl);
    }

    // Acción para manejar el resultado del pago
    [HttpPost]
    public async Task<IActionResult> CreatePayment(Pago pago)
    {
        if (ModelState.IsValid)
        {
            var returnUrl = Url.Action("Success", "Pago", null, Request.Scheme);
            var cancelUrl = Url.Action("Cancel", "Pago", null, Request.Scheme);
            var redirectUrl = await _payPalService.CreatePaymentAsync(pago.MontoTotal, "USD", returnUrl, cancelUrl);

            // Guardar los datos en la base de datos
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    var query = @"
                        INSERT INTO Pagos (PaymentDate, NombreTarjeta, NumeroTarjeta, DueDateYYMM, Cvv, MontoTotal, UserID)
                        VALUES (@PaymentDate, @NombreTarjeta, @NumeroTarjeta, @DueDateYYMM, @Cvv, @MontoTotal, @UserID);
                    ";
                    var command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@PaymentDate", DateTime.Now);
                    command.Parameters.AddWithValue("@NombreTarjeta", pago.NombreTarjeta);
                    command.Parameters.AddWithValue("@NumeroTarjeta", pago.NumeroTarjeta);
                    command.Parameters.AddWithValue("@DueDateYYMM", pago.DueDateYYMM);
                    command.Parameters.AddWithValue("@Cvv", pago.Cvv);
                    command.Parameters.AddWithValue("@MontoTotal", pago.MontoTotal);
                    command.Parameters.AddWithValue("@UserID", pago.UserID);

                    await command.ExecuteNonQueryAsync();
                }

                return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                // Manejo de errores, redirigir a una vista de error, registrar el error, etc.
                return RedirectToAction("Error", new { message = ex.Message });
            }
        }
        return View(pago);
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