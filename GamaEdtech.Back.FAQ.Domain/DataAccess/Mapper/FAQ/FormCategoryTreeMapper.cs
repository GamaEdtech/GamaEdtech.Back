using GamaEdtech.Back.FAQ.Domain.Common.Utilities;
using GamaEdtech.Back.FAQ.Domain.Entities.FAQCategory;

namespace GamaEdtech.Back.FAQ.Domain.DataAccess.Mapper.FAQ
{
    public record FAQCategoryResult
    {
        public Guid Id { get; init; }
        public required string Title { get; init; }
        public CustomDateTimeFormat CreateDate { get; init; }
        public List<FAQCategoryResult> Children { get; init; } = [];
    }
    public static class FAQCategoryTreeMapper
    {
        public static List<FAQCategoryResult> MapToResult(this List<FAQCategoryTree> trees, CustomDateFormat customDateFormat)
        {
            var results = new List<FAQCategoryResult>(trees.Count);
            foreach (var node in trees)
            {
                results.Add(MapNode(node, customDateFormat));
            }
            return results;
        }

        private static FAQCategoryResult MapNode(FAQCategoryTree node, CustomDateFormat customDateFormat)
        {
            //create from root
            var result = new FAQCategoryResult
            {
                Id = node.Category.Id,
                Title = node.Category.Title,
                CreateDate = node.Category.CreateDate.ConvertToCustomDate(customDateFormat)
            };

            // recursive map from node with have child 
            if (node.Children != null && node.Children.Count > 0)
            {
                var children = new List<FAQCategoryResult>(node.Children.Count);
                foreach (var child in node.Children)
                {
                    children.Add(MapNode(child, customDateFormat));
                }
                // create immutable child  record
                result = result with { Children = children };
            }

            return result;
        }
    }
}