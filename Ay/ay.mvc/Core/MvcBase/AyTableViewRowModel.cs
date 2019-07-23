using ay.Controls;
public class AyTableViewRowModel : AyUIEntity
{
    public AyTableViewRowModel()
    {
        ROWID = IDHelper.GetGuidNoSplit;
    }
    /// <summary>
    /// 请勿设置
    /// </summary>
    private string _ROWID;

    /// <summary>
    /// 未填写
    /// </summary>
    public string ROWID
    {
        get { return _ROWID; }
        set { Set(ref _ROWID, value); }
    }



    private bool isEditRow = false;
    /// <summary>
    /// 是否行编辑
    /// </summary>
    public bool IsEditRow
    {
        get { return isEditRow; }
        set
        {
            isEditRow = value;
        }
    }

    private AyTableViewStatuss _RowStatus = AyTableViewStatuss.Normal;
    public AyTableViewStatuss RowStatus
    {
        get { return _RowStatus; }
        set
        {
            if (_RowStatus != value)
            {
                _RowStatus = value;
                this.OnPropertyChanged("RowStatus");
            }

        }
    }

    private bool _HasRowDetail;

    /// <summary>
    /// 未填写
    /// </summary>
    public bool HasRowDetail
    {
        get { return _HasRowDetail; }
        set { Set(ref _HasRowDetail, value); }
    }


}
