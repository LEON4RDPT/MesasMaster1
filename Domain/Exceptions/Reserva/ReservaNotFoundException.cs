namespace Domain.Exceptions.Reserva;

public class ReservaNotFoundException(int reservaId) : 
    Exception($"Reserva com o Id: {reservaId} não encontrada!")
{
    public int ReservaId { get; set; } = reservaId;
}