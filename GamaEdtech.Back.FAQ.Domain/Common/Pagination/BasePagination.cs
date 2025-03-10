using GamaEdtech.Back.FAQ.Domain.Common.Sorting;

namespace GamaEdtech.Back.FAQ.Domain.Common.Pagination
{
    public class BasePagination : BaseSort
    {
        public BasePagination(BasePagination basePagination)
        {
            PageId = basePagination.PageId;
            PageCount = basePagination.PageCount;
            ActivePage = basePagination.ActivePage;
            StartPage = basePagination.StartPage;
            EndPage = basePagination.EndPage;
            TakeEntity = basePagination.TakeEntity;
            SkipEntity = basePagination.SkipEntity;
        }
        public BasePagination()
        {
            PageId = 1;
            TakeEntity = 4;
        }
        /// <summary>
        /// Explain: شماره صفحه
        /// </summary>
        public int PageId { get; set; }
        /// <summary>
        /// Explain: تعداد صفحات => مقدار دهی نشود / تاثیری ندارد
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// Explain: شماره صفحه فعلی
        /// </summary>
        public int ActivePage { get; set; }

        /// <summary>
        /// Explain: صفحه شروع
        /// </summary>
        public int StartPage { get; set; }

        /// <summary>
        /// Explain: صفحه پایان
        /// </summary>
        public int EndPage { get; set; }

        /// <summary>
        /// Explain: تعداد نمایش در هر هر صفحه
        /// </summary>
        public int TakeEntity { get; set; }

        /// <summary>
        /// Explain: مقدار دهی نشود/تاثیری ندارد
        /// </summary>
        public int SkipEntity { get; set; }

    }

    public class BasePagination<TPagiationSelectedDto> : BasePagination
       where TPagiationSelectedDto : class

    {
        public BasePagination()
        {
            PageId = 1;
            TakeEntity = 4;
        }

        public virtual TPagiationSelectedDto SetPagination<TPagiationSelectedDto>(BasePagination basePagination)
            where TPagiationSelectedDto : class
        {
            PageId = basePagination.ActivePage;
            PageCount = basePagination.PageCount == int.MinValue ? 0 : basePagination.PageCount;
            StartPage = basePagination.StartPage == int.MinValue ? 0 : basePagination.StartPage;
            EndPage = basePagination.EndPage == int.MinValue ? 0 : basePagination.EndPage;
            TakeEntity = basePagination.TakeEntity;
            SkipEntity = basePagination.SkipEntity;
            ActivePage = basePagination.ActivePage;
            return this as TPagiationSelectedDto;
        }
    }
}
