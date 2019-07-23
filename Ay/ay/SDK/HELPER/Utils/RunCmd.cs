using System;
using System.Diagnostics;

public class RunCmd
{

    //dosCommand Dos命令语句 
    public string _Execute(string dosCommand)
    {
        return _Execute(dosCommand, 0);
    }

    /// <summary>
    /// 执行DOS命令，返回DOS命令的输出 
    /// </summary>
    /// <param name="command">dos命令</param>
    /// <param name="seconds">等待命令执行的时间（单位：毫秒）， </param>
    /// 如果设定为0，则无限等待</param> 
    /// <returns>返回DOS命令的输出</returns>
    public static string _Execute(string command, int seconds)
    {
        string output = ""; //输出字符串 
        if (command != null && !command.Equals(""))
        {
            Process process = new Process();//创建进程对象 
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";//设定需要执行的命令 
            startInfo.Arguments = "/C " + command;//“/C”表示执行完命令后马上退出 
            startInfo.UseShellExecute = false;//不使用系统外壳程序启动 
            startInfo.RedirectStandardInput = false;//不重定向输入 
            startInfo.RedirectStandardOutput = true; //重定向输出 
            startInfo.CreateNoWindow = true;//不创建窗口 

            process.StartInfo = startInfo;
            try
            {
                if (process.Start())//开始进程 
                {
                    if (seconds == 0)
                    {
                        process.WaitForExit();//这里无限等待进程结束 
                    }
                    else
                    {
                        process.WaitForExit(seconds); //等待进程结束，等待时间为指定的毫秒 
                    }
                    output = process.StandardOutput.ReadToEnd();//读取进程的输出 
                }
            }
            catch
            {
                // log.Error("Execute:_Execute:" + ex.Message);

            }
            finally
            {
                if (process != null)
                    process.Close();
            }
        }
        return output;
    }
}
