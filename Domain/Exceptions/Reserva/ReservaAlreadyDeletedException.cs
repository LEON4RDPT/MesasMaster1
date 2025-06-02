namespace Domain.Exceptions.Reserva;

public class ReservaAlreadyDeletedException(int reservaId) : 
    Exception($"Reserva com o Id: {reservaId} já foi removida!")
{
    public int ReservaId { get; set; } = reservaId;
}