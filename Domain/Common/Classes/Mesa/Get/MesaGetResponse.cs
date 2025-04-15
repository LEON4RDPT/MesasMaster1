﻿using Domain.Common.Interfaces;

namespace Domain.Common.Classes.Mesa.Get;

public class MesaGetResponse : IResponse
{
    public int Id { get; set; }
    public required int TimeLimit { get; set; } //minutes
    public required int LocalX { get; set; }
    public required int LocalY { get; set; }
    public required int CapUsers { get; set; }
}