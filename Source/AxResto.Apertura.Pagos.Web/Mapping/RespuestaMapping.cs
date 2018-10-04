namespace AxResto.Apertura.Pagos.Web.Mapping
{
    /// <summary>
    /// Esta clase transforma una respuesta de cada plugin a una respuesta generica
    /// entendida por el sistema Restô
    /// </summary>
    public static class RespuestaMapping
    {
        /// <summary>
        /// Transforma una respuesta del servicio de Amipass a una rspuesta de Restô
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void FromRespuestaAmipass(AxResto.Amipass.Plugin.Dto.RespuestaDto source,
            AxResto.Apertura.Pagos.Web.Dto.RespuestaDto target)
        {
            if (source.CodRespuesta.Equals("1"))
            {
                // aprobada
                target.Estado = true;
                target.Monto = source.Monto;
                target.Transaccion = source.CodAutorizacion;
            }
            else // rechazado
            {
                target.Estado = false;
                target.MensajeError = source.DesRespuesta;
            }
        }

        /// <summary>
        /// Transforma una respuesta del servicio de Pipol a una rspuesta de Restô
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void FromRespuestaPipol(AxResto.Pipol.Plugin.Dto.RespuestaDto source,
            AxResto.Apertura.Pagos.Web.Dto.RespuestaDto target)
        {
            if (source.resultCode == 0)
            {
                // aprobada
                target.Estado = true;               
            }
            else // rechazado
            {
                target.Estado = false;
                target.MensajeError = $"{source.resultData} ({source.resultCode})"  ;
            }
        }
    }
}
