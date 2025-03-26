
namespace GamaEdtech.Back.Domain.Entities.School.Aggregates
{
    public class SchoolComment : BaseEntity
    {
        #region Ctors
        private SchoolComment()
        {
            
        }
        private SchoolComment(Guid schoolId, string bodyComment)
        {
            
        }
        #endregion

        #region Properties
        public Guid SchoolId { get; private set; }
        public string BodyComment { get; private set; }
        public double ClassesQualityRate { get; private set; }
        public double EducationRate { get; private set; }
        public double ITTrainingRate { get; private set; }
        public double SafetyAndHappinessRate { get; private set; }
        public double BehaviorRate { get; private set; }
        public double TuitionRatioRate { get; private set; }
        public double FacilitiesRate { get; private set; }
        public double ArtisticActivitiesRate { get; private set; }
        public int LikeCount { get; private set; }
        public int DislikeCount { get; private set; }
        public double AverageRate { get; private set; }
        public Status Status { get; private set; }
        #endregion

        #region Relations
        #region ForeignKey
        public virtual School School { get; private set; }
        #endregion

        #region ICollections

        #endregion
        #endregion

        #region Functionalities

        #endregion
    }

    public enum Status
    {
        Draft,
        Confirmed,
        Rejected,
        Deleted
    }
}