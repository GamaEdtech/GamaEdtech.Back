using GamaEdtech.Back.Domain.Common.Utilities;
using System.Reflection;

namespace GamaEdtech.Back.Domain.Entities
{
    public interface IEntity
    {
        DateTime CreateDate { get; }
        public bool SoftDeleted { get; }
        DateTime LastUpdatedDate { get; }
    }
    public abstract class BaseEntity<TKey> : IEntity
    {
        protected BaseEntity()
        {
            CreateDate = DateTimeHelper.SystemNow();
        }

        public virtual TKey Id { get; protected set; }
        public virtual DateTime CreateDate { get; private set; }
        public virtual bool SoftDeleted { get; private set; }
        public virtual DateTime LastUpdatedDate { get; private set; }

        public void UpdateLastUpdatedDate()
        {
            LastUpdatedDate = DateTime.Now;
        }

        #region Entity Versioning
        private readonly Dictionary<string, object> _originalValues = [];
        private readonly List<EntityVersion.EntityVersion> _versions = [];
        public IReadOnlyCollection<EntityVersion.EntityVersion> Versions => _versions.AsReadOnly();

        /// <summary>
        /// add init value (at insert or update)
        /// </summary>
        public void CaptureOriginalValues()
        {
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite && p.Name != nameof(Versions));
            foreach (var property in properties)
            {
                var value = property.GetValue(this);

                if (value != null && IsComplexType(property.PropertyType))
                {
                    var complexValues = GetComplexTypeValues(value);
                    _originalValues[property.Name] = complexValues;
                }
                else
                {
                    _originalValues[property.Name] = value;
                }
            }
        }

        public void TrackChanges()
        {
            var currentUserId = GetCurrentUserId();
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite && p.Name != nameof(Versions));

            foreach (var property in properties)
            {
                var originalValue = _originalValues.TryGetValue(property.Name, out object? value) ? value : null;
                var currentValue = property.GetValue(this);

                if (IsComplexType(property.PropertyType))
                {
                    var originalComplexValues = originalValue != null ? GetComplexTypeValues(originalValue) : null;
                    var currentComplexValues = currentValue != null ? GetComplexTypeValues(currentValue) : null;

                    if (!AreDictionariesEqual(originalComplexValues, currentComplexValues))
                    {
                        _versions.Add(EntityVersion.EntityVersion
                            .Create((Guid)(object)Id, GetType().Name, currentUserId,
                            property.Name, SerializeComplexType(originalComplexValues), SerializeComplexType(currentComplexValues)));
                    }
                }
                else
                {
                    if (!Equals(originalValue, currentValue))
                    {
                        _versions.Add(EntityVersion.EntityVersion
                            .Create((Guid)(object)Id, GetType().Name, currentUserId,
                            property.Name, originalValue?.ToString(), currentValue?.ToString()));

                        _originalValues[property.Name] = currentValue;
                    }
                }
            }
        }

        /// <summary>
        /// get userId with override
        /// </summary>
        protected virtual Guid GetCurrentUserId()
        {
            return Guid.Empty;
        }

        public void ClearVersions()
        {
            _versions.Clear();
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Determines if a type is a complex type (not a primitive or string).
        /// </summary>
        private bool IsComplexType(Type type)
        {
            return !(type.IsPrimitive || type == typeof(string) || type == typeof(decimal));
        }

        /// <summary>
        /// Recursively captures values of a complex object.
        /// </summary>
        private Dictionary<string, object> GetComplexTypeValues(object complexObject)
        {
            var result = new Dictionary<string, object>();
            var properties = complexObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite);

            foreach (var property in properties)
            {
                var value = property.GetValue(complexObject);

                if (value != null && IsComplexType(property.PropertyType))
                {
                    // Recursive call for nested complex types
                    result[property.Name] = GetComplexTypeValues(value);
                }
                else
                {
                    // Store simple values directly
                    result[property.Name] = value;
                }
            }

            return result;
        }

        private bool AreDictionariesEqual(Dictionary<string, object>? dict1, Dictionary<string, object>? dict2)
        {
            if (dict1 == null && dict2 == null) return true;
            if (dict1 == null || dict2 == null) return false;
            if (dict1.Count != dict2.Count) return false;

            return dict1.All(kvp => dict2.ContainsKey(kvp.Key) && Equals(kvp.Value, dict2[kvp.Key]));
        }
        private string SerializeComplexType(Dictionary<string, object>? complexValues)
        {
            return complexValues == null
                ? string.Empty
                : string.Join(", ", complexValues.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
        }
        #endregion
    }

    public abstract class BaseEntity : BaseEntity<Guid>
    {
    }
}