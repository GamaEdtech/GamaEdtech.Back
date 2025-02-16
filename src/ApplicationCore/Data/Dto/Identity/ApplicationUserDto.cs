namespace GamaEdtech.Backend.Data.Dto.Identity
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Backend.Common.DataAccess.Entities;

    public class ApplicationUserDto : Common.Mapping.IRegister, IEnablable<ApplicationUserDto>
    {
        public int Id { get; set; }

        public string? UserName { get; set; }

        public string? SecurityStamp { get; set; }

        public string? Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string? PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public DateTimeOffset? RegistrationDate { get; set; }

        public bool Enabled { get; set; }

        public void Register([NotNull] Common.Mapping.TypeAdapterConfig config) => _ = config.ForType<ApplicationUserDto, Entity.Identity.ApplicationUser>();
    }
}
