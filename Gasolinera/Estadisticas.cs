using System.Collections.Generic;
using System.Linq;

namespace Gasolinera
{
    public static class Estadisticas 
    {
        public static Dictionary<string, object> GenerarCierreDiario(List<TransaccionAbastecimiento> transacciones)
        {
            var cierreDiario = transacciones.GroupBy(t => t.Fecha.Date)
                                            .Select(g => new { Fecha = g.Key, Total = g.Sum(t => t.Cantidad * t.PrecioPorLitro) })
                                            .ToDictionary(x => x.Fecha.ToString("yyyy-MM-dd"), x => (object)x.Total);
            return cierreDiario;
        }

        public static Dictionary<string, object> GenerarInformePrepago(List<TransaccionAbastecimiento> transacciones)
        {
            var informePrepago = transacciones.Where(t => t.TipoAbastecimiento == "Prepago")
                                              .GroupBy(t => t.Fecha.Date)
                                              .Select(g => new { Fecha = g.Key, Total = g.Sum(t => t.Cantidad * t.PrecioPorLitro) })
                                              .ToDictionary(x => x.Fecha.ToString("yyyy-MM-dd"), x => (object)x.Total);
            return informePrepago;
        }

        public static Dictionary<string, object> GenerarInformeTanqueLleno(List<TransaccionAbastecimiento> transacciones)
        {
            var informeTanqueLleno = transacciones.Where(t => t.TipoAbastecimiento == "Tanque Lleno")
                                                  .GroupBy(t => t.Fecha.Date)
                                                  .Select(g => new { Fecha = g.Key, Total = g.Sum(t => t.Cantidad * t.PrecioPorLitro) })
                                                  .ToDictionary(x => x.Fecha.ToString("yyyy-MM-dd"), x => (object)x.Total);
            return informeTanqueLleno;
        }

        public static string ObtenerBombaMasUtilizada(List<TransaccionAbastecimiento> transacciones)
        {
            var bombaMasUtilizada = transacciones.GroupBy(t => t.IdBomba)  // Asumiendo que IdBomba es un campo existente
                                                 .OrderByDescending(g => g.Count())
                                                 .FirstOrDefault();
            return bombaMasUtilizada?.Key.ToString() ?? "N/A";
        }

        public static string ObtenerBombaMenosUtilizada(List<TransaccionAbastecimiento> transacciones)
        {
            var bombaMenosUtilizada = transacciones.GroupBy(t => t.IdBomba)  // Asumiendo que IdBomba es un campo existente
                                                   .OrderBy(g => g.Count())
                                                   .FirstOrDefault();
            return bombaMenosUtilizada?.Key.ToString() ?? "N/A";
        }

    }
}
