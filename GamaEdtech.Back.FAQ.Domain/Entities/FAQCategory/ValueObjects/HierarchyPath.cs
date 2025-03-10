namespace GamaEdtech.Back.FAQ.Domain.Entities.FAQCategory.ValueObjects
{
    public record HierarchyPath(string Value)
    {
        public static HierarchyPath Root() => new("/");

        public static HierarchyPath FromString(string value)
        {
            return new HierarchyPath(value);
        }

        public int GetLevel() => Value.Count(c => c == '/') - 1;

        public HierarchyPath GetDescendant(string defaultSegment, HierarchyPath? lastChild = null)
        {
            string newSegment;
            if (lastChild == null)
            {
                newSegment = defaultSegment;
            }
            else
            {
                var trimmed = lastChild.Value.TrimEnd('/');
                var segments = trimmed.Split('/');
                var lastSegment = segments.Last();
                if (int.TryParse(lastSegment, out int lastNumber))
                {
                    newSegment = (lastNumber + 1).ToString();
                }
                else
                {
                    throw new InvalidOperationException("بخش انتهایی مسیر نامعتبر است.");
                }
            }
            var newPath = $"{Value}{newSegment}/";
            return new HierarchyPath(newPath);
        }

        public override string ToString() => Value;
    }
}