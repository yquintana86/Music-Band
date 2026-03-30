using SharedLib.Models.Common;

namespace Application.Activities.Queries.Dtos;

public sealed record ActivityMusiciansNameDto(
    int Id,
    string Name,
    string Client,
    string? Description,
    bool International,
    DateTime? Begin,
    DateTime? End,
    double Price,
    List<SelectItem> Musicians
    );
