using Microsoft.Extensions.Logging;
using SambhramaBook.Application.Models.Booking;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Domain.Entities;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Booking;

public class CreateBookingHandler
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IListingRepository _listingRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<CreateBookingHandler> _logger;

    public CreateBookingHandler(
        IBookingRepository bookingRepository,
        IListingRepository listingRepository,
        IUserRepository userRepository,
        ILogger<CreateBookingHandler> logger)
    {
        _bookingRepository = bookingRepository;
        _listingRepository = listingRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<BookingDto> HandleAsync(CreateBookingRequest request, long customerId, CancellationToken cancellationToken = default)
    {
        // Validate listing exists and is available
        var listing = await _listingRepository.GetByIdAsync(request.ListingId, cancellationToken);
        if (listing == null)
        {
            throw new InvalidOperationException("Listing not found");
        }

        if (listing.Status != ListingStatus.Approved || listing.ApprovalStatus != ApprovalStatus.Approved)
        {
            throw new InvalidOperationException("Listing is not available for booking");
        }

        // Check availability
        var isAvailable = await CheckAvailabilityAsync(request.ListingId, request.StartDate, request.EndDate, cancellationToken);
        if (!isAvailable)
        {
            throw new InvalidOperationException("Listing is not available for the selected dates");
        }

        // Get customer
        var customer = await _userRepository.GetByIdAsync(customerId, cancellationToken);
        if (customer == null)
        {
            throw new InvalidOperationException("Customer not found");
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

        // Create booking (reference is already set, so CreateAsync won't regenerate)
        booking = await _bookingRepository.CreateAsync(booking, cancellationToken);

        // Add timeline entry
        booking.Timeline.Add(new Domain.Entities.BookingTimeline
        {
            StatusTo = BookingStatus.Pending.ToString(),
            Notes = "Booking created",
            CreatedAt = DateTime.UtcNow
        });

        await _bookingRepository.UpdateAsync(booking, cancellationToken);

        return new BookingDto
        {
            Id = booking.Id,
            BookingReference = booking.BookingReference,
            ListingId = listing.Id,
            ListingName = listing.Title,
            ListingImageUrl = listing.Images.FirstOrDefault(img => img.IsPrimary)?.ImageUrl,
            Location = $"{listing.City}, {listing.State}",
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            Days = booking.DurationDays,
            GuestCount = booking.GuestCount,
            TotalAmount = booking.TotalAmount,
            Status = booking.Status,
            PaymentStatus = booking.PaymentStatus,
            EventType = booking.EventType,
            CreatedAt = booking.CreatedAt
        };
    }

    private async Task<bool> CheckAvailabilityAsync(long listingId, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken)
    {
        // Check if dates are blocked in availability table
        // Check if dates overlap with existing bookings
        // Return true if available
        return true; // Simplified for now
    }
}

