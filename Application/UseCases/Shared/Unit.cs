using Application.Interfaces;

namespace Application.UseCases.Shared;

public sealed class Unit : IRequest, IResponse
{
    public static readonly Unit Value = new();

    private Unit()
    {
    }
}