using GamaEdtech.Back.Domain.Common.Utilities;
using GamaEdtech.Back.Domain.DataAccess.Responses.FAQ;
using GamaEdtech.Back.Domain.Entities.FAQCategory;

namespace GamaEdtech.Back.Domain.DataAccess.Mapper.FAQ
{
    public static class FAQCategoryTreeMapperExtension
    {
        public static List<FAQCategoryResponse> MapToResult(this List<FAQCategoryTree> tree, CustomDateFormat customDateFormat)
        {
            var results = new List<FAQCategoryResponse>(tree.Count);
            foreach (var node in tree)
            {
                results.Add(MapNode(node, customDateFormat));
            }
            return results;
        }

        private static FAQCategoryResponse MapNode(FAQCategoryTree node, CustomDateFormat customDateFormat)
        {
            //create from root
            var result = new FAQCategoryResponse
            {
                Id = node.Category.Id,
                Title = node.Category.Title,
                CreateDate = node.Category.CreateDate.ConvertToCustomDate(customDateFormat)
            };

            // recursive map from node with have child 
            if (node.Children != null && node.Children.Count > 0)
            {
                var children = new List<FAQCategoryResponse>(node.Children.Count);
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