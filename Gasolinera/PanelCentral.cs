using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class PanelCentral
{
    public List<Bomba> Bombas { get; set; } = new List<Bomba>();
    public List<TransaccionAbastecimiento> Transacciones { get; set; } = new List<TransaccionAbastecimiento>();

    public PanelCentral()
    {
        // Inicializar bombas (ejemplo con 4 bombas)
        for (int i = 1; i <= 4; i++)
        {
            Bombas.Add(new Bomba { ID = i });
        }
    }

    public void AgregarTransaccion(TransaccionAbastecimiento transaccion)
    {
        Transacciones.Add(transaccion);
    }

    public async Task OperarBombaAsync(int idBomba, bool iniciar)
    {
        var bomba = Bombas.FirstOrDefault(b => b.ID == idBomba);
        if (bomba != null)
        {
            if (iniciar)
            {
                await bomba.IniciarAbastecimientoAsync();
            }
            else
            {
                await bomba.TerminarAbastecimientoAsync();
            }
        }
    }

    public void ResetearDatos()
    {
        Transacciones.Clear();
        foreach (var bomba in Bombas)
        {
            bomba.EstaOperativa = false;
        }
    }

    public Dictionary<string, object> ObtenerEstadisticas()
    {
        return new Dictionary<string, object>
        {
            {"Total Transacciones", Transacciones.Count},
            {"Total Litros Abastecidos", Transacciones.Sum(t => t.Cantidad)},
            {"Total Ingresos", Transacciones.Sum(t => t.MontoPagado)},
            {"Total Prepago", Transacciones.Where(t => t.TipoAbastecimiento == "Prepago").Sum(t => t.MontoPagado)},
            {"Total TanqueLleno", Transacciones.Where(t => t.TipoAbastecimiento == "Tanque Lleno").Sum(t => t.MontoPagado)},
            {"Bomba Mas Utilizada", Transacciones.GroupBy(t => t.IdBomba).OrderByDescending(g => g.Count()).FirstOrDefault()?.Key},
            {"Bomba Menos Utilizada", Transacciones.GroupBy(t => t.IdBomba).OrderBy(g => g.Count()).FirstOrDefault()?.Key}
        };
    }

    public List<TransaccionAbastecimiento> ObtenerTransaccionesPorFecha(DateTime fechaInicio, DateTime fechaFin)
    {
        return Transacciones.Where(t => t.Fecha >= fechaInicio && t.Fecha <= fechaFin).ToList();
    }
}
