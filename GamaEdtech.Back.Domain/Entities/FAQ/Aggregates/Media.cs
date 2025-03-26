namespace GamaEdtech.Back.Domain.Entities.FAQ.Aggregates
{
    public class Media : BaseEntity
    {
        #region Ctors
        private Media()
        {

        }
        private Media(string fileName, string fileAddress, string contentType, MediaEntity mediaEntity, Guid mediaEntityId)
        {
            FileName = fileName;
            FileAddress = fileAddress;
            ContentType=contentType;
            MediaEntity = mediaEntity;
            MediaEntityId = mediaEntityId;
        }
        #endregion

        #region Propeties
        public string FileName { get; private set; }
        public string FileAddress { get; private set; }
        public string ContentType { get; private set; }
        public MediaType MediaType { get; private set; }
        public MediaEntity MediaEntity { get; private set; }
        public Guid MediaEntityId { get; private set; }
        #endregion

        #region Relations
        #endregion

        #region Functionalities
        public static Media Create(string fileName, string fileAddress,
            MediaEntity mediaEntity, Guid mediaEntityId, string contentType)
        {
            return new Media(fileName, fileAddress, contentType, mediaEntity, mediaEntityId);
        }
        #endregion

        #region Domain Events

        #endregion
    }

    public enum MediaType
    {
        None,
        Photo
    }
    public enum MediaEntity
    {
        FAQ
    }
}