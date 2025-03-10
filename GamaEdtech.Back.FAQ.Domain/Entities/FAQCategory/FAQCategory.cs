using GamaEdtech.Back.FAQ.Domain.Common;
using GamaEdtech.Back.FAQ.Domain.Entities.FAQ.Aggregates;
using GamaEdtech.Back.FAQ.Domain.Entities.FAQCategory.ValueObjects;

namespace GamaEdtech.Back.FAQ.Domain.Entities.FAQCategory
{
    public class FAQCategory : AggregateRoot
    {
        #region Ctors
        private FAQCategory() { }
        private FAQCategory(string title, CategoryType categoryType, HierarchyPath hierarchyPath)
        {
            Title = title;
            CategoryType=categoryType;
            HierarchyPath = hierarchyPath;
            _fAQAndFAQCategories = [];
        }
        #endregion

        #region Propeties
        public string Title { get; private set; }
        public CategoryType CategoryType { get; private set; }
        public HierarchyPath HierarchyPath { get; private set; }
        #endregion

        #region Relation
        #region ForeignKey
        #endregion

        #region ICollaction
        private readonly List<FAQAndFAQCategory> _fAQAndFAQCategories;
        public IReadOnlyCollection<FAQAndFAQCategory> FAQAndFAQCategories => _fAQAndFAQCategories;
        #endregion
        #endregion

        #region Functionalities
        public static FAQCategory Create(string title, CategoryType categoryType, FAQCategory? parent = null)
        {
            var defaultSegment = categoryType switch
            {
                CategoryType.Board => "1",
                CategoryType.Grade => "2",
                CategoryType.Subject => "3",
                CategoryType.Topic => "4",
                _ => throw new ArgumentException("Invalid CategoryType", nameof(categoryType))
            };

            HierarchyPath newPath;

            if (parent == null)
            {
                if (categoryType != CategoryType.Board)
                    throw new InvalidOperationException("دسته‌بندی ریشه (بدون والد) باید از نوع Board باشد.");

                newPath = HierarchyPath.FromString($"/{defaultSegment}/");
            }
            else
            {
                switch (categoryType)
                {
                    case CategoryType.Grade:
                        if (parent.CategoryType != CategoryType.Board)
                            throw new InvalidOperationException("دسته‌بندی Grade باید زیرمجموعه یک دسته‌بندی Board باشد.");
                        break;
                    case CategoryType.Subject:
                        if (parent.CategoryType != CategoryType.Grade)
                            throw new InvalidOperationException("دسته‌بندی Subject باید زیرمجموعه یک دسته‌بندی Grade باشد.");
                        break;
                    case CategoryType.Topic:
                        if (parent.CategoryType != CategoryType.Subject && parent.CategoryType != CategoryType.Topic)
                            throw new InvalidOperationException("دسته‌بندی Topic باید زیرمجموعه یک دسته‌بندی Subject یا Topic باشد.");
                        break;
                }

                newPath = parent.HierarchyPath.GetDescendant(defaultSegment, null);
            }

            return new FAQCategory(title, categoryType, newPath);
        }
        public static List<FAQCategoryTree> BuildHierarchyTree(IEnumerable<FAQCategory> categories)
        {
            var nodes = categories
                .Select(c => new FAQCategoryTree(c))
                .GroupBy(n => n.Category.HierarchyPath.Value)
                .ToDictionary(g => g.Key, g => g.ToList());

            var roots = new List<FAQCategoryTree>();

            foreach (var group in nodes.Values)
            {
                foreach (var node in group)
                {
                    var parentPath = GetParentPath(node.Category.HierarchyPath.Value);
                    if (!string.IsNullOrEmpty(parentPath) && nodes.TryGetValue(parentPath, out var parentGroup))
                    {
                        parentGroup.First().Children.Add(node);
                    }
                    else
                    {
                        roots.Add(node);
                    }
                }
            }

            return roots;
        }
        private static string? GetParentPath(string path)
        {
            var trimmed = path.TrimEnd('/');
            int lastIndex = trimmed.LastIndexOf('/');
            if (lastIndex <= 0) return null;
            return trimmed[..(lastIndex + 1)];
        }
        #endregion

        #region Domain Events

        #endregion
    }

    #region Enums
    public enum CategoryType
    {
        Board = 0,
        Grade = 1,
        Subject = 2,
        Topic = 3
    }
    #endregion

    public record FAQCategoryTree(FAQCategory Category)
    {
        public List<FAQCategoryTree> Children { get; init; } = [];
    }
}