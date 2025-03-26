using GamaEdtech.Back.Domain.Entities;

namespace GamaEdtech.Back.Domain.Common.Sorting
{
    public static class SortBuilder
    {
        public static BaseSort BuildSort<T>(this IQueryable<T> queryableEntity, SortType sortType)
        where T : class
        {
            return new BaseSort
            {
                SortType = sortType
            };
        }
        public static IQueryable<T> Sort<T>(this IQueryable<T> queryable, BaseSort baseSort)
            where T : IEntity
        {
            return baseSort.SortType switch
            {
                SortType.BasedOnDecreasingCreationDate => queryable.OrderByDescending(c => c.CreateDate),
                SortType.BasedOnAscendingCreationDate => queryable.OrderBy(c => c.CreateDate),
                _ => queryable,
            };
        }
    }
}
