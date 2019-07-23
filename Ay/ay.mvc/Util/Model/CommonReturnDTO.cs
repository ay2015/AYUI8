using System;

/// <summary>
/// 公共返回结果类
/// </summary>
/// <typeparam name="T"></typeparam>
public class CommonReturnDTO<T>
{
    /// <summary>
    /// 1 添加 2删除 3修改 4保存 5操作 6查询 7获取信息
    /// </summary>
    public int Type { get; set; }
    /// <summary>
    /// 错误信息
    /// </summary>
    public string Error { get; set; }
    public CommonReturnDTO()
    {
        
    }
    private bool isSuccess = true;
    public bool IsSuccess
    {
        get { return isSuccess; }
        set { isSuccess = value; }
    }

    private string message;
    public string Message
    {
        get { return message; }
        set { message = value; }
    }

    public T Result { get; set; }

    /// <summary>
    /// 返回某条数据
    /// </summary>
    private object data;
    /// <summary>
    /// 返回某条数据，例如成功后的ID
    /// </summary>
    public object Data
    {
        get { return data; }
        set { data = value; }
    }

}

/// <summary>
/// 公共返回结果类
/// </summary> 
public class CommonReturnDTO
{
    public string Error { get; set; }
    /// <summary>
    /// 1 添加 2删除 3修改 4保存 5操作 6查询 7获取信息
    /// </summary>
    public int Type { get; set; }
    public CommonReturnDTO()
    {
   
    }
    private bool isSuccess = true;
    public bool IsSuccess
    {
        get { return isSuccess; }
        set { isSuccess = value; }
    }

    private string message;
    public string Message
    {
        get { return message; }
        set { message = value; }
    }

    /// <summary>
    /// 返回某条数据
    /// </summary>
    private object data;
    /// <summary>
    /// 返回某条数据，例如成功后的ID
    /// </summary>
    public object Data
    {
        get { return data; }
        set { data = value; }
    }
}


public static class ApiMsgHelper
{
    /// <summary>
    /// api返回接口
    /// </summary>
    /// <param name="flag">返回信息模版ID</param>
    /// <param name="status">状态码</param>
    /// <param name="msg"></param>
    /// <param name="lists"></param>
    /// <returns></returns>
    public static string ApiMessage(int flag, string status, string msg, string lists = "")
    {
        switch (flag)
        {
            case 1:
                return String.Format("{{\"status\":\"" + status + "\",\"message\":\"{0}\"}}", msg);
            case 2:
                return String.Format("{{\"status\":\"" + status + "\",\"message\":\"{0}\",\"content\":[{1}]}}", msg, lists);
            case 3:
                return String.Format("{{\"status\":\"" + status + "\",\"message\":\"{0}\",\"content\":\"{1}\"}}", msg, lists);
            default:
                return String.Format("{{\"status\":\"" + status + "\",\"message\":\"{0}\"}}", msg);
        }
    }
}


public static class CommonReturnDTOHelper
{
    public static CommonReturnDTO ToDeleteCommonReturnDTO(this int execResult)
    {
        if (execResult > 0)
        {
            return new CommonReturnDTO { IsSuccess = true, Type=2  };
        }
        else
        {
            return new CommonReturnDTO { IsSuccess = false, Type = 2 };
        }
    }
    public static CommonReturnDTO<T> ToDeleteCommonReturnDTO<T>(this int execResult, T returnResult)
    {
        if (execResult > 0)
        {
            return new CommonReturnDTO<T> { IsSuccess = true, Type = 2, Result = returnResult };
        }
        else
        {
            return new CommonReturnDTO<T> { IsSuccess = false, Type = 2, Result = returnResult };
        }
    }
    public static CommonReturnDTO ToAddCommonReturnDTO(this int execResult)
    {
        if (execResult > 0)
        {
            return new CommonReturnDTO { IsSuccess = true,  Type = 1};
        }
        else
        {
            return new CommonReturnDTO { IsSuccess = false,  Type = 1};
        }
    }
    public static CommonReturnDTO<T> ToAddCommonReturnDTO<T>(this int execResult, T returnResult)
    {
        if (execResult > 0)
        {
            return new CommonReturnDTO<T> { IsSuccess = true, Type = 1, Result = returnResult };
        }
        else
        {
            return new CommonReturnDTO<T> { IsSuccess = false, Type = 1, Result = returnResult };
        }
    }

    public static CommonReturnDTO ToEditCommonReturnDTO(this int execResult)
    {
        if (execResult > 0)
        {
            return new CommonReturnDTO { IsSuccess = true, Type = 3 };
        }
        else
        {
            return new CommonReturnDTO { IsSuccess = false, Type = 3 };
        }
    }
    public static CommonReturnDTO<T> ToEditCommonReturnDTO<T>(this int execResult, T returnResult)
    {
        if (execResult > 0)
        {
            return new CommonReturnDTO<T> { IsSuccess = true, Type = 3, Result = returnResult };
        }
        else
        {
            return new CommonReturnDTO<T> { IsSuccess = false, Type = 3, Result = returnResult };
        }
    }


    public static CommonReturnDTO ToDeleteCommonReturnDTO(this long execResult)
    {
        if (execResult > 0)
        {
            return new CommonReturnDTO { IsSuccess = true, Type = 2 };
        }
        else
        {
            return new CommonReturnDTO { IsSuccess = false, Type = 2 };
        }
    }
    public static CommonReturnDTO<T> ToDeleteCommonReturnDTO<T>(this long execResult, T returnResult)
    {
        if (execResult > 0)
        {
            return new CommonReturnDTO<T> { IsSuccess = true, Type = 2, Result = returnResult };
        }
        else
        {
            return new CommonReturnDTO<T> { IsSuccess = false,  Type = 2, Result = returnResult };
        }
    }
    public static CommonReturnDTO ToAddCommonReturnDTO(this long execResult)
    {
        if (execResult > 0)
        {
            return new CommonReturnDTO { IsSuccess = true,  Type = 1 };
        }
        else
        {
            return new CommonReturnDTO { IsSuccess = false, Type = 1 };
        }
    }
    public static CommonReturnDTO<T> ToAddCommonReturnDTO<T>(this long execResult, T returnResult)
    {
        if (execResult > 0)
        {
            return new CommonReturnDTO<T> { IsSuccess = true, Type = 1, Result = returnResult };
        }
        else
        {
            return new CommonReturnDTO<T> { IsSuccess = false, Type = 1, Result = returnResult };
        }
    }

    public static CommonReturnDTO ToEditCommonReturnDTO(this long execResult)
    {
        if (execResult > 0)
        {
            return new CommonReturnDTO { IsSuccess = true, Type = 3};
        }
        else
        {
            return new CommonReturnDTO { IsSuccess = false, Type = 3 };
        }
    }
    public static CommonReturnDTO<T> ToEditCommonReturnDTO<T>(this long execResult, T returnResult)
    {
        if (execResult > 0)
        {
            return new CommonReturnDTO<T> { IsSuccess = true, Type = 3, Result = returnResult };
        }
        else
        {
            return new CommonReturnDTO<T> { IsSuccess = false, Type = 3, Result = returnResult };
        }
    }
    public static CommonReturnDTO ToSaveCommonReturnDTO(this int execResult)
    {
        if (execResult > 0)
        {
            return new CommonReturnDTO { IsSuccess = true, Type = 4 };
        }
        else
        {
            return new CommonReturnDTO { IsSuccess = false, Type = 4 };
        }
    }
    public static CommonReturnDTO<T> ToSaveCommonReturnDTO<T>(this int execResult, T returnResult)
    {
        if (execResult > 0)
        {
            return new CommonReturnDTO<T> { IsSuccess = true, Type = 4, Result = returnResult };
        }
        else
        {
            return new CommonReturnDTO<T> { IsSuccess = false, Type = 4, Result = returnResult };
        }
    }
    public static CommonReturnDTO ToSaveCommonReturnDTO(this long execResult)
    {
        if (execResult > 0)
        {
            return new CommonReturnDTO { IsSuccess = true, Type = 4 };
        }
        else
        {
            return new CommonReturnDTO { IsSuccess = false, Type = 4 };
        }
    }
    public static CommonReturnDTO<T> ToSaveCommonReturnDTO<T>(this long execResult, T returnResult)
    {
        if (execResult > 0)
        {
            return new CommonReturnDTO<T> { IsSuccess = true, Type = 4, Result = returnResult };
        }
        else
        {
            return new CommonReturnDTO<T> { IsSuccess = false, Type = 4, Result = returnResult };
        }
    }
}



