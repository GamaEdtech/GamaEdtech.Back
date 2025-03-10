using System.Globalization;
namespace GamaEdtech.Back.FAQ.Domain.Common.Utilities
{
    public static class DateTimeExtentions
    {
        public static DateTime SystemNow()
            => DateTime.Now;
        public static string ToShamsiDate(this DateTime Value)
        {
            PersianCalendar pc = new();
            return pc.GetYear(Value) + "/" + pc.GetMonth(Value) + "/" + pc.GetDayOfMonth(Value);
        }
        public static string ToShamsiDate(this DateTimeOffset Value)
        {
            PersianCalendar pc = new();
            return pc.GetYear(Value.DateTime) + "/" + pc.GetMonth(Value.DateTime) + "/" + pc.GetDayOfMonth(Value.DateTime);
        }
        public static string ToPersionExactTime(this DateTime Value)
        {
            PersianCalendar pc = new();
            return
                pc.GetHour(Value).ToString() + ":"
                + pc.GetMinute(Value).ToString();
        }
        public static string ToShamsiDateAndExactTime(this DateTime Value)
        {
            PersianCalendar pc = new();
            return $"[{pc.GetYear(Value)}/{pc.GetMonth(Value)}/{pc.GetDayOfMonth(Value)} ({pc.GetHour(Value)}:{pc.GetMinute(Value)}:{pc.GetSecond(Value)})]";
        }
        private static SolarDayOfWeek? ToSolarDayOfWeek(this DayOfWeek dayOfWeek)
        {
            return dayOfWeek switch
            {
                DayOfWeek.Sunday => (SolarDayOfWeek?)SolarDayOfWeek.یکشنبه,
                DayOfWeek.Monday => (SolarDayOfWeek?)SolarDayOfWeek.دوشنبه,
                DayOfWeek.Tuesday => (SolarDayOfWeek?)SolarDayOfWeek.سهشنبه,
                DayOfWeek.Wednesday => (SolarDayOfWeek?)SolarDayOfWeek.چهارشنبه,
                DayOfWeek.Thursday => (SolarDayOfWeek?)SolarDayOfWeek.پنجشنبه,
                DayOfWeek.Friday => (SolarDayOfWeek?)SolarDayOfWeek.جمعه,
                DayOfWeek.Saturday => (SolarDayOfWeek?)SolarDayOfWeek.شنبه,
                _ => null,
            };
        }
        private static SolarMonthName? ToSolarMonthName(this int monthNumber)
        {
            return monthNumber switch
            {
                1 => (SolarMonthName?)SolarMonthName.فروردین,
                2 => (SolarMonthName?)SolarMonthName.اردیبهشت,
                3 => (SolarMonthName?)SolarMonthName.خرداد,
                4 => (SolarMonthName?)SolarMonthName.تیر,
                5 => (SolarMonthName?)SolarMonthName.مرداد,
                6 => (SolarMonthName?)SolarMonthName.شهریور,
                7 => (SolarMonthName?)SolarMonthName.مهر,
                8 => (SolarMonthName?)SolarMonthName.آبان,
                9 => (SolarMonthName?)SolarMonthName.آذر,
                10 => (SolarMonthName?)SolarMonthName.دی,
                11 => (SolarMonthName?)SolarMonthName.بهمن,
                12 => (SolarMonthName?)SolarMonthName.اسفند,
                _ => null,
            };
        }
        public static LunarMonthName? ToLunarMonthName(this int monthNumber)
        {
            return monthNumber switch
            {
                1 => (LunarMonthName?)LunarMonthName.محرم,
                2 => (LunarMonthName?)LunarMonthName.صفر,
                3 => (LunarMonthName?)LunarMonthName.ربیعالاوّل,
                4 => (LunarMonthName?)LunarMonthName.ربیعالثانی,
                5 => (LunarMonthName?)LunarMonthName.جُمادیالاَوَّل,
                6 => (LunarMonthName?)LunarMonthName.جُمادیالثّانی,
                7 => (LunarMonthName?)LunarMonthName.رَجَب,
                8 => (LunarMonthName?)LunarMonthName.شعبان,
                9 => (LunarMonthName?)LunarMonthName.رمضان,
                10 => (LunarMonthName?)LunarMonthName.شوال,
                11 => (LunarMonthName?)LunarMonthName.ذیقعده,
                12 => (LunarMonthName?)LunarMonthName.ذیالحِجّه,
                _ => null,
            };
        }
        public static CustomDateTimeFormat ToCustomShamsiDateAndExactTime(this DateTime Value, CustomDateFormat customDateFormat)
        {
            PersianCalendar pc = new();
            return new CustomDateTimeFormat
            {
                DayOfWeak = pc.GetDayOfWeek(Value).ToSolarDayOfWeek().ToDisplay(),
                MonthName = pc.GetMonth(Value).ToSolarMonthName().ToDisplay(),
                Year = pc.GetYear(Value).ToString().En2Fa(),
                Month = pc.GetMonth(Value).ToString().En2Fa(),
                Day = pc.GetDayOfMonth(Value).ToString().En2Fa(),
                Hour = pc.GetHour(Value).ToString().En2Fa(),
                Minutes = pc.GetMinute(Value).ToString().En2Fa(),
                Second = pc.GetSecond(Value).ToString().En2Fa(),
                Millisecond = pc.GetMilliseconds(Value).ToString().En2Fa(),
                TimeInterval = Value.GetTimeIntervalWithNow(customDateFormat)
            };
        }
        public static CustomDateTimeFormat ToCustomGregorianDateAndExactTime(this DateTime Value, CustomDateFormat customDateFormat)
        {
            return new CustomDateTimeFormat
            {
                DayOfWeak = Value.DayOfWeek.ToString(),
                Year = Value.Year.ToString(),
                Month = Value.Month.ToString(),
                Day = Value.Day.ToString(),
                Hour = Value.Hour.ToString(),
                Minutes = Value.Minute.ToString(),
                Second = Value.Second.ToString(),
                Millisecond = Value.Millisecond.ToString(),
                TimeInterval = Value.GetTimeIntervalWithNow(customDateFormat)
            };
        }
        public static CustomDateTimeFormat ToCustomLunarDateAndExactTime(this DateTime Value, CustomDateFormat customDateFormat)
        {
            HijriCalendar pc = new();
            return new CustomDateTimeFormat
            {
                DayOfWeak = pc.GetDayOfWeek(Value).ToDisplay(),
                Year = pc.GetYear(Value).ToString(),
                Month = pc.GetMonth(Value).ToLunarMonthName().ToString(),
                Day = pc.GetDayOfMonth(Value).ToString(),
                Hour = pc.GetHour(Value).ToString(),
                Minutes = pc.GetMinute(Value).ToString(),
                Second = pc.GetSecond(Value).ToString(),
                Millisecond = pc.GetMilliseconds(Value).ToString(),
                TimeInterval = Value.GetTimeIntervalWithNow(customDateFormat)
            };
        }
        public static string ToEpochTime(this DateTime dateTime)
        {
            TimeSpan t = dateTime - new DateTime(1970, 1, 1);
            long secondsSinceEpoch = (long)t.TotalSeconds;
            return secondsSinceEpoch.ToString();
        }
        public static DateTime EpochToDateTime(this string epochTime)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(double.Parse(epochTime)).ToLocalTime();
            return dtDateTime;
        }
        public static CustomDateTimeFormat EpochToCustomDateTimeFormat(this string epochTime)
        {
            DateTime dtDateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(double.Parse(epochTime)).ToLocalTime();
            return dtDateTime.ConvertToCustomDate(CustomDateFormat.ToSolarDate);
        }
        public static CustomDateTimeFormat ConvertToCustomDate(this DateTime dateTime, CustomDateFormat customDateFormat)
        {
            if (dateTime == default) return new CustomDateTimeFormat();

            return customDateFormat switch
            {
                CustomDateFormat.ToSolarDate => dateTime.ToCustomShamsiDateAndExactTime(customDateFormat),
                CustomDateFormat.ToGregorianDate => dateTime.ToCustomGregorianDateAndExactTime(customDateFormat),
                CustomDateFormat.ToLunarDate => dateTime.ToCustomLunarDateAndExactTime(customDateFormat),
                CustomDateFormat.ToEpochTime => new CustomDateTimeFormat { EpochTime = dateTime.ToEpochTime() },
                CustomDateFormat.ToUTCTime => new CustomDateTimeFormat { UTCTime = dateTime.ToUniversalTime().ToString() },
                _ => new CustomDateTimeFormat { },
            };
        }
        public static CustomDateTimeFormat? ConvertTSoCustomDate(this DateTimeOffset? dateTime, CustomDateFormat customDateFormat)
        {
            return customDateFormat switch
            {
                CustomDateFormat.ToSolarDate => (CustomDateTimeFormat?)dateTime.Value.DateTime.ToCustomShamsiDateAndExactTime(customDateFormat),
                CustomDateFormat.ToGregorianDate => (CustomDateTimeFormat?)dateTime.Value.DateTime.ToCustomGregorianDateAndExactTime(customDateFormat),
                CustomDateFormat.ToLunarDate => (CustomDateTimeFormat?)dateTime.Value.DateTime.ToCustomLunarDateAndExactTime(customDateFormat),
                CustomDateFormat.ToEpochTime => (CustomDateTimeFormat?)new CustomDateTimeFormat { EpochTime = dateTime.Value.DateTime.ToEpochTime() },
                CustomDateFormat.ToUTCTime => (CustomDateTimeFormat?)new CustomDateTimeFormat { UTCTime = dateTime.Value.ToUniversalTime().ToString() },
                _ => (CustomDateTimeFormat?)new CustomDateTimeFormat { },
            };
        }
        private static string GetTimeIntervalWithNow(this DateTime dateTime, CustomDateFormat customDateFormat)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;
            TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - dateTime.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);
            double deltaOriginal = ts.TotalSeconds;
            switch (customDateFormat)
            {
                case CustomDateFormat.ToSolarDate:
                    {
                        if (delta < 1 * MINUTE)
                        {
                            if (deltaOriginal > 0)
                                return ts.Seconds == 1 ? "لحظه ای قبل" : ts.Seconds.ConvertToPersionString().Trim() + " ثانیه قبل";
                            else
                                return Math.Abs(ts.Seconds) == 1 ? "لحظاتی دیگر" : ts.Seconds.ConvertToPersionString().Trim() + " ثانیه قبل";
                        }
                        else if (delta < 2 * MINUTE)
                        {
                            if (deltaOriginal > 0)
                                return "یک دقیقه قبل";
                            else
                                return "یک دقیقه دیگر";
                        }
                        else if (delta < 45 * MINUTE)
                        {
                            if (deltaOriginal > 0)
                                return ts.Minutes.ConvertToPersionString().Trim() + " دقیقه قبل";
                            else
                                return Math.Abs(ts.Minutes).ConvertToPersionString().Trim() + " دقیقه دیگر";
                        }
                        else if (delta < 90 * MINUTE)
                        {
                            if (deltaOriginal > 0)
                                return "یک ساعت قبل";
                            else
                                return "یک ساعت دیگر";
                        }
                        else if (delta < 24 * HOUR)
                        {
                            if (deltaOriginal > 0)
                                return ts.Hours.ConvertToPersionString().Trim() + " ساعت قبل";
                            else
                                return Math.Abs(ts.Hours).ConvertToPersionString().Trim() + " ساعت دیگر";
                        }
                        else if (delta < 48 * HOUR)
                        {
                            if (deltaOriginal > 0)
                                return "دیروز";
                            else
                                return "فردا";
                        }
                        else if (delta < 30 * DAY)
                        {
                            if (deltaOriginal > 0)
                                return ts.Days.ConvertToPersionString().Trim() + " روز قبل";
                            else
                                return Math.Abs(ts.Days).ConvertToPersionString().Trim() + " روز دیگر";
                        }
                        else if (delta < 12 * MONTH)
                        {
                            if (deltaOriginal > 0)
                            {
                                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                                return months <= 1 ? "یک ماه قبل" : months.ConvertToPersionString().Trim() + " ماه قبل";
                            }
                            else
                            {
                                int months = Math.Abs(Convert.ToInt32(Math.Floor((double)ts.Days / 30)));
                                return months <= 1 ? "یک ماه دیگر" : months.ConvertToPersionString().Trim() + " ماه دیگر";
                            }
                        }
                        else
                        {
                            if (deltaOriginal > 0)
                            {
                                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                                return years <= 1 ? "یک سال قبل" : years.ConvertToPersionString() + " سال قبل";
                            }
                            else
                            {
                                int years = Math.Abs(Convert.ToInt32(Math.Floor((double)ts.Days / 365)));
                                return years <= 1 ? "یک سال دیگر" : years.ConvertToPersionString() + " سال دیگر";
                            }
                        }
                    }
                case CustomDateFormat.ToGregorianDate:
                    {
                        if (delta < 1 * MINUTE)
                        {
                            if (deltaOriginal > 0)
                                return ts.Seconds <= 1 ? "a moment ago" : ts.Seconds + " Seconds ago";
                            else
                                return Math.Abs(ts.Seconds) <= 1 ? "one more moment" : Math.Abs(ts.Seconds) + " more seconds";
                        }
                        else if (delta < 2 * MINUTE)
                        {
                            if (deltaOriginal > 0)
                                return "a minute ago";
                            else
                                return "a few more minutes";
                        }
                        else if (delta < 45 * MINUTE)
                        {
                            if (deltaOriginal > 0)
                                return ts.Minutes + " minutes ago";
                            else
                                return Math.Abs(ts.Minutes) + " more minutes";
                        }
                        else if (delta < 90 * MINUTE)
                        {
                            if (deltaOriginal > 0)
                                return "an hour ago";
                            else
                                return "one more hour";
                        }
                        else if (delta < 24 * HOUR)
                        {
                            if (deltaOriginal > 0)
                                return ts.Hours + " hours ago";
                            else
                                return Math.Abs(ts.Hours) + " more hours";
                        }
                        else if (delta < 48 * HOUR)
                        {
                            if (deltaOriginal > 0)
                                return "yesterday";
                            else
                                return "tomorrow";
                        }
                        else if (delta < 30 * DAY)
                        {
                            if (deltaOriginal > 0)
                                return ts.Days + " days ago";
                            else
                                return Math.Abs(ts.Days) + " more days";
                        }
                        else if (delta < 12 * MONTH)
                        {
                            if (deltaOriginal > 0)
                            {
                                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                                return months <= 1 ? "a month ago" : months + " months ago";
                            }
                            else
                            {
                                int months = Math.Abs(Convert.ToInt32(Math.Floor((double)ts.Days / 30)));
                                return months <= 1 ? "one more month" : months + " more months";
                            }
                        }
                        else
                        {
                            if (deltaOriginal > 0)
                            {
                                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                                return years <= 1 ? "a year ago" : years + " years ago";
                            }
                            else
                            {
                                int years = Math.Abs(Convert.ToInt32(Math.Floor((double)ts.Days / 365)));
                                return years <= 1 ? "one more year" : years + "more years";
                            }
                        }
                    }
                case CustomDateFormat.ToLunarDate:
                    {
                        if (delta < 1 * MINUTE)
                        {
                            if (deltaOriginal > 0)
                                return ts.Seconds <= 1 ? "منذ لحظة" : ts.Seconds.ToString().En2Fa() + " منذ ثوانى";
                            else
                                return Math.Abs(ts.Seconds) <= 1 ? "لحظة أخرى" : Math.Abs(ts.Seconds).ToString().En2Fa() + " ثوانٍ أخرى";
                        }
                        else if (delta < 2 * MINUTE)
                        {
                            if (deltaOriginal > 0)
                                return "قبل دقيقة";
                            else
                                return "المزيد من دقيقة واحدة";
                        }
                        else if (delta < 45 * MINUTE)
                        {
                            if (deltaOriginal > 0)
                                return ts.Minutes.ToString().En2Fa() + " قبل دقيقة";
                            else
                                return Math.Abs(ts.Minutes).ToString().En2Fa() + " دقائق أخرى";
                        }
                        else if (delta < 90 * MINUTE)
                        {
                            if (deltaOriginal > 0)
                                return "قبل ساعة";
                            else
                                return "ساعات أخرى";
                        }
                        else if (delta < 24 * HOUR)
                        {
                            if (deltaOriginal > 0)
                                return ts.Hours.ToString().En2Fa() + " منذ ساعات";
                            else
                                return Math.Abs(ts.Hours).ToString().En2Fa() + " ساعات أخرى";
                        }
                        else if (delta < 48 * HOUR)
                        {
                            if (deltaOriginal > 0)
                                return "في الامس";
                            else
                                return "غدا";
                        }
                        else if (delta < 30 * DAY)
                        {
                            if (deltaOriginal > 0)
                                return ts.Days.ToString().En2Fa() + " أيام مضت";
                            else
                                return Math.Abs(ts.Days).ToString().En2Fa() + " أيام أخرى";
                        }
                        else if (delta < 12 * MONTH)
                        {
                            if (deltaOriginal > 0)
                            {
                                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                                return months <= 1 ? "قبل شهر" : months.ToString().En2Fa() + " منذ اشهر";
                            }
                            else
                            {
                                int months = Math.Abs(Convert.ToInt32(Math.Floor((double)ts.Days / 30)));
                                return months <= 1 ? "بعد شهر واحد" : months.ToString().En2Fa() + " شهور أخرى";
                            }
                        }
                        else
                        {
                            if (deltaOriginal > 0)
                            {
                                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                                return years <= 1 ? "قبل عام" : years.ToString().En2Fa() + " منذ سنوات";
                            }
                            else
                            {
                                int years = Math.Abs(Convert.ToInt32(Math.Floor((double)ts.Days / 365)));
                                return years <= 1 ? "سنة أخرى" : years.ToString().En2Fa() + " سنوات أخرى";
                            }
                        }

                    }
                default:
                    return null;
            }
        }
    }
    #region Struct And Events
    public struct CustomDateTimeFormat
    {
        public string DayOfWeak { get; set; }
        public string MonthName { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
        public string Hour { get; set; }
        public string Minutes { get; set; }
        public string Second { get; set; }
        public string Millisecond { get; set; }
        public string EpochTime { get; set; }
        public string UTCTime { get; set; }
        public string TimeInterval { get; set; }

        public bool HasValue()
        {

            if (!DayOfWeak.HasValue() ||
                 !MonthName.HasValue() ||
                 !Year.HasValue() ||
                 !Month.HasValue() ||
                 !Day.HasValue() ||
                 !Hour.HasValue() ||
                 !Minutes.HasValue() ||
                 !Second.HasValue() ||
                 !Millisecond.HasValue() ||
                 !EpochTime.HasValue() ||
                 !UTCTime.HasValue() ||
                 !TimeInterval.HasValue())
            {
                return false;
            }
            return true;
        }
    }
    public enum SolarDayOfWeek
    {
        //
        // Summary:
        //     Indicates Sunday.
        یکشنبه = 0,
        //
        // Summary:
        //     Indicates Monday.
        دوشنبه = 1,
        //
        // Summary:
        //     Indicates Tuesday.
        سهشنبه = 2,
        //
        // Summary:
        //     Indicates Wednesday.
        چهارشنبه = 3,
        //
        // Summary:
        //     Indicates Thursday.
        پنجشنبه = 4,
        //
        // Summary:
        //     Indicates Friday.
        جمعه = 5,
        //
        // Summary:
        //     Indicates Saturday.
        شنبه = 6
    }
    /*public enum LunarDayOfWeek
    {
        ‫الأحد = 0,
        ‫الاثنین = 1,
        ‫الثلاثاء = 2,
        ‫الأربعاء = 3,
        ‫الخمیس = 4,
        ‫الجمعه = 5,
        ‫السبت = 6
    }*/
    public enum SolarMonthName
    {
        فروردین = 1,
        اردیبهشت = 2,
        خرداد = 3,
        تیر = 4,
        مرداد = 5,
        شهریور = 6,
        مهر = 7,
        آبان = 8,
        آذر = 9,
        دی = 10,
        بهمن = 11,
        اسفند = 12
    }
    public enum LunarMonthName
    {
        محرم = 1,
        صفر = 2,
        ربیع‌الاوّل = 3,
        ربیع‌الثانی = 4,
        جُمادی‌الاَوَّل = 5,
        جُمادی‌الثّانی = 6,
        رَجَب = 7,
        شعبان = 8,
        رمضان = 9,
        شوال = 10,
        ذیقعده = 11,
        ذی‌الحِجّه = 12
    }
    public enum CustomDateFormat
    {
        /// <summary>
        /// Explain: تاریخ شمسی
        /// </summary>
        ToSolarDate,
        /// <summary>
        /// Explain: تاریخ میلادی
        /// </summary>
        ToGregorianDate,
        /// <summary>
        /// Explain: تاریخ قمری
        /// </summary>
        ToLunarDate,
        /// <summary>
        /// Explain: Epoch Time
        /// </summary>
        ToEpochTime,
        /// <summary>
        /// Explain: UTC time
        /// </summary>
        ToUTCTime
    }
    #endregion
}
