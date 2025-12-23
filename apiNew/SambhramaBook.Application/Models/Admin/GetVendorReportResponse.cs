namespace SambhramaBook.Application.Models.Admin;

public class GetVendorReportResponse
{
    public bool Success { get; set; }
    public VendorReportData? Data { get; set; }
}

public class VendorReportData
{
    public PeriodDto Period { get; set; } = new();
    public int TotalVendors { get; set; }
    public int ActiveVendors { get; set; }
    public int NewVendors { get; set; }
    public List<VendorReportBreakdownDto> Breakdown { get; set; } = [];
}

public class VendorReportBreakdownDto
{
    public string Month { get; set; } = string.Empty;
    public int New { get; set; }
    public int Active { get; set; }
}

