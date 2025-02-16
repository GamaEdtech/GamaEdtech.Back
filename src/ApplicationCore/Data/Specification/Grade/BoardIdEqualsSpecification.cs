namespace GamaEdtech.Backend.Data.Specification.Grade
{
    using System.Linq.Expressions;

    using GamaEdtech.Backend.Common.DataAccess.Specification;

    using GamaEdtech.Backend.Data.Entity;

    public sealed class BoardIdEqualsSpecification(int boardId) : SpecificationBase<Grade>
    {
        public override Expression<Func<Grade, bool>> Expression() => (t) => t.BoardId == boardId;
    }
}
