using System.Collections;

public class AYUI
{
    private static Hashtable _session;
    private static readonly object sync = new object();
    /// <summary>
    /// 默认 键值对对象
    /// </summary>
    public static Hashtable Session
    {
        get
        {
            if (_session == null)
            {
                lock (sync)
                {
                    if (_session == null)
                    {
                        _session = new Hashtable();
                    }
                }
            }
            return _session;
        }
    }


    private static Hashtable _cache;
    private static readonly object syncCache = new object();
    /// <summary>
    /// 默认 键值对对象
    /// </summary>
    public static Hashtable Cache
    {
        get
        {
            if (_cache == null)
            {
                lock (syncCache)
                {
                    if (_cache == null)
                    {
                        _cache = new Hashtable();
                    }
                }
            }
            return _cache;
        }
    }

}

