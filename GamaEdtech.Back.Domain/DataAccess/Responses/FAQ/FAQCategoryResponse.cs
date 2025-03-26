using GamaEdtech.Back.Domain.Common.Utilities;

namespace GamaEdtech.Back.Domain.DataAccess.Responses.FAQ
{
    public record FAQCategoryResponse
    {
        public Guid Id { get; init; }
        public required string Title { get; init; }
        public CustomDateTimeFormat CreateDate { get; init; }
        public List<FAQCategoryResponse> Children { get; init; } = [];
    }
}