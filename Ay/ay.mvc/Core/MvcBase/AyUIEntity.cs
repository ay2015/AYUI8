using Ay.MvcFramework;
using System;

[Serializable]
public abstract class AyUIEntity : Model
{
    private bool selected = false;
    /// <summary>
    /// 是否选中
    /// </summary>
    public bool Selected
    {
        get { return selected; }
        set
        {
            if (selected != value)
            {
                selected = value;
                this.OnPropertyChanged("Selected");
            }

        }
    }

  

    private bool isMouseOver;

    public bool IsMouseOver
    {
        get { return isMouseOver; }
        set
        {
            if (isMouseOver != value)
            {
                isMouseOver = value;
                this.OnPropertyChanged("IsMouseOver");
            }

        }
    }

    private bool isExpanded;
    /// <summary>
    /// 2016-8-1 11:29:28 增加
    /// </summary>
    public bool IsExpanded
    {
        get { return isExpanded; }
        set
        {
            if (isExpanded != value)
            {
                isExpanded = value;
                this.OnPropertyChanged("IsExpanded");
            }

        }
    }


    private bool isEnabled;

    public bool IsEnabled
    {
        get { return isEnabled; }
        set
        {
            if (isEnabled != value)
            {
                isEnabled = value;
                this.OnPropertyChanged("IsEnabled");
            }
        }
    }

    private bool isChecked;

    public bool IsChecked
    {
        get { return isChecked; }
        set
        {
            if (isChecked != value)
            {
                isChecked = value;
                this.OnPropertyChanged("IsChecked");
            }
        }
    }

    private bool? isCheckedThree;

    public bool? IsCheckedThree
    {
        get { return isCheckedThree; }
        set
        {
            if (isCheckedThree != value)
            {
                isCheckedThree = value;
                this.OnPropertyChanged("IsCheckedThree");
            }
        }
    }
}

