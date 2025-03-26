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
                _originalValues[property.Name] = property.GetValue(this);
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

                if (!Equals(originalValue, currentValue))
                {
                    _versions.Add(EntityVersion.EntityVersion
                        .Create((Guid)(object)Id, GetType().Name, currentUserId,
                        property.Name, originalValue?.ToString(), currentValue?.ToString()));

                    // Update original Value
                    _originalValues[property.Name] = currentValue;
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

    }

    public abstract class BaseEntity : BaseEntity<Guid>
    {
    }
}