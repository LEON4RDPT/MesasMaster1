namespace Application.Exceptions.Mesa;

public class InvalidMesaPosition(int x, int y)
    : Exception($"A posição: {x},{y} não é valida!")
{
    public int X { get; } = x;
    public int Y { get; } = y;
}