using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
// 添加命令空间
using System.Net;
using System.Windows;
using ay.FuncFactory;

namespace ay.FuncFactory
{
    public class AyFuncFtp : AyFuncBase
    {
        private static AyFuncFtp _Singleton = null;
        private static object _Lock = new object();
        internal static AyFuncFtp CreateInstance()
        {
            if (_Singleton == null) //双if +lock
            {
                lock (_Lock)
                {
                    if (_Singleton == null)
                    {
                        _Singleton = new AyFuncFtp();
                    }
                }
            }
            return _Singleton;
        }
        /// <summary>
        /// 对外操作实例
        /// </summary>
        public static AyFuncFtp Instance
        {
            get
            {
                return CreateInstance();
            }
        }
        public AyFuncFtp()
        {

        }
        public AyFuncFtp(string ip, string port)
        {
            this.ip = ip;
            this.ftpport = port;
            ftpUristring = "ftp://" + ip + ":" + ftpport;
        }
        public AyFuncFtp(string ip, string port, string user, string pwd)
        {
            this.ip = ip;
            this.ftpport = port;
            this.user = user;
            this.password = pwd;
            ftpUristring = "ftp://" + ip + ":" + ftpport;
            networkCredential = new NetworkCredential(this.user, this.password);
        }
        public AyFuncFtp(string ip, string port, string user, string pwd, string para1, bool tr)
        {
            this.ip = ip;
            this.ftpport = port;
            this.user = user + para1;
            if (tr)
            {
                this.password = pwd+para1;
            }
            else
            {
                this.password = pwd;
            }

            ftpUristring = "ftp://" + ip + ":" + ftpport;
            networkCredential = new NetworkCredential(this.user, this.password);
        }
        public AyFuncFtp(string ip, string port, string user, string pwd, string para1)
        {
            this.ip = ip;
            this.ftpport = port;
            this.user = user + para1;
            this.password = pwd;
            ftpUristring = "ftp://" + ip + ":" + ftpport;
            networkCredential = new NetworkCredential(this.user, this.password);
        }
        public AyFuncFtp(string ip, string port, string user, string pwd, string para1, string para2)
        {
            this.ip = ip;
            this.ftpport = port;
            this.user = user + para1;
            this.password = pwd + para2;
            ftpUristring = "ftp://" + ip + ":" + ftpport;
            networkCredential = new NetworkCredential(this.user, this.password);
        }

        public string ip = "127.0.0.1";
        public string ftpport = "21";
        internal string user = "";
        internal string password = "";
        public string ftpUristring = null;
        public NetworkCredential networkCredential;

        public void SetAccountInfo()
        {
            networkCredential = new NetworkCredential(user, password);
        }
        //public bool InitCreateDir(string dirPath)
        //{
        //    FtpWebRequest request = CreateFtpWebRequest(dirPath, WebRequestMethods.Ftp.MakeDirectory);
        //    FtpWebResponse response = GetFtpResponse(request);
        //    if (response == null)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //    //if (ShowFtpFileAndDirectory())
        //    //{
        //    //    return true;
        //    //}
        //    //else
        //    //{
        //    //    return false;
        //    //}
        //}


        //public bool CheckFileExist(string ftpFilePath)
        //{
        //    FtpWebRequest ftpWebRequest = null;
        //    WebResponse webResponse = null;
        //    StreamReader reader = null;

        //    try
        //    {
        //        int s = ftpFilePath.LastIndexOf('/');
        //        if (s == ftpFilePath.Length - 1)
        //        {
        //            ftpFilePath = ftpFilePath.Substring(0, ftpFilePath.Length - 1);
        //            s = ftpFilePath.LastIndexOf('/');
        //        }

        //        string ftpFileName = ftpFilePath.Substring(s + 1, ftpFilePath.Length - s - 1);
        //        string uri = ftpUristring + ftpFilePath;

        //        ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
        //        ftpWebRequest.Credentials = networkCredential;
        //        ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectory;
        //        ftpWebRequest.UsePassive = false;
        //        ftpWebRequest.KeepAlive = false;
        //        webResponse = ftpWebRequest.GetResponse();
        //        reader = new StreamReader(webResponse.GetResponseStream());
        //        string line = reader.ReadLine();
        //        while (line != null)
        //        {
        //            if (line == ftpFileName)
        //            {
        //                return true;
        //            }
        //            line = reader.ReadLine();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //        {
        //            reader.Close();
        //        }
        //        if (webResponse != null)
        //        {
        //            webResponse.Close();
        //        }
        //    }
        //    return false;
        //}
        public string Download(string filePath, string localFile)
        {
            try
            {
                string fd = System.Web.HttpUtility.UrlEncode(System.IO.Path.GetFileName(localFile));

                FtpWebRequest request = CreateFtpWebRequest(ftpUristring + filePath + fd, WebRequestMethods.Ftp.DownloadFile);
                request.Timeout = 6000;
                FtpWebResponse response = GetFtpResponse(request);
                if (response == null)
                {
                    return "服务器未响应...";
                }
                else if (response.StatusDescription.Trim().Substring(0, 3) == "125")
                {
                    return "文件不存在";
                }
                Stream responseStream = response.GetResponseStream();
                FileStream filestream = File.Create(localFile);
                int buflength = 8196;
                byte[] buffer = new byte[buflength];
                int bytesRead = 1;

                while (bytesRead != 0)
                {
                    bytesRead = responseStream.Read(buffer, 0, buflength);
                    filestream.Write(buffer, 0, bytesRead);
                }

                responseStream.Close();
                filestream.Close();


                return "下载完成";
            }
            catch (WebException ex)
            {
                return "下载失败";
            }
        }
        public void Logout()
        {

        }
        public bool Upload(string filePath, string localFile)
        {
            //检查目录是否存在，不存在创建
            FtpCheckDirectoryExist(filePath);

            FileInfo fi = new FileInfo(localFile);
            FileStream fs = fi.OpenRead();
            long length = fs.Length;
            string fd = System.Web.HttpUtility.UrlEncode(fi.Name);
            FtpWebRequest req = CreateFtpWebRequest(ftpUristring + filePath + fd, WebRequestMethods.Ftp.UploadFile);
            req.ContentLength = length;
            double startbye = 0;
            int contentLen = 0;
            req.Timeout = 10 * 1000;
            try
            {
                Stream stream = req.GetRequestStream();
                int BufferLength = 8196;
                byte[] b = new byte[BufferLength];
                contentLen = fs.Read(b, 0, BufferLength);

                while (contentLen != 0)
                {
                    stream.Write(b, 0, contentLen);
                    contentLen = fs.Read(b, 0, BufferLength);
                    startbye += contentLen;

                }
                stream.Close();
                stream.Dispose();
                FtpWebResponse response = GetFtpResponse(req);
                if (response == null)
                {
                    return false;
                }
            }
            catch (Exception ee)
            {
                return false;
            }
            finally
            {
                fs.Close();
                req.Abort();
            }
            req.Abort();

            return true;
        }
        public void ErrLog(string ed)
        {
            MessageBox.Show(ed);
        }

        //判断文件的目录是否存,不存则创建
        public void FtpCheckDirectoryExist(string destFilePath)
        {
            string fullDir = FtpParseDirectory(destFilePath);
            string[] dirs = fullDir.Split('/');
            string curDir = "/";
            for (int i = 0; i < dirs.Length; i++)
            {
                string dir = dirs[i];
                //如果是以/开始的路径,第一个为空  
                if (dir != null && dir.Length > 0)
                {
                    try
                    {
                        curDir += dir + "/";
                        var _3 = "/" + curDir;
                        Console.WriteLine(_3);
                        FtpMakeDir(_3);
                    }
                    catch (Exception)
                    { }
                }
            }
        }

        public string FtpParseDirectory(string destFilePath)
        {
            return destFilePath.Substring(0, destFilePath.LastIndexOf("/"));
        }

        //创建目录
        public Boolean FtpMakeDir(string localFile)
        {
            FtpWebRequest req = CreateFtpWebRequest(ftpUristring + localFile, WebRequestMethods.Ftp.MakeDirectory);
            try
            {
                FtpWebResponse response = (FtpWebResponse)req.GetResponse();
                response.Close();
            }
            catch (Exception)
            {
                req.Abort();
                return false;
            }
            req.Abort();
            return true;
        }






        //public List<string> Files = new List<string>();

        //private bool ShowFtpFileAndDirectory()
        //{

        //    string uri = string.Empty;
        //    if (currentDir == "/")
        //    {
        //        uri = ftpUristring;
        //    }
        //    else
        //    {
        //        uri = ftpUristring + currentDir;
        //    }

        //    string[] urifield = uri.Split(' ');
        //    uri = urifield[0];
        //    FtpWebRequest request = CreateFtpWebRequest(uri, WebRequestMethods.Ftp.ListDirectoryDetails);

        //    // 获得服务器返回的响应信息
        //    FtpWebResponse response = GetFtpResponse(request);
        //    if (response == null)
        //    {
        //        return false;
        //    }
        //    else if (response.StatusDescription.Trim().Substring(0, 3) == "125")
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        //lstbxFtpState.Items.Add("连接成功，服务器返回的是：" + " " + response.StatusDescription);
        //        Files.Clear();
        //        // 读取网络流数据
        //        Stream stream = response.GetResponseStream();
        //        StreamReader streamReader = new StreamReader(stream, Encoding.Default);
        //        //lstbxFtpState.Items.Add("获取响应流....");
        //        string s = streamReader.ReadToEnd();

        //        streamReader.Close();
        //        stream.Close();
        //        response.Close();
        //        //lstbxFtpState.Items.Add("传输完成");

        //        // 处理并显示文件目录列表          
        //        string[] ftpdir = s.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        //        //lstbxFtpResources.Items.Add("↑返回上层目录");
        //        int length = 0;
        //        for (int i = 0; i < ftpdir.Length; i++)
        //        {
        //            if (ftpdir[i].EndsWith("."))
        //            {
        //                length = ftpdir[i].Length - 2;
        //                break;
        //            }
        //        }
        //        for (int i = 0; i < ftpdir.Length; i++)
        //        {
        //            s = ftpdir[i];
        //            int index = s.LastIndexOf('\t');
        //            if (index == -1)
        //            {
        //                if (length < s.Length)
        //                {
        //                    index = length;
        //                }
        //                else
        //                {
        //                    continue;
        //                }
        //            }

        //            string name = s.Substring(index + 1);
        //            if (name == "." || name == "..")
        //            {
        //                continue;
        //            }

        //            // 判断是否为目录，在名称前加"目录"来表示
        //            if (s[0] == 'd' || (s.ToLower()).Contains("<dir>"))
        //            {
        //                string[] namefield = name.Split(' ');
        //                int namefieldlength = namefield.Length;
        //                string dirname;
        //                dirname = namefield[namefieldlength - 1];

        //                // 对齐
        //                dirname = dirname.PadRight(34, ' ');
        //                name = dirname;
        //                // 显示目录
        //                Files.Add("[目录]" + name);
        //                //lstbxFtpResources.Items.Add("[目录]" + name);
        //            }
        //        }

        //        for (int i = 0; i < ftpdir.Length; i++)
        //        {
        //            s = ftpdir[i];
        //            int index = s.LastIndexOf('\t');
        //            if (index == -1)
        //            {
        //                if (length < s.Length)
        //                {
        //                    index = length;
        //                }
        //                else
        //                {
        //                    continue;
        //                }
        //            }

        //            string name = s.Substring(index + 1);
        //            if (name == "." || name == "..")
        //            {
        //                continue;
        //            }

        //            // 判断是否为文件
        //            if (!(s[0] == 'd' || (s.ToLower()).Contains("<dir>")))
        //            {
        //                string[] namefield = name.Split(' ');
        //                int namefieldlength = namefield.Length;
        //                string filename;
        //                filename = namefield[namefieldlength - 1];
        //                // 对齐
        //                filename = filename.PadRight(34, ' ');
        //                name = filename;
        //                // 显示文件
        //                Files.Add(name);
        //            }
        //        }
        //    }


        //    return true;
        //}



        #region 与服务器的交互

        // 创建FTP连接
        public FtpWebRequest CreateFtpWebRequest(string uri, string requestMethod)
        {
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(uri);
            request.Credentials = networkCredential;
            request.KeepAlive = true;
            request.UseBinary = true;
            request.UsePassive = false;
            request.Method = requestMethod;
            return request;
        }

        // 获取服务器返回的响应体
        public FtpWebResponse GetFtpResponse(FtpWebRequest request)
        {
            FtpWebResponse response = null;
            try
            {
                response = (FtpWebResponse)request.GetResponse();

                return response;
            }
            catch (WebException ex)
            {
                return null;
            }
        }
        #endregion 
    }
}
