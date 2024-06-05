using Gasolinera;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

public class PanelCentral
{
    // Lista de bombas disponibles en la gasolinera
    public List<Bomba> Bombas { get; set; } = new List<Bomba>();

    // Lista de transacciones de abastecimiento realizadas
    public List<TransaccionAbastecimiento> Transacciones { get; set; } = new List<TransaccionAbastecimiento>();

    // Ruta del archivo donde se guardarán las transacciones
    private const string FilePath = "transacciones.txt";

    // Constructor de la clase
    public PanelCentral()
    {
        // Inicializar bombas (solo bomba 1 y 2)
        for (int i = 1; i <= 2; i++)
        {
            Bombas.Add(new Bomba { ID = i });
        }

        // Cargar transacciones desde archivo
        CargarTransacciones();
    }

    // Agrega una nueva transacción a la lista y guarda las transacciones en el archivo
    public void AgregarTransaccion(TransaccionAbastecimiento transaccion)
    {
        Transacciones.Add(transaccion);
        GuardarTransacciones();
    }

    // Opera una bomba, iniciando o terminando el abastecimiento de manera asincrónica
    public async Task OperarBombaAsync(int idBomba, bool iniciar)
    {
        // Buscar la bomba por ID
        var bomba = Bombas.FirstOrDefault(b => b.ID == idBomba);
        if (bomba != null)
        {
            if (iniciar)
            {
                // Iniciar abastecimiento de manera asincrónica
                await bomba.IniciarAbastecimientoAsync();
            }
            else
            {
                // Terminar abastecimiento de manera asincrónica
                await bomba.TerminarAbastecimientoAsync();
            }
        }
    }

    // Resetea los datos de transacciones y pone todas las bombas en estado no operativo
    public void ResetearDatos()
    {
        // Limpiar la lista de transacciones
        Transacciones.Clear();
        
        // Poner todas las bombas en estado no operativo
        foreach (var bomba in Bombas)
        {
            bomba.EstaOperativa = false;
        }

        // Guardar transacciones actualizadas
        GuardarTransacciones();
    }

    // Obtiene estadísticas agregadas de las transacciones realizadas
    public Dictionary<string, object> ObtenerEstadisticas()
    {
        return new Dictionary<string, object>
        {
            {"Total Transacciones", Transacciones.Count},
            {"Total Litros Abastecidos", Transacciones.Sum(t => t.Cantidad)},
            {"Total Ingresos", Transacciones.Sum(t => t.MontoPagado)},
            {"Total Prepago", Transacciones.Where(t => t.TipoAbastecimiento == "Prepago").Sum(t => t.MontoPagado)},
            {"Total Tanque Lleno", Transacciones.Where(t => t.TipoAbastecimiento == "Tanque Lleno").Sum(t => t.MontoPagado)},
            {"Bomba Mas Utilizada", Estadisticas.ObtenerBombaMasUtilizada(Transacciones)},
            {"Bomba Menos Utilizada", Estadisticas.ObtenerBombaMenosUtilizada(Transacciones)}
        };
    }

    // Obtiene las transacciones que se realizaron en un rango de fechas especificado
    public List<TransaccionAbastecimiento> ObtenerTransaccionesPorFecha(DateTime fechaInicio, DateTime fechaFin)
    {
        // Filtrar transacciones por rango de fechas
        return Transacciones.Where(t => t.Fecha >= fechaInicio && t.Fecha <= fechaFin).ToList();
    }

    // Guarda las transacciones en un archivo de texto
    public void GuardarTransacciones()
    {
        try
        {
            // Abrir StreamWriter para escribir en el archivo
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                // Iterar sobre cada transacción en la lista
                foreach (var transaccion in Transacciones)
                {
                    // Serializar la transacción a un string JSON
                    var json = JsonSerializer.Serialize(transaccion);
                    
                    // Escribir el string JSON en el archivo
                    writer.WriteLine(json);
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    // Carga las transacciones desde un archivo de texto
    public void CargarTransacciones()
    {
        try
        {
            // Verificar si el archivo existe
            if (File.Exists(FilePath))
            {
                // Abrir StreamReader para leer desde el archivo
                using (StreamReader reader = new StreamReader(FilePath))
                {
                    string line;
                    
                    // Leer línea por línea del archivo
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Deserializar la línea desde JSON a un objeto TransaccionAbastecimiento
                        var transaccion = JsonSerializer.Deserialize<TransaccionAbastecimiento>(line);
                        
                        // Si la deserialización fue exitosa, añadir la transacción a la lista
                        if (transaccion != null)
                        {
                            Transacciones.Add(transaccion);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
}
