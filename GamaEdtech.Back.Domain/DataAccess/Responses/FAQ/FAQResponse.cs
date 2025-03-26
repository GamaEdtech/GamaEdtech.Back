using GamaEdtech.Back.Domain.Common.Utilities;
namespace GamaEdtech.Back.Domain.DataAccess.Responses.FAQ
{
    public record FAQResponse
    {
        public Guid Id { get; init; }
        public string SummaryOfQuestion { get; init; }
        public string Question { get; init; }
        public List<FAQCategoryResponse> FAQCategoryTree { get; init; }
        public CustomDateTimeFormat CreateDate { get; init; }
    }
}
