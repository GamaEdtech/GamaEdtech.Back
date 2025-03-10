using Ardalis.Specification;
using GamaEdtech.Back.FAQ.Domain.Common;
using GamaEdtech.Back.FAQ.Domain.DataAccess.DomainModels;
using GamaEdtech.Back.FAQ.Domain.Entities.FAQ.Criterias;

namespace GamaEdtech.Back.FAQ.Domain.Entities.FAQ.Specifications
{
    public class GetFAQWithDynamicFilterSpecification : BaseSpecification<FAQ>
    {
        private readonly GetFAQWithDynamicFilterRequest _dynamicFilterReq;

        public GetFAQWithDynamicFilterSpecification(GetFAQWithDynamicFilterRequest dynamicFilterReq, params FAQRelations[] fAQRelations)
        {
            _dynamicFilterReq = dynamicFilterReq;
            Query.Where(Criteria().ToExpression());
            OrderBy();
            fAQRelations.ToList().ForEach(GetFAQRelations);
        }
        protected override CriteriaSpecification<FAQ> Criteria()
        {
            return new CheckFAQCategoriesOfFAQCriteria(_dynamicFilterReq.FaqCategoriesTitle)
                .And(new CheckFAQDateTimeCriteria(_dynamicFilterReq.FromDate, _dynamicFilterReq.ToDate));
        }

        private void GetFAQRelations(FAQRelations fAQRelations)
        {
            switch (fAQRelations)
            {
                case FAQRelations.FAQCategory:
                    Query.Include(inc => inc.FAQAndFAQCategories)
                        .ThenInclude(thin => thin.FAQCategory);
                    break;
                default:
                    break;
            }
        }

        private void OrderBy()
        {
            switch (_dynamicFilterReq.CustomOrderBy)
            {
                case CustomOrderBy.OrderByCreateDateAscending:
                    Query.OrderBy(c => c.CreateDate);
                    break;
                case CustomOrderBy.OrderByCreateDateDescending:
                    Query.OrderByDescending(c => c.CreateDate);
                    break;
                default:
                    break;
            }
        }
    }
    public enum FAQRelations
    {
        FAQCategory
    }
}