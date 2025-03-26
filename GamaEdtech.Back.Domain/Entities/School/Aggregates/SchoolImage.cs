namespace GamaEdtech.Back.Domain.Entities.School.Aggregates
{
    public class SchoolImage : BaseEntity
    {
        #region Ctors
        private SchoolImage() {}
        private SchoolImage(Guid shoolId, string fileId, FileType fileType)
        {
            SchoolId = shoolId;
            FileId = fileId;
            FileType = fileType;
            Status = Status.Draft;
        }
        #endregion

        #region Propeties
        public Guid SchoolId { get; private set; }
        public string FileId { get; private set; }
        public FileType FileType { get; private set; }
        public Status Status { get; private set; }
        #endregion

        #region Relations
        #region ForeignKey
        public virtual School School { get; private set; }
        #endregion
        #endregion

        #region Functionalities
        public static SchoolImage Create(Guid schoolId, string fileId, FileType fileType)
        {
            return new SchoolImage(schoolId, fileId, fileType);
        }
        #endregion
    }

    public enum FileType
    {
        SimpleImage,
        Tour360
    }
}
