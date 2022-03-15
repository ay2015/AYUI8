using System.Collections.Generic;
using System.Collections.ObjectModel;

public class AyCommonResult<T> where T : class
{
    public AyCommonResult()
    {

    }
    public T Data { get; set; }

    public bool IsOK { get; set; }

    public string Message { get; set; }
}
public class AyCommonResult
{
    public AyCommonResult()
    {

    }
    public object Data { get; set; }

    public bool IsOK { get; set; }

    public string Message { get; set; }
}


public class AyPagingResult<T> where T:class
{
    public AyPagingResult()
    {

    }
    public List<T> Data { get; set; }

    public bool IsOK { get; set; }

    public int Total { get; set; }

    public string Message { get; set; }
}

    public class AyPagingDto<T> : AyPropertyChanged
{
    public AyPagingDto()
    {
        //Data = new ObservableCollection<T>();
    }

    private ObservableCollection<T> _data = new ObservableCollection<T>();
    public ObservableCollection<T> Data
    {
        get
        {
            return _data;
        }

        set
        {
            if (_data != value)
            {
                _data = value;
                OnPropertyChanged("Data");
            }

        }
    }

    public int _total;
    public int Total
    {
        get
        {
            return _total;
        }
        set
        {
            if (_total != value)
            {
                _total = value;
                OnPropertyChanged("Total");
            }
        }
    }
    private int _PageIndex = 1;

    /// <summary>
    /// 当前页
    /// </summary>
    public int PageIndex
    {
        get { return _PageIndex; }
        set { Set(ref _PageIndex, value); }
    }

    private int _PageSize = 15;

    /// <summary>
    /// 一页多少条
    /// </summary>
    public int PageSize
    {
        get { return _PageSize; }
        set { Set(ref _PageSize, value); }
    }
    private int _PageTotal;

    /// <summary>
    /// 总页数
    /// </summary>
    public int PageTotal
    {
        get { return _PageTotal; }
        set { Set(ref _PageTotal, value); }
    }

}
/// <summary>
/// 分页查询参数对象
/// </summary>
public class AyPagingInParameter
{
    private string _TableName;
    ///表名,多表是请使用 tA a inner join tB b On a.AID = b.AID
    public string TableName
    {
        get { return _TableName; }
        set { _TableName = value; }
    }
    private string _Fileds = "*";
    /// <summary>
    /// 字段
    /// </summary>
    public string Fields
    {
        get { return _Fileds; }
        set { _Fileds = value; }
    }
    private string _PrimaryKey = "ID";
    public string PrimaryKey
    {
        get { return _PrimaryKey; }
        set { _PrimaryKey = value; }
    }
    private int _PageSize = 10;
    public int PageSize
    {
        get { return _PageSize; }
        set { _PageSize = value; }
    }
    private int _CurrentPage = 1;
    public int CurrentPage
    {
        get { return _CurrentPage; }
        set { _CurrentPage = value; }
    }
    private string _Sort = string.Empty;
    /// <summary>
    /// 排序条件
    /// </summary>
    public string Sort
    {
        get { return _Sort; }
        set { _Sort = value; }
    }
    private string _Condition = string.Empty;
    /// <summary>
    /// 条件
    /// </summary>
    public string Condition
    {
        get { return _Condition; }
        set { _Condition = value; }
    }
    private int _RecordCount;
    /// <summary>
    /// 总记录数
    /// </summary>
    public int RecordCount
    {
        get { return _RecordCount; }
        set { _RecordCount = value; }
    }
}


