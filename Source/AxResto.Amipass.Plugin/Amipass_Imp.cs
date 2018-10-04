using System;
using System.IO;
using System.Net;
using AxResto.Amipass.Plugin.Dto;
using Newtonsoft.Json;

namespace AxResto.Amipass.Plugin
{
    public class Amipass_Imp : IAmipass
    {
        public RespuestaDto Pagar(string tokenAutenticacion, string codigoLocal, string codigoQr, string monto)
        {
            string codigoPromocion = "";
            string parametros = $"?NumeroTransaccion={codigoQr}&Monto={monto}&CodLocal={codigoLocal}&CodPromocion={codigoPromocion}";
            string url = "https://pay.amipass.com/wspayTest/PayPAP" + parametros;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Headers["Authorization"] = string.Format("Basic {0}", tokenAutenticacion);
            string respuesta = "";
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                    {
                        var content = stream.ReadToEnd();
                        respuesta = JsonConvert.DeserializeObject<string>(content);
                    }
                }
                RespuestaDto resp = JsonConvert.DeserializeObject<RespuestaDto>(respuesta);
                return resp;
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception Plugin Amipass [{ex.Message}]" );
            }
        }
    }
}

