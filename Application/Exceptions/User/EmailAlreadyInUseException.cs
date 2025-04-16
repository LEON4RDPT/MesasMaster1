namespace Application.Exceptions.User;

public class EmailAlreadyInUseException(string emailAlreadyInUse)
    : Exception($"O email {emailAlreadyInUse} já está em uso!")
{
    public string EmailAlreadyInUse { get; } = emailAlreadyInUse;
}