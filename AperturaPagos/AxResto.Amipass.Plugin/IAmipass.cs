using AxResto.Amipass.Plugin.Dto;

namespace AxResto.Amipass.Plugin
{
    public interface IAmipass
    {
        RespuestaDto Pagar(string tokenAutenticacion, string codigoLocal, string codigoQr, string monto, string url = "");
    }
}
