
using ay;

public class AyGlobalConfig
{
    /// <summary>
    /// 是否开启动画，默认值true
    /// </summary>
    public static bool IsOpenAnimate = true;

    /// <summary>
    /// AYUI AyWindowShell窗体的背景图片Application.xml位置
    /// </summary>
    public static string AYUI_ConfigFileNamePath = null;

    public static string ReturnCurrentFolder()
    {
        return System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
    }
    public static string ReturnCurrentFolderCombinePath2(string path2)
    {
        return System.IO.Path.Combine(ReturnCurrentFolder(), path2);
    }
    public static IAyConfigManager _ACM;
    public static IAyConfigManager ACM
    {
        get
        {
            if (_ACM == null)
            {
                _ACM = ConfigManagerFactory.Create(ManagerFile.Config);
            }
            return _ACM;
        }
    }

}
