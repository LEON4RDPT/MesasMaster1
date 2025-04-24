namespace Domain.Exceptions.Mesa;

public class MesaAlreadyAtUseException(int mesaId)
    : Exception($"A mesa {mesaId} já está em uso!")
{
    private int MesaId { get;set; } = mesaId;
}