using GamaEdtech.Back.Domain.Entities.School.Valueobjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace GamaEdtech.Back.Infrastructure.EntityConfigurations.Converters
{
    public class AddressConverter : ValueConverter<Address, string>
    {
        public AddressConverter() : base(
            address => JsonSerializer.Serialize(address, (JsonSerializerOptions)null),
            value => JsonSerializer.Deserialize<Address>(value, (JsonSerializerOptions)null))
        {
        }
    }
}