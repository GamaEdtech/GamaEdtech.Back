using GamaEdtech.Back.Domain.Entities.School.Valueobjects;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GamaEdtech.Back.Infrastructure.EntityConfigurations.Converters
{
    public class AddressComparer : ValueComparer<Address>
    {
        public AddressComparer() : base(
            (a, b) => a == b,
            a => a.GetHashCode(),
            a => Address.Create(a.Street, a.City, a.State, a.Country, a.ZipCode))
        { }
    }
}