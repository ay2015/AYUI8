
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// 当前登录用户
/// </summary>
public class LoginContext
{
    #region 单例
    private static volatile LoginContext _User;
    private static readonly object obj = new object();
    private LoginContext() { }
    public static LoginContext User
    {
        get
        {
            if (null == _User)
            {
                lock (obj)
                {
                    if (null == _User)
                    {
                        _User = new LoginContext();
                    }
                }

            }
            return _User;
        }
    }
    #endregion


    private DateTime _LastLoginTime;

    /// <summary>
    /// 最后登陆时间
    /// </summary>
    public DateTime LastLoginTime
    {
        get { return _LastLoginTime; }
        set { _LastLoginTime = value; }
    }


    private string _ID = "1";

    /// <summary>
    /// 账号Key
    /// </summary>
    public string ID
    {
        get { return _ID; }
        set { _ID = value; }
    }
    private string _Acc;

    /// <summary>
    /// 账号  
    /// </summary>
    public string Acc
    {
        get { return _Acc; }
        set { _Acc = value; }
    }


    private string _username = "admin";

    /// <summary>
    /// 昵称
    /// </summary>
    public string UserName
    {
        get { return _username; }
        set { _username = value; }
    }


}
