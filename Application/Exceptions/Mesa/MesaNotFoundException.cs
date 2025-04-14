namespace Application.Exceptions.Mesa;

public class MesaNotFoundException(int mesaId) :
    Exception($"Mesa com o id {mesaId} não foi encontrada!")
{
    public int MesaId { get; set; } = mesaId;
}