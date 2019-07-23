using System;
using System.Threading;
using System.Windows.Threading;


public class AyThread
{
    #region Singleton

    private AyThread()
    {
    }

    public static AyThread Instance
    {
        get
        {
            return Nested.instance;
        }
    }

    class Nested
    {
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Nested()
        {
        }

        internal static readonly AyThread instance = new AyThread();
    }

    #endregion

    #region New Thread

    static readonly object padlock = new object();

    /// <summary>
    /// ay 在2015年12月10日15:04:15增加
    /// 方便暂停线程
    /// </summary>
    /// <param name="millisecondsTimeout"></param>
    public void Sleep(int millisecondsTimeout)
    {
        System.Threading.Thread.Sleep(millisecondsTimeout);
    }
    public void RunNew(Action action)
    {
        lock (padlock)
        {
            action.BeginInvoke(ar => ActionCompleted(ar, res => action.EndInvoke(res)), null);
        }
    }

    public void RunNew<TResult>(Func<TResult> func, Action<TResult> callbackAction)
    {
        lock (padlock)
        {
            func.BeginInvoke(ar => FuncCompleted<TResult>(ar, res => func.EndInvoke(res), callbackAction), null);
        }
    }

    private static void ActionCompleted(IAsyncResult asyncResult, Action<IAsyncResult> endInvoke)
    {
        if (asyncResult.IsCompleted)
        {
            endInvoke(asyncResult);
        }
    }

    private static void FuncCompleted<TResult>(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endInvoke, Action<TResult> callbackAction)
    {
        if (asyncResult.IsCompleted)
        {
            TResult response = endInvoke(asyncResult);
            if (callbackAction != null)
            {
                callbackAction(response);
            }
        }
    }

    #endregion

    #region UI Thread

    private Dispatcher m_Dispatcher = null;

    //You have to Init the Dispatcher in the UI thread! - init once per application (if there is only one Dispatcher).
    public void InitDispatcher(Dispatcher dispatcher = null)
    {
        //m_Dispatcher = dispatcher == null ? (new UserControl()).Dispatcher : dispatcher;
        m_Dispatcher = dispatcher;
    }

    public void RunUI(Action action)
    {
        #region UI Thread Safety

        //handel by UI Thread.
        if (m_Dispatcher.Thread != Thread.CurrentThread)
        {
            m_Dispatcher.BeginInvoke(DispatcherPriority.Normal, action);
            return;
        }

        action();

        #endregion
    }

    public T RunUI<T>(Func<T> function)
    {
        #region UI Thread Safety

        //handel by UI Thread.
        if (m_Dispatcher.Thread != Thread.CurrentThread)
        {
            return (T)m_Dispatcher.Invoke(DispatcherPriority.Normal, function);
        }

        return function();

        #endregion
    }

    #endregion
}

