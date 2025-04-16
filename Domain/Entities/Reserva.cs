namespace Domain.Entities;

public class Reserva
{
    public int Id { get; set; }
    public required Mesa Mesa { get; set; }
    public required User User { get; set; }
    public DateTime DataReserva { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public bool Ativa { get; set; } = true;

}