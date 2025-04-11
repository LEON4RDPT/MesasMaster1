namespace Application.Exceptions.User;

public class InvalidEmailException(string email) : 
    Exception($"O email {email} não é valido!")
{
    public string Email { get; } = email;
}