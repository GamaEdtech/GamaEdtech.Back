namespace GamaEdtech.Back.Domain.DataAccess.Repositories.FAQCategory
{
    public interface IFAQCategoryRepository : IBaseRepository<Entities.FAQCategory.FAQCategory>
    {
        Task<IReadOnlyCollection<Entities.FAQCategory.FAQCategory>> ListAsyncWithSecondaryLevelCache(CancellationToken cancellationToken);
    }
}