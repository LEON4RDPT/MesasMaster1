﻿namespace Application.UseCases.User.DTO;

public class UserDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public bool IsAdmin { get; set; }
}