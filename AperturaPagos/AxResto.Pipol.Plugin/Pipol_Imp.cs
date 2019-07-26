using AxResto.Pipol.Plugin.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;

namespace AxResto.Pipol.Plugin
{
    public class Pipol_Imp : IPipol
    {
        public RespuestaDto Pagar(string commerceKey, string codigoQr, string monto, string comanda, string url = "")
        {
            string urlx;
            if (url.Equals(""))
            {
                urlx = "http://api.mobile.mundopipol.com:8080/PipolRestoWeb/PipolrestoWebJson/RestoMethods/shellProducts";
            } else
            {
                urlx = url;
            }

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(urlx));
            var values = new Dictionary<string, string>{
                { "commerceKey", commerceKey },
                { "tokenQR", codigoQr },
                { "amount", monto },
                { "comanda", comanda }
            };
           
            try
            {
                HttpClient client = new HttpClient();
                var content = new FormUrlEncodedContent(values);
                Console.WriteLine(content.ToString());
                var response = client.PostAsync(url, content).Result;
                var respuesta = response.Content.ReadAsStringAsync().Result;
                RespuestaDto resp = JsonConvert.DeserializeObject<RespuestaDto>(respuesta);
                return resp;
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception Plugin Pipol [{ex.Message}]");
            }
        }
    }
}
