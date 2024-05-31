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
            {"TotalTransacciones", Transacciones.Count},
            {"TotalLitrosAbastecidos", Transacciones.Sum(t => t.Cantidad)},
            {"TotalIngresos", Transacciones.Sum(t => t.MontoPagado)},
            {"TotalPrepago", Transacciones.Where(t => t.TipoAbastecimiento == "Prepago").Sum(t => t.MontoPagado)},
            {"TotalTanqueLleno", Transacciones.Where(t => t.TipoAbastecimiento == "Tanque Lleno").Sum(t => t.MontoPagado)},
            {"BombaMasUtilizada", Transacciones.GroupBy(t => t.IdBomba).OrderByDescending(g => g.Count()).FirstOrDefault()?.Key},
            {"BombaMenosUtilizada", Transacciones.GroupBy(t => t.IdBomba).OrderBy(g => g.Count()).FirstOrDefault()?.Key}
        };
    }

    public List<TransaccionAbastecimiento> ObtenerTransaccionesPorFecha(DateTime fechaInicio, DateTime fechaFin)
    {
        return Transacciones.Where(t => t.Fecha >= fechaInicio && t.Fecha <= fechaFin).ToList();
    }
}
