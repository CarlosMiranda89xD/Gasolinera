public class TransaccionAbastecimiento
{
    public DateTime Fecha { get; set; }
    public string NombreCliente { get; set; }
    public double Cantidad { get; set; }
    public double PrecioPorLitro { get; set; }
    public string TipoAbastecimiento { get; set; }
    public double MontoPagado { get; set; }
    public int IdBomba { get; set; }  // Campo para identificar la bomba utilizada
}
