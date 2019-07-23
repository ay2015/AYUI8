using System;


namespace ay.Controls.Args
{
    public class AyBoxListEventArgs : EventArgs
    {
        public AyBoxListEventArgs(object data,string text,string value)
        {
            Data = data;
            Text = text;
            Value = value;
        }
        public object Data { get; private set; }
        public string Text { get; set; }
        public string Value { get; set; }
    }
}
