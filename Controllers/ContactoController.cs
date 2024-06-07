using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using grupo_rojo.Models;
using grupo_rojo.Data;
using Microsoft.Extensions.ML;

namespace grupo_rojo.Controllers
{
    public class ContactoController : Controller
    {
        private readonly ILogger<ContactoController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly PredictionEnginePool<MLModel1.ModelInput,MLModel1.ModelOutput> _predictionEnginePool;

        public ContactoController(ILogger<ContactoController> logger,ApplicationDbContext context,
            PredictionEnginePool<MLModel1.ModelInput,MLModel1.ModelOutput> predictionEnginePool)
        {
            _logger = logger;
            _context = context;
            _predictionEnginePool = predictionEnginePool;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EnviarMensaje(Contacto objcontato)
        {
            _logger.LogDebug("Ingreso a Enviar Mensaje");
            
            //Se registran los datos del objeto a la base datos
            _context.Add(objcontato);
            _context.SaveChanges();
            
            ViewData["Message"] = "Se registro el contacto";

            // Realizar predicción de sentimiento
            var modelInput = new MLModel1.ModelInput
            {
                SentimentText = objcontato.Message
            };
            
            var prediction = _predictionEnginePool.Predict(modelInput);
            
            ViewData["Sentimiento"] = prediction.PredictedLabel;
            ViewData["Score"] = prediction.Score[1];

            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}