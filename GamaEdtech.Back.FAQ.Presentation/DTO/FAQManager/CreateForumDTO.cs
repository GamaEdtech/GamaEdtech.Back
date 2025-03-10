namespace GamaEdtech.Back.FAQ.Application.DTO.FAQManager
{
    public record CreateForumDTO(List<string> FaqCategoryTitles, 
        string SummaryOfQuestion, string Question, List<IFormFile> AttachFiles);
}