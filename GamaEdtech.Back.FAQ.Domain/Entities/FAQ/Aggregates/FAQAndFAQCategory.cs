namespace GamaEdtech.Back.FAQ.Domain.Entities.FAQ.Aggregates
{
    public class FAQAndFAQCategory : BaseEntity
    {
        #region Ctors
        private FAQAndFAQCategory()
        {
            
        }
        private FAQAndFAQCategory(Guid faqId, Guid faqCategoryId)
        {
            FAQId = faqId;
            FAQCategoryId = faqCategoryId;
        }
        #endregion

        #region Propeties
        public Guid FAQId { get; private set; }
        public Guid FAQCategoryId { get; private set; }
        #endregion

        #region Relations
        #region ForeignKey
        public virtual FAQ FAQ {  get; private set; }
        public virtual FAQCategory.FAQCategory FAQCategory {  get; private set; }
        #endregion
        #region ICollectiona

        #endregion
        #endregion

        #region Functionalities
        public static FAQAndFAQCategory Create(Guid faqId, Guid faqCategoryId)
        {
            return new FAQAndFAQCategory(faqId, faqCategoryId);
        }
        #endregion

        #region Domain Events

        #endregion
    }
}