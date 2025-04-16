namespace Application.Exceptions.Shared;

public class MissingAttributeException(string atributeName)
    : Exception($"O atributo: {atributeName} está em falta!")
{
    public string AtributeName { get; } = atributeName;
}