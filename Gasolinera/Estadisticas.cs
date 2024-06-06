using System;
using System.Collections.Generic;
using System.Linq;

// Define el espacio de nombres que agrupa las clases relacionadas con la gasolinera.
namespace Gasolinera
{
    // La clase Estadisticas es estática porque solo contiene métodos que no dependen de instancias específicas de la clase.
    public static class Estadisticas
    {
        // Genera un informe de cierre diario basado en una lista de transacciones.
        public static Dictionary<string, object> GenerarCierreDiario(List<TransaccionAbastecimiento> transacciones)
        {
            // Agrupa las transacciones por fecha, calcula el total de ventas por día y devuelve un diccionario.
            var cierreDiario = transacciones
                .GroupBy(t => t.Fecha.Date) // Agrupa las transacciones por la propiedad Fecha (solo la parte de la fecha).
                .Select(g => new { Fecha = g.Key, Total = g.Sum(t => t.Cantidad * t.PrecioPorLitro) }) // Para cada grupo, calcula el total vendido.
                .ToDictionary(x => x.Fecha.ToString("yyyy-MM-dd"), x => (object)x.Total); // Convierte los resultados en un diccionario.

            return cierreDiario; // Devuelve el diccionario con los totales diarios.
        }

        // Genera un informe de ventas para transacciones de tipo "Prepago".
        public static Dictionary<string, object> GenerarInformePrepago(List<TransaccionAbastecimiento> transacciones)
        {
            // Filtra las transacciones por el tipo "Prepago", agrupa por fecha y calcula el total de ventas.
            var informePrepago = transacciones
                .Where(t => t.TipoAbastecimiento == "Prepago") // Filtra para obtener solo las transacciones de tipo Prepago.
                .GroupBy(t => t.Fecha.Date) // Agrupa por la fecha de la transacción.
                .Select(g => new { Fecha = g.Key, Total = g.Sum(t => t.Cantidad * t.PrecioPorLitro) }) // Calcula el total vendido por día.
                .ToDictionary(x => x.Fecha.ToString("yyyy-MM-dd"), x => (object)x.Total); // Convierte a diccionario.

            return informePrepago; // Devuelve el diccionario.
        }

        // Genera un informe de ventas para transacciones donde se llenó el tanque completamente.
        public static Dictionary<string, object> GenerarInformeTanqueLleno(List<TransaccionAbastecimiento> transacciones)
        {
            // Similar a GenerarInformePrepago pero filtrando por el tipo "Tanque Lleno".
            var informeTanqueLleno = transacciones
                .Where(t => t.TipoAbastecimiento == "Tanque Lleno") // Filtra para obtener solo las transacciones de tipo Tanque Lleno.
                .GroupBy(t => t.Fecha.Date) // Agrupa por la fecha de la transacción.
                .Select(g => new { Fecha = g.Key, Total = g.Sum(t => t.Cantidad * t.PrecioPorLitro) }) // Calcula el total vendido por día.
                .ToDictionary(x => x.Fecha.ToString("yyyy-MM-dd"), x => (object)x.Total); // Convierte a diccionario.

            return informeTanqueLleno; // Devuelve el diccionario.
        }

        // Identifica la bomba más utilizada en las transacciones.
        public static string ObtenerBombaMasUtilizada(List<TransaccionAbastecimiento> transacciones)
        {
            // Agrupa las transacciones por ID de bomba y encuentra la más utilizada.
            var bombaMasUtilizada = transacciones
                .GroupBy(t => t.IdBomba) // Agrupa por el ID de la bomba.
                .OrderByDescending(g => g.Count()) // Ordena los grupos por la cantidad de uso descendente.
                .FirstOrDefault(); // Toma el primer grupo, el más utilizado.

            return bombaMasUtilizada?.Key.ToString() ?? "N/A"; // Devuelve el ID de la bomba más utilizada o "N/A" si no hay datos.
        }

        // Identifica la bomba menos utilizada en las transacciones.
        public static string ObtenerBombaMenosUtilizada(List<TransaccionAbastecimiento> transacciones)
        {
            // Agrupa las transacciones por ID de bomba y encuentra la menos utilizada.
            var bombaMenosUtilizada = transacciones
                .GroupBy(t => t.IdBomba) // Agrupa por el ID de la bomba.
                .OrderBy(g => g.Count()) // Ordena los grupos por la cantidad de uso ascendente.
                .FirstOrDefault(); // Toma el primer grupo, el menos utilizado.

            return bombaMenosUtilizada?.Key.ToString() ?? "N/A"; // Devuelve el ID de la bomba menos utilizada o "N/A" si no hay datos.
        }
    }
}
