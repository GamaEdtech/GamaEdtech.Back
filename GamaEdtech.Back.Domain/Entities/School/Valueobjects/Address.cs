using GamaEdtech.Back.Domain.Common.Exceptions;
using GamaEdtech.Back.Domain.Common.Utilities;

namespace GamaEdtech.Back.Domain.Entities.School.Valueobjects
{
    public class Address : IEquatable<Address>
    {
        public string Street { get; }
        public string City { get; }
        public string State { get; }
        public string Country { get; }
        public string ZipCode { get; }

        private Address(string street, string city, string state, string country, string zipCode)
        {
            if (string.IsNullOrWhiteSpace(street))
                throw new BadRequestException("Street cannot be empty.", nameof(street));

            if (string.IsNullOrWhiteSpace(city))
                throw new BadRequestException("City cannot be empty.", nameof(city));

            if (string.IsNullOrWhiteSpace(state))
                throw new BadRequestException("State cannot be empty.", nameof(state));

            if (string.IsNullOrWhiteSpace(country))
                throw new BadRequestException("Country cannot be empty.", nameof(country));

            if (string.IsNullOrWhiteSpace(zipCode))
                throw new BadRequestException("ZipCode cannot be empty.", nameof(zipCode));

            Street = street;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipCode;
        }

        public static Address Create(string street, string city, string state, string country, string zipCode)
        {
           
            return new Address(street, city, state, country, zipCode);
        }

        public static Address FromString(string address)
        {
            if (!address.HasValue()) throw new BadRequestException("address cannot be empty");

            var addressArray = address.Split(',');
            return new Address(addressArray[0], addressArray[1], addressArray[2], addressArray[3], addressArray[4]);
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Address);
        }

        public bool Equals(Address other)
        {
            return other != null &&
                   Street == other.Street &&
                   City == other.City &&
                   State == other.State &&
                   Country == other.Country &&
                   ZipCode == other.ZipCode;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Street, City, State, Country, ZipCode);
        }

        public override string ToString()
        {
            return $"{Street}, {City}, {State}, {Country}, {ZipCode}";
        }

        public static bool operator ==(Address left, Address right)
        {
            return EqualityComparer<Address>.Default.Equals(left, right);
        }

        public static bool operator !=(Address left, Address right)
        {
            return !(left == right);
        }
    }
}