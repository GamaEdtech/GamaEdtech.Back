using GamaEdtech.Back.FAQ.Domain.Entities.FAQCategory;

namespace GamaEdtech.Back.FAQ.Application.DTO.FAQManager
{
    public class CreateFAQCategoryDTO
    {
        #region Properties
        public string? ParentCategoryTitle { get; init; }
        public required string Title { get; init; }
        public required CategoryType CategoryType { get; init; }
        #endregion
    }

    public class FAQCategorySelectedDTO
    {
        public required string Title { get; init; }
        public required CategoryType CategoryType { get; init; }
    }
}