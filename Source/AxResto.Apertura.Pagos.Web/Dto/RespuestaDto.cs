namespace AxResto.Apertura.Pagos.Web.Dto
{
    public class RespuestaDto
    {
        public bool Estado { get; set; }
        public string Transaccion { get; set; }
        public string Monto { get; set; }
        public string MensajeError { get; set; }
    }
}
