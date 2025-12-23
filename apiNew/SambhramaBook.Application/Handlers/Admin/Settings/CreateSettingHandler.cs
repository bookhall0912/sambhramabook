using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Entities;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Admin.Settings;

public class CreateSettingHandler
{
    private readonly IPlatformSettingRepository _platformSettingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateSettingHandler(
        IPlatformSettingRepository platformSettingRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _platformSettingRepository = platformSettingRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<CreateSettingResponse> HandleAsync(CreateSettingRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await _platformSettingRepository.GetByKeyAsync(request.Key, cancellationToken);
        if (existing != null)
        {
            return new CreateSettingResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "CONFLICT", Message = "Setting already exists" }
            };
        }

        var setting = new PlatformSetting
        {
            SettingKey = request.Key,
            SettingValue = request.Value?.ToString() ?? string.Empty,
            Description = request.Description,
            // PlatformSetting doesn't have CreatedAt property
            UpdatedAt = _dateTimeProvider.GetUtcNow()
        };

        setting = await _platformSettingRepository.CreateAsync(setting, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new CreateSettingResponse
        {
            Success = true,
            Data = new CreateSettingResponseData
            {
                Key = setting.SettingKey,
                Value = setting.SettingValue,
                Message = "Setting created successfully"
            }
        };
    }
}

