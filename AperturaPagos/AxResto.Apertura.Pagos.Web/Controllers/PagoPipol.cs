using AxResto.Apertura.Pagos.Web.Config;
using AxResto.Apertura.Pagos.Web.Dto;
using AxResto.Apertura.Pagos.Web.Mapping;
using AxResto.Pipol.Plugin;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace AxResto.Apertura.Pagos.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagoPipol : ControllerBase
    {
        #region private members
        private readonly ILog _logger;
        private readonly IPipol _service;
        private readonly IOptions<PipolConfig> _settings;
        #endregion

        #region constructor
        public PagoPipol(IPipol pipol, ILog log, IOptions<PipolConfig> pipolConfig)
        {
            _logger = log;
            _service = pipol;
            _settings = pipolConfig;
        }
        #endregion

        /// <summary>
        /// Método que genera un pago con Pipol.
        /// En caso de querer realizar una prueba copiar en el browser
        /// http://localhost:5000/api/PagoPipol/Pagar
        /// Parámetros en el body
        /// {
        ///   "comanda": "55",
        ///   "monto": "100",
        ///   "codigo": "QR01PIPOL31"
        /// }
        /// </summary>

        [HttpPost("Pagar")]
        public ActionResult<RespuestaDto> Pagar([FromBody]IDictionary<string, string> value)
        {
            _logger.Debug($"[START] PagoPipol.Pagar");
            RespuestaDto resp = new RespuestaDto();
            try
            {
                // lectura del dictionary
                string comanda = value["comanda"];
                string codigo = value["codigo"];
                string monto = value["monto"];
                _logger.Debug($"Comanda: {comanda}, código: {codigo}, monto {monto}");

                string url;
                if (_settings.Value.Enviroment.Equals("STAGING"))
                {
                    url = _settings.Value.Url_Test;
                } else
                {
                    url = _settings.Value.Url_Prod;
                }
                string commerceKey = _settings.Value.CommerceKey;
                _logger.Debug($"commerceKey: {commerceKey}, url: {url}");
                var respuestaService = _service.Pagar(commerceKey, codigo, monto, comanda, url);
                RespuestaMapping.FromRespuestaPipol(respuestaService, resp);
                _logger.Debug($"[FINISH] PagoPipol.Pagar: {comanda}");               
            }
            catch (Exception ex)
            {
                _logger.Error("[EXCEPT] PagoPipol.Pagar", ex);
                //throw new Exception("Error el la llamada al servicio. (ver log)");
                resp.Estado = false;
                resp.MensajeError = $"Error en la llamada al servicio. (Error nativo: {ex})";
            }
            return resp;
        }
    }
}
