using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Entities;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Admin.Settings;

public class UpdateSettingHandler
{
    private readonly IPlatformSettingRepository _platformSettingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateSettingHandler(
        IPlatformSettingRepository platformSettingRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _platformSettingRepository = platformSettingRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<UpdateSettingResponse> HandleAsync(string key, UpdateSettingRequest request, CancellationToken cancellationToken = default)
    {
        var setting = await _platformSettingRepository.GetByKeyAsync(key, cancellationToken);
        if (setting == null)
        {
            return new UpdateSettingResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Setting not found" }
            };
        }

        setting.SettingValue = request.Value.ToString() ?? string.Empty;
        setting.UpdatedAt = _dateTimeProvider.GetUtcNow();

        await _platformSettingRepository.UpdateAsync(setting, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new UpdateSettingResponse
        {
            Success = true,
            Data = new UpdateSettingResponseData
            {
                Key = setting.SettingKey,
                Value = setting.SettingValue,
                Message = "Setting updated successfully"
            }
        };
    }
}

