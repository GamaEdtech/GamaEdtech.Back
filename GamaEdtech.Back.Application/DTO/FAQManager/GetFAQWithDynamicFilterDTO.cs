using GamaEdtech.Back.Domain.Common.Utilities;
using GamaEdtech.Back.Domain.DataAccess.Requests.FAQ;

namespace GamaEdtech.Back.Application.DTO.FAQManager;

public class GetFAQWithDynamicFilterDTO
{
    public List<string>? FaqCategoriesTitle { get; init; }
    public CustomDateFormat CustomDateFormat { get; init; } = CustomDateFormat.ToSolarDate;
    public CustomOrderBy CustomOrderBy { get; init; }
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }

    public string? SummaryOfQuestion { get; set; }
}
