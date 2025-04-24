namespace Domain.Exceptions.User;

public class EmailNotFoundException(string email) : Exception($"Utilizador com o email: {email} não foi encontrado!")
{
    public string Email { get; } = email;
}