using Ardalis.Specification;
using GamaEdtech.Back.Domain.Common;
using GamaEdtech.Back.Domain.DataAccess.Requests.Location;
using GamaEdtech.Back.Domain.Entities.Location.Criteria;
using Mapster;

namespace GamaEdtech.Back.Domain.Entities.Location.Specifications
{
    public class GetLocationByDynamicFilterSpecification : BaseSpecification<Location>
    {
        private readonly GetLocationByDynamicFilterRequest _req;

        public GetLocationByDynamicFilterSpecification(GetLocationByDynamicFilterRequest req)
        {
            _req = req;
            Query.Where(Criteria().ToExpression());
        }
        protected override CriteriaSpecification<Location> Criteria()
        {
            return new CheckLocationByLocationTypeCriteria(_req.LocationType)
                .And(new CheckLocationByCodeCriteria(_req.Code))
                .And(new CheckLocationByRadiusCriteria(_req.Coordinates, _req.Radius))
                //.And(new CheckLocationByCoordinateCriteria(_req.Coordinates))
                .And(new CheckLocationByTitleCriteria(_req.Title, _req.LatinTitle))
                .And(new CheckLocationByIdCriteria(_req.LocationId));
        }
    }
}