using GamaEdtech.Back.FAQ.Domain.Common;
using GamaEdtech.Back.FAQ.Domain.Entities.FAQ.Aggregates;

namespace GamaEdtech.Back.FAQ.Domain.Entities.FAQ
{
    public class FAQ : AggregateRoot
    {
        #region Ctors
        private FAQ()
        {
            
        }
        private FAQ(string summaryOfQuestion, string question, List<FAQCategory.FAQCategory> fAQCategories)
        {
            SummaryOfQuestion = summaryOfQuestion;
            Question = question;
            _fAQAndFAQCategories = fAQCategories.Select(s => FAQAndFAQCategory.Create(Id, s.Id)).ToList();
        }
        #endregion

        #region Propeties
        public string SummaryOfQuestion { get; private set; }
        public string Question { get; private set; }
        #endregion

        #region Relation
        #region ForeignKey
        #endregion

        #region ICollaction
        private readonly List<FAQAndFAQCategory> _fAQAndFAQCategories;
        public IReadOnlyCollection<FAQAndFAQCategory> FAQAndFAQCategories => _fAQAndFAQCategories;

        private readonly List<Media> _media;
        public IReadOnlyCollection<Media> Media => _media;
        #endregion
        #endregion

        #region Functionalities
        public static FAQ Create(string summaryOfQuestion, string Question, List<FAQCategory.FAQCategory> fAQCategories)
        {
            return new FAQ(summaryOfQuestion, Question, fAQCategories) 
            {
                Id = Guid.NewGuid()
            };
        }

        public void AddMedia(IEnumerable<Media> media)
        {
            if (true)
            {
                //TODO: Validate Media
            }
            _media.AddRange(media);
        }
        #endregion

        #region Domain Events

        #endregion
    }
}