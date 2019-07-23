public class SelectListItem : AyUIEntity
{
    public string ID { get; set; }

    private string _value;

    public string Value
    {
        get { return _value; }
        set
        {
            _value = value;
            this.OnPropertyChanged("Value");
        }
    }

    private string _text;

    public string Text
    {
        get { return _text; }
        set
        {
            _text = value;
            this.OnPropertyChanged("Text");
        }
    }
    public string field { get; set; }

    public string systemname { get; set; }

    public string ext { get; set; }

}



