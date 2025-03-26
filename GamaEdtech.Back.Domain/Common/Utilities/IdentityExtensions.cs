using System.Globalization;
using System.Security.Claims;
using System.Security.Principal;

namespace GamaEdtech.Back.Domain.Common.Utilities
{
    public static class IdentityExtensions
    {
        public static string FindFirstValue(this ClaimsIdentity identity, string claimType)
        {
            return identity?.FindFirst(claimType)?.Value;
        }

        public static string FindFirstValue(this IIdentity identity, string claimType)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            return claimsIdentity?.FindFirstValue(claimType);
        }

        public static string GetUserId(this IIdentity identity)
        {
            return identity?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        public static long GetLongTypeUserId(this IIdentity identity)
        {
            string userId = identity?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null || userId is "")
                return 0;
            else
                return long.Parse(userId);
        }
        public static string GetUserFullName(this IIdentity identity)
        {
            return identity?.FindFirstValue(ClaimTypes.Name);
        }

        public static T GetUserId<T>(this IIdentity identity) where T : IConvertible
        {
            var userId = identity?.GetUserId();
            return userId.HasValue()
                ? (T)Convert.ChangeType(userId, typeof(T), CultureInfo.InvariantCulture)
                : default;
        }
        public static Guid GetGuidTypeUserId(this IIdentity identity)
        {
            string userId = identity?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Guid.Empty;
            else
                return Guid.Parse(userId);
        }

        public static string GetUserName(this IIdentity identity)
        {
            return identity?.FindFirstValue(ClaimTypes.Name);
        }
    }
}
