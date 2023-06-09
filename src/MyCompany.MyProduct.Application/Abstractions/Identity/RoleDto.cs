﻿namespace MyCompany.MyProduct.Application.Abstractions.Identity;

public sealed class RoleDto
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string NormalizeName { get; set; } = default!;
    public string ConcurrencyStamp { get; set; } = default!;
}