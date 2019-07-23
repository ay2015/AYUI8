using System.Collections.Generic;
using System;
using System.Linq;
using ay.FuncFactory;
using RDS.Models.UI;
using RDS.IAdapter;

public class AppBase
{
    #region 接口
    public ICommonAdapter CommonAdapter = null;
    public void SERVICEINIT()
    {
#if DEBUG
        CommonAdapter = new RDS.Adapter.CommonAdapter();//方便测试
#else
        CommonAdapter = DataFactory.Instance.GetService<ICommonAdapter>();
#endif

    }
    #endregion

    #region 单例
    private static AppBase _Singleton = null;
    private static object _Lock = new object();
    internal static AppBase CreateInstance()
    {
        if (_Singleton == null)
        {
            lock (_Lock)
            {
                if (_Singleton == null)
                {
                    _Singleton = new AppBase();
                }
            }
        }
        return _Singleton;
    }

    public static AppBase Instance
    {
        get
        {
            return CreateInstance();
        }
    }
    #endregion

    #region 字典服务
    public List<SelectListItemNoNotify> _dicPool = new List<SelectListItemNoNotify>();
    public void InitDic()
    {
        CommonReturnDTO<List<dicts>> _1 = CommonAdapter.GetDicts();
        foreach (var item in _1.Result)
        {
            SelectListItemNoNotify i = new SelectListItemNoNotify();
            i.ID = item.Id.ToString();
            i.Text = item.Name;
            i.Value = item.Value;
            i.field = item.Field;
            i.systemname = item.SystemName;
            i.ext = item.Ext;
            _dicPool.Add(i);
        }
    }
    #endregion

    #region 管理员和权限服务
    private const string ADMIN = "1";
    #endregion



    public void Initialize()
    {

        try
        {
            InitDic();
        }
        catch (System.Exception ex)
        {
            AyMessageBox.ShowError(ex.Message);
        }
    }

    private IList<SelectListItemNoNotify> _Sex;
    /// <summary>
    /// 性别 ，测试字典
    /// </summary>
    public IList<SelectListItemNoNotify> Sex
    {
        get
        {
            if (_Sex == null)
            {
                _Sex = _dicPool.Where(x => x.field == "sex").ToList();
            }
            return _Sex;
        }
    }



    #region 测试全局变量


    private string _PhotoFolder;
    public string PhotoFolder
    {
        get
        {
            if (_PhotoFolder == null)
            {
                _PhotoFolder = AyFuncIO.Instance.GetDirectory(AyGlobalConfig.ReturnCurrentFolderCombinePath2("headphoto"));
            }
            return _PhotoFolder;
        }
    }



    #endregion

}


