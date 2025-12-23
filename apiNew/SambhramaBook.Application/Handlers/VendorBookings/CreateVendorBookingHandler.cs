using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.VendorBookings;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Entities;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.VendorBookings;

public class CreateVendorBookingHandler
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IListingRepository _listingRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateVendorBookingHandler(
        IBookingRepository bookingRepository,
        IListingRepository listingRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _bookingRepository = bookingRepository;
        _listingRepository = listingRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<CreateVendorBookingResponse> HandleAsync(long vendorId, CreateVendorBookingRequest request, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByIdAsync(request.ListingId, cancellationToken);
        if (listing == null || listing.VendorId != vendorId)
        {
            return new CreateVendorBookingResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Listing not found or not owned by vendor" }
            };
        }

        // Find or create customer
        User? customer = null;
        if (!string.IsNullOrEmpty(request.CustomerPhone))
        {
            customer = await _userRepository.GetByMobileAsync(request.CustomerPhone, cancellationToken);
        }
        else if (!string.IsNullOrEmpty(request.CustomerEmail))
        {
            customer = await _userRepository.GetByEmailAsync(request.CustomerEmail, cancellationToken);
        }

        if (customer == null)
        {
            // Create new customer user
            customer = new User
            {
                Name = request.CustomerName,
                Mobile = request.CustomerPhone,
                Email = request.CustomerEmail,
                Role = UserRole.User,
                IsActive = true,
                CreatedAt = _dateTimeProvider.GetUtcNow(),
                UpdatedAt = _dateTimeProvider.GetUtcNow()
            };
            customer = await _userRepository.CreateAsync(customer, cancellationToken);
        }

        var numberOfDays = (request.EndDate.ToDateTime(TimeOnly.MinValue) - request.StartDate.ToDateTime(TimeOnly.MinValue)).Days + 1;
        var baseAmount = listing.BasePrice * numberOfDays;
        var platformFee = baseAmount * 0.10m;
        var taxAmount = (baseAmount + platformFee) * 0.18m;
        var totalAmount = baseAmount + platformFee + taxAmount;

        var booking = new Domain.Entities.Booking
        {
            BookingReference = await _bookingRepository.GenerateBookingReferenceAsync(cancellationToken),
            CustomerId = customer.Id,
            VendorId = vendorId,
            ListingId = request.ListingId,
            GuestCount = request.Guests,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            DurationDays = numberOfDays,
            BaseAmount = baseAmount,
            PlatformFee = platformFee,
            TaxAmount = taxAmount,
            TotalAmount = request.Amount > 0 ? request.Amount : totalAmount,
            Status = BookingStatus.Confirmed,
            PaymentStatus = PaymentStatus.Pending,
            VendorStatus = "APPROVED",
            VendorRespondedAt = _dateTimeProvider.GetUtcNow(),
            ConfirmedAt = _dateTimeProvider.GetUtcNow(),
            CreatedAt = _dateTimeProvider.GetUtcNow(),
            UpdatedAt = _dateTimeProvider.GetUtcNow()
        };

        booking.Guests.Add(new BookingGuest
        {
            GuestName = request.CustomerName,
            GuestEmail = request.CustomerEmail,
            GuestPhone = request.CustomerPhone,
            IsPrimaryContact = true,
            CreatedAt = _dateTimeProvider.GetUtcNow()
        });

        booking.Timeline.Add(new BookingTimeline
        {
            StatusTo = BookingStatus.Confirmed.ToString(),
            ChangedBy = vendorId,
            Notes = "Booking created manually by vendor",
            CreatedAt = _dateTimeProvider.GetUtcNow()
        });

        booking = await _bookingRepository.CreateAsync(booking, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new CreateVendorBookingResponse
        {
            Success = true,
            Data = new CreateVendorBookingResponseData
            {
                BookingId = booking.BookingReference,
                Status = "CONFIRMED",
                Message = "Booking created successfully"
            }
        };
    }
}

