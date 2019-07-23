using RDS;

public class DataFactory
{
    #region 单例
    private static DataFactory _Singleton = null;
    private static object _Lock = new object();
    internal static DataFactory CreateInstance()
    {
        if (_Singleton == null) //双if +lock
        {
            lock (_Lock)
            {
                if (_Singleton == null)
                {
                    _Singleton = new DataFactory();
                }
            }
        }
        return _Singleton;
    }
    /// <summary>
    /// 对外操作实例
    /// </summary>
    public static DataFactory Instance
    {
        get
        {
            return CreateInstance();
        }
    }
    #endregion


    Castle.Windsor.IWindsorContainer DeveloperService = null;
    Castle.Windsor.IWindsorContainer DesignerService = null;

    public void Init()
    {
        var _d = AyGlobalConfig.ReturnCurrentFolderCombinePath2("Content/section.designer.windsor.config");
        if (System.IO.File.Exists(_d))
        {
            DesignerService = new Castle.Windsor.WindsorContainer(_d);
        }
        var _dev = AyGlobalConfig.ReturnCurrentFolderCombinePath2("Content/section.windsor.config");
        if (System.IO.File.Exists(_dev))
        {
            DeveloperService = new Castle.Windsor.WindsorContainer(_dev);
        }
    }

    public T GetService<T>()
    {
        if (System.Configuration.ConfigurationManager.AppSettings["isDevMode"] == "true")
        {
            return DeveloperService.Resolve<T>();
        }
        else
        {
            return DesignerService.Resolve<T>();
        }

    }
}

