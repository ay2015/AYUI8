using ay.contents;
using System.Collections.Generic;

namespace ay.Date.Info
{
    internal class AyFirstOfWeekDictionary
    {
        private static List<int> _FirstDayDistanceDays;
        public static List<int> FirstDayDistanceDays
        {
            get
            {
                if (_FirstDayDistanceDays.IsNull())
                {
                    _FirstDayDistanceDays = new List<int>();
                    _FirstDayDistanceDays.Add(1);
                    _FirstDayDistanceDays.Add(0);
                    _FirstDayDistanceDays.Add(-1);
                    _FirstDayDistanceDays.Add(-2);
                    _FirstDayDistanceDays.Add(-3);
                    _FirstDayDistanceDays.Add(-4);
                    _FirstDayDistanceDays.Add(-5);

                }
                return _FirstDayDistanceDays;
            }
        }
        private static List<string> _FirstDayDistanceDaysText;
        public static List<string> FirstDayDistanceDaysText
        {
            get
            {
                if (_FirstDayDistanceDaysText.IsNull())
                {
                    _FirstDayDistanceDaysText = new List<string>();
                    _FirstDayDistanceDaysText.Add(Langs.ay_WeekName7.Lang());
                    _FirstDayDistanceDaysText.Add(Langs.ay_WeekName1.Lang());
                    _FirstDayDistanceDaysText.Add(Langs.ay_WeekName2.Lang());
                    _FirstDayDistanceDaysText.Add(Langs.ay_WeekName3.Lang());
                    _FirstDayDistanceDaysText.Add(Langs.ay_WeekName4.Lang());
                    _FirstDayDistanceDaysText.Add(Langs.ay_WeekName5.Lang());
                    _FirstDayDistanceDaysText.Add(Langs.ay_WeekName6.Lang());

                }
                return _FirstDayDistanceDaysText;
            }
        }

    }
}
