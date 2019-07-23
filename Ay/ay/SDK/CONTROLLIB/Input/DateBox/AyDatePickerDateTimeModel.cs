using System.Windows;

namespace ay.Enums
{
    /// <summary>
    /// 年月日时分秒 今天是否可见
    /// </summary>
    public class AyDatePickerDateTimeModel : AyPropertyChanged
    {
        private int year;

        public int Year
        {
            get { return year; }
            set
            {
                if (year != value)
                {
                    year = value;
                    OnPropertyChanged("Year");
                }
            }
        }

        private int month;

        public int Month
        {
            get { return month; }
            set
            {
                if (month != value)
                {
                    month = value;
                    OnPropertyChanged("Month");
                    OnPropertyChanged("GetMonth");
                }


            }
        }

        public string GetMonth
        {
            get
            {
                if (this.Month < 10)
                {
                    return "0" + this.Month.ToString();
                }
                else
                {
                    return this.Month.ToString();
                }
            }
        }


        private int day;

        public int Day
        {
            get { return day; }
            set
            {
                if (Day != value)
                {
                    day = value;
                    OnPropertyChanged("Day");
                }
            }
        }

        private int hour;

        public int Hour
        {
            get { return hour; }
            set
            {
                if (Hour != value)
                {
                    hour = value;
                    OnPropertyChanged("Hour");
                    OnPropertyChanged("GetHour");
                }
            }
        }

        public string GetHour
        {
            get
            {
                if (this.Hour < 10)
                {
                    return "0" + this.Hour.ToString();
                }
                else
                {
                    return this.Hour.ToString();
                }
            }
        }

        private int minute;

        public int Minute
        {
            get { return minute; }
            set
            {
                if (Minute != value)
                {
                    minute = value;
                    OnPropertyChanged("Minute");
                    OnPropertyChanged("GetMinute");
                }
            }
        }
        public string GetMinute
        {
            get
            {
                if (this.Minute < 10)
                {
                    return "0" + this.Minute.ToString();
                }
                else
                {
                    return this.Minute.ToString();
                }
            }
        }

        private int second;

        public int Second
        {
            get { return second; }
            set
            {
                if (Second != value)
                {
                    second = value;
                    OnPropertyChanged("Second");
                    OnPropertyChanged("GetSecond");
                }
            }
        }

        public string GetSecond
        {
            get
            {
                if (this.Second < 10)
                {
                    return "0" + this.Second.ToString();
                }
                else
                {
                    return this.Second.ToString();
                }
            }
        }


        private Visibility todayVisibility;

        public Visibility TodayVisibility
        {
            get { return todayVisibility; }
            set
            {

                if (todayVisibility != value)
                {
                    todayVisibility = value;
                    OnPropertyChanged("TodayVisibility");
                }

            }
        }

    }
}
