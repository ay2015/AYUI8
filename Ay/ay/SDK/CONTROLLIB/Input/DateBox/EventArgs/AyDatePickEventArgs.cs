using ay.Controls.Info;
using System;

namespace ay.Controls.Args
{
    public class AyDatePickEventArgs : EventArgs
    {
        public AyDatePickEventArgs(DateTime? pickedDateTime)
        {
            this.PickedDateTime = pickedDateTime;
        }
        public DateTime? PickedDateTime { get; set; }

    }

    public class AyDateListItemClickEventArgs : EventArgs
    {
        public AyDateListItemClickEventArgs(AyDatePickerItem pickerItem)
        {
            this.PickerItem = pickerItem;
        }
        public AyDatePickerItem PickerItem { get; set; }
    }
}
