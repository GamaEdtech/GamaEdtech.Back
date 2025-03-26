using GamaEdtech.Back.Domain.Common.Utilities;
namespace GamaEdtech.Back.Domain.Entities.EntityVersion
{
    public class EntityVersion : IEntity
    {
        #region Ctors
        private EntityVersion() {}
        private EntityVersion(Guid entityId, string entityType, Guid userId,
            string propertyName, string oldValue, string newValue)
        {
            EntityId = entityId;
            EntityType = entityType;
            UserId = userId;
            OldValue = oldValue;
            NewValue = newValue;
            PropertyName = propertyName;

            CreateDate = DateTimeHelper.SystemNow(); 
        }
        #endregion

        #region Propeties
        public Guid Id { get; private set; }
        public Guid EntityId { get; private set; }
        public string EntityType { get; private set; }
        public Guid UserId { get; private set; }

        public string PropertyName { get; private set; }
        public string OldValue { get; private set; }
        public string NewValue { get; private set; }

        public DateTime CreateDate { get; private set; }
        public DateTime LastUpdatedDate { get; private set; }

        public bool SoftDeleted { get; private set; }
        #endregion

        #region Relations
        #region ForeignKey

        #endregion

        #region ICollection

        #endregion
        #endregion

        #region Functionalities
        public static EntityVersion Create(Guid entityId, string entityType, Guid userId, 
            string properyName, string oldValue, string newValue)
        {
            return new EntityVersion(entityId, entityType, userId, properyName,
                oldValue, newValue);
        }
        #endregion

    }
}