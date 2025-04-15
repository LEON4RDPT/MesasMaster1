namespace Application.Exceptions.Shared;

public class MissingEnvironmentValue(string value)
    : Exception($"A {value} variavel não encontrada!")
{
    public string Value { get; } = value;
}