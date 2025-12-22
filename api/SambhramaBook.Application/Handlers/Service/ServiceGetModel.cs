using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Service;

public sealed record ServiceGetModel(double Longitude, double Lattitude, ServiceType Type, double Radius, int Limit);
