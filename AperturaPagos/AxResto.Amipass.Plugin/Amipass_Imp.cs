using System;
using System.IO;
using System.Net;
using AxResto.Amipass.Plugin.Dto;
using Newtonsoft.Json;

namespace AxResto.Amipass.Plugin
{
    public class Amipass_Imp : IAmipass
    {
        public RespuestaDto Pagar(string tokenAutenticacion, string codigoLocal, string codigoQr, string monto, string url = "")
        {
            string codigoPromocion = "";
            string parametros = $"?NumeroTransaccion={codigoQr}&Monto={monto}&Codlocal={codigoLocal}&CodPromocion={codigoPromocion}";          
            string urlx;
            if (url.Equals(""))
            {
                urlx = "https://intpay.amipassqa.com/wspay/PAYPAP" + parametros;                
            } else
            {
                urlx = url + parametros;
            }
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(urlx));
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
            } catch (Exception ex)
            {
                throw new Exception($"Exception Plugin Amipass [{ex.Message}]");
            }
        }
    }
}

