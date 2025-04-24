namespace Application.UseCases.Mesa.DTO;

public class MesaDto
{
    public int Id { get; set; }
    public required int TimeLimit { get; set; } //minutes
    public required int LocalX { get; set; }
    public required int LocalY { get; set; }
    public required int CapUsers { get; set; }
}
