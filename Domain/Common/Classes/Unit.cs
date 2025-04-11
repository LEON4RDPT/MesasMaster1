
using Domain.Common.Interfaces;

namespace Domain.Common.Classes;

public sealed class Unit : IRequest, IResponse
{
    public static readonly Unit Value = new();
    private Unit() { }
}