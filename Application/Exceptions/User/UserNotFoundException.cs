namespace Application.Exceptions.User;

public class UserNotFoundException(int id) : Exception($"Utilizador com o id: {id} não foi encontrado!")
{
    public int Id { get; } = id;
}

