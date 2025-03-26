namespace GamaEdtech.Back.Application.DataInitializer
{
    public interface IDataInitializer
    {
        int SortNumber { get; init; }
        Task InitializeData();
    }
}
