using AxResto.Amipass.Plugin;
using AxResto.Apertura.Pagos.Web.Config;
using AxResto.Apertura.Pagos.Web.Dto;
using AxResto.Apertura.Pagos.Web.Mapping;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace AxResto.Apertura.Pagos.Web.Controllers
{
    [Route("api/[controller]")]
    public class PagoAmipass : ControllerBase
    {
        #region private members
        private readonly ILog _logger;
        private readonly IAmipass _service;
        private readonly IOptions<AmipassConfig> _amipassConfig;
        #endregion

        #region constructor
        public PagoAmipass(IAmipass amipass, ILog log, IOptions<AmipassConfig> amipassConfig)
        {
            _logger = log;
            _service = amipass;
            _amipassConfig = amipassConfig;
        }
        #endregion

        /// <summary>
        /// Método que genera un pago con Amipass.
        /// En caso de querer realizar una prueba copiar en el AdvanceRest Client 
        /// http://localhost:5000/api/PagoAmipass/Pagar
        /// 
        /// Parametros en el body
        /// {
        ///  "comanda": "55",
        ///  "monto": "100",
        ///  "codigo": "90157563"
        /// }
        /// </summary>
        [HttpPost("Pagar")]
        public ActionResult<RespuestaDto> Pagar([FromBody]IDictionary<string, string> value)
        {
            _logger.Debug($"[START] PagoAmipass.Pagar");
            RespuestaDto resp = new RespuestaDto();
            try
            {
                // lectura del dictionary
                string comanda = value["comanda"];
                string codigo = value["codigo"];
                string monto = value["monto"];
                _logger.Debug($"Comanda: {comanda}, código: {codigo}, monto {monto}");

                string url;
                if (_amipassConfig.Value.Enviroment.Equals("STAGING"))
                {
                    url = _amipassConfig.Value.Url_Test;
                } else
                {
                    url = _amipassConfig.Value.Url_Prod;
                }
                string authorization = _amipassConfig.Value.Authorization;
                string codigoLocal = _amipassConfig.Value.CodigoLocal;
                _logger.Debug($"authorization: {authorization}, codigoLocal: {codigoLocal}, url: {url}");
                var respuestaService = _service.Pagar(authorization, codigoLocal, codigo, monto, url);
                RespuestaMapping.FromRespuestaAmipass(respuestaService, resp);
                _logger.Debug("[FINISH] PagoAmipass.Pagar: {commanda}");               
            }
            catch (Exception ex)
            {
                _logger.Error("[EXCEPT] PagoAmipass.Pagar", ex);
                //throw new Exception("Error el la llamada al servicio. (ver log)");
                resp.Estado = false;
                resp.MensajeError = $"Error en la llamada al servicio. (Error nativo: {ex})";
            }
            return resp;
        }
    }
}
