namespace GamaEdtech.Back.Domain.Common.Sorting
{
    public class BaseSort
    {
        public BaseSort()
        {
            SortType = SortType.BasedOnDecreasingCreationDate;
        }
        public SortType SortType { get; set; }

    }

    public enum SortType
    {
        BasedOnAscendingCreationDate,
        BasedOnDecreasingCreationDate
    }
}
