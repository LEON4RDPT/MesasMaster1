namespace Application.Exceptions.Reserva;

public class InvalidDateException : Exception
{
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }

    public InvalidDateException(DateTime startDate, DateTime endDate)
        : base($"Data inválida: de {startDate:HH:mm} até {endDate:HH:mm}")
    {
        StartDate = startDate;
        EndDate = endDate;
    }
}