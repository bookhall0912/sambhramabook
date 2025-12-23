using Microsoft.Extensions.Logging;
using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Booking;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Entities;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Booking;

public class CreateBookingHandler
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IListingRepository _listingRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<CreateBookingHandler> _logger;

    public CreateBookingHandler(
        IBookingRepository bookingRepository,
        IListingRepository listingRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ILogger<CreateBookingHandler> logger)
    {
        _bookingRepository = bookingRepository;
        _listingRepository = listingRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<CreateBookingResponse> HandleAsync(long customerId, CreateBookingRequest request, CancellationToken cancellationToken = default)
    {

        // Validate listing exists and is available
        var listing = await _listingRepository.GetByIdAsync(request.ListingId, cancellationToken);
        if (listing == null)
        {
            return new CreateBookingResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Listing not found" }
            };
        }

        if (listing.Status != ListingStatus.Approved || listing.ApprovalStatus != ApprovalStatus.Approved)
        {
            return new CreateBookingResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "UNAVAILABLE", Message = "Listing is not available for booking" }
            };
        }

        // Check availability
        var isAvailable = await CheckAvailabilityAsync(request.ListingId, request.StartDate, request.EndDate, cancellationToken);
        if (!isAvailable)
        {
            return new CreateBookingResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "UNAVAILABLE", Message = "Listing is not available for the selected dates" }
            };
        }

        // Get customer
        var customer = await _userRepository.GetByIdAsync(customerId, cancellationToken);
        if (customer == null)
        {
            return new CreateBookingResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Customer not found" }
            };
        }

        // Calculate pricing
        var baseAmount = listing.BasePrice * request.NumberOfDays;
        var platformFee = baseAmount * 0.10m; // 10% platform fee
        var taxAmount = (baseAmount + platformFee) * 0.18m; // 18% GST
        var totalAmount = baseAmount + platformFee + taxAmount;

        // Generate booking reference
        var bookingReference = await _bookingRepository.GenerateBookingReferenceAsync(cancellationToken);

        // Create booking
        var booking = new Domain.Entities.Booking
        {
            BookingReference = bookingReference,
            CustomerId = customerId,
            VendorId = listing.VendorId,
            ListingId = request.ListingId,
            EventType = request.EventType,
            EventName = request.EventName,
            GuestCount = request.Guests,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            DurationDays = request.NumberOfDays,
            ServicePackageId = request.ServicePackageId,
            BaseAmount = baseAmount,
            PlatformFee = platformFee,
            TaxAmount = taxAmount,
            TotalAmount = totalAmount,
            Status = BookingStatus.Pending,
            PaymentStatus = PaymentStatus.Pending,
            SpecialRequirements = request.SpecialRequests,
            VendorStatus = "PENDING",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Add primary guest
        booking.Guests.Add(new Domain.Entities.BookingGuest
        {
            GuestName = request.ContactInfo.Name,
            GuestEmail = request.ContactInfo.Email,
            GuestPhone = request.ContactInfo.Phone,
            IsPrimaryContact = true,
            CreatedAt = DateTime.UtcNow
        });

        // Add timeline entry
        booking.Timeline.Add(new Domain.Entities.BookingTimeline
        {
            StatusTo = BookingStatus.Pending.ToString(),
            Notes = "Booking created",
            CreatedAt = _dateTimeProvider.GetUtcNow()
        });

        // Create booking
        booking = await _bookingRepository.CreateAsync(booking, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new CreateBookingResponse
        {
            Success = true,
            Data = new CreateBookingResponseData
            {
                BookingId = booking.BookingReference,
                HallId = listing.Id.ToString(),
                HallName = listing.Title,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                NumberOfDays = booking.DurationDays,
                Guests = booking.GuestCount,
                TotalAmount = booking.TotalAmount,
                Status = booking.Status.ToString().ToLower(),
                ConfirmationNumber = booking.BookingReference,
                Message = "Booking submitted successfully"
            }
        };
    }

    private async Task<bool> CheckAvailabilityAsync(long listingId, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken)
    {
        // Check if dates overlap with existing bookings
        var existingBookings = await _bookingRepository.GetByListingIdAsync(listingId, cancellationToken);
        var hasConflict = existingBookings.Any(b => 
            b.Status != BookingStatus.Cancelled &&
            b.StartDate <= endDate && 
            b.EndDate >= startDate);
        
        return !hasConflict;
    }
}


