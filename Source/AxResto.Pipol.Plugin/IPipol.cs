using AxResto.Pipol.Plugin.Dto;

namespace AxResto.Pipol.Plugin
{
    public interface IPipol
    {
        RespuestaDto Pagar(string commerceKey, string codigoQr, string monto, string comanda);
    }
}
