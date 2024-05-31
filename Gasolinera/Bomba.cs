using System.Threading.Tasks;

public class Bomba
{
    public int ID { get; set; }
    public bool EstaOperativa { get; set; }

    public async Task IniciarAbastecimientoAsync()
    {
        EstaOperativa = true;
        // Simular operación asincrónica
        await Task.Delay(5000);
    }

    public async Task TerminarAbastecimientoAsync()
    {
        EstaOperativa = false;
        // Simular operación asincrónica
        await Task.Delay(2000);
    }
}
