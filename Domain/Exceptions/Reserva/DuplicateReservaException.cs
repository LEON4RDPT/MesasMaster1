namespace Domain.Exceptions.Reserva;

public class DuplicateReservaException : Exception
{
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
    public int UserId { get; }

    public DuplicateReservaException(DateTime startDate, DateTime endDate, int userId)
        : base($"Reserva duplicada pelo utilizador com o id {userId}.\nDas {startDate.Date} ás {endDate.Date}")
    {
        StartDate = startDate;
        EndDate = endDate;
        UserId = userId;
    }
}