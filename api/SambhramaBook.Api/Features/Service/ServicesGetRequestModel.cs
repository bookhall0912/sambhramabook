using Microsoft.AspNetCore.Mvc;
using SambhramaBook.Application.Handlers.Service;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Api.Features.Service;

public sealed class ServicesGetRequestModel
{
    [FromQuery]
    public required double Longitude { get; init; }
    [FromQuery]
    public required double Lattitude { get; init; }
    [FromQuery]
    public ServiceType Type { get; init; }
    [FromQuery]
    public int Limit { get; init; }
    [FromQuery]
    public double Radius { get; init; }

    public ServiceGetModel ToModel() => new ServiceGetModel(Longitude, Lattitude, Type, Radius, Limit);
}
