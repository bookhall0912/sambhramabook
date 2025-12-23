using SambhramaBook.Application.Common;
using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Queries;

namespace SambhramaBook.Application.Handlers.Admin.Users;

public interface IGetAllUsersHandler : IQueryHandler<GetAllUsersRequest, GetAllUsersResponse>;

public class GetAllUsersHandler : IGetAllUsersHandler
{
    private readonly IAdminQueries _adminQueries;

    public GetAllUsersHandler(IAdminQueries adminQueries)
    {
        _adminQueries = adminQueries;
    }

    public async Task<GetAllUsersResponse> Handle(GetAllUsersRequest request, CancellationToken ct)
    {
        var (users, total) = await _adminQueries.GetAllUsersAsync(
            request.Search,
            request.Status,
            request.Page,
            request.PageSize,
            ct);

        var userDtos = new List<AdminUserDto>();
        foreach (var user in users)
        {
            var bookingCount = await _adminQueries.GetUserBookingCountAsync(user.Id, ct);
            userDtos.Add(new AdminUserDto
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Email = user.Email,
                Mobile = user.Mobile,
                Avatar = user.UserProfile?.ProfileImageUrl,
                Status = user.IsActive ? "ACTIVE" : "INACTIVE",
                JoinDate = user.CreatedAt.ToString("yyyy-MM-dd"),
                Bookings = bookingCount
            });
        }

        return new GetAllUsersResponse
        {
            Success = true,
            Data = userDtos,
            Pagination = new PaginationInfo
            {
                Page = request.Page,
                PageSize = request.PageSize,
                Total = total,
                TotalPages = (int)Math.Ceiling(total / (double)request.PageSize)
            }
        };
    }
}

