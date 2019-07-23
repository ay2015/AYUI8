using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using ay.FuncFactory.Base;

namespace ay.FuncFactory
{
    public class AyFuncICSharpCode:AyFuncBase
    {
        private static AyFuncICSharpCode _Singleton = null;
        private static object _Lock = new object();
        internal static AyFuncICSharpCode CreateInstance()
        {
            if (_Singleton == null) //双if +lock
            {
                lock (_Lock)
                {
                    if (_Singleton == null)
                    {
                        _Singleton = new AyFuncICSharpCode();
                    }
                }
            }
            return _Singleton;
        }
        /// <summary>
        /// 对外操作实例
        /// </summary>
        public static AyFuncICSharpCode Instance
        {
            get
            {
                return CreateInstance();
            }
        }

        #region 私有方法
        /// <summary>
        /// 递归压缩文件夹方法
        /// </summary>
        private bool ZipFileDirectory(string FolderToZip, ZipOutputStream s, string ParentFolderName)
        {
            bool res = true;
            string[] folders, filenames;
            ZipEntry entry = null;
            FileStream fs = null;
            Crc32 crc = new Crc32();
            try
            {
                entry = new ZipEntry(Path.Combine(ParentFolderName, Path.GetFileName(FolderToZip) + "/"));
                s.PutNextEntry(entry);
                s.Flush();
                filenames = Directory.GetFiles(FolderToZip);
                foreach (string file in filenames)
                {
                    fs = File.OpenRead(file);
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    entry = new ZipEntry(Path.Combine(ParentFolderName, Path.GetFileName(FolderToZip) + "/" + Path.GetFileName(file)));
                    entry.DateTime = DateTime.Now;
                    entry.Size = fs.Length;
                    fs.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    s.PutNextEntry(entry);
                    s.Write(buffer, 0, buffer.Length);
                }
            }
            catch
            {
                res = false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs = null;
                }
                if (entry != null)
                {
                    entry = null;
                }
                GC.Collect();
                GC.Collect(1);
            }
            folders = Directory.GetDirectories(FolderToZip);
            foreach (string folder in folders)
            {
                if (!ZipFileDirectory(folder, s, Path.Combine(ParentFolderName, Path.GetFileName(FolderToZip))))
                {
                    return false;
                }
            }
            return res;
        }

        /// <summary>
        /// 压缩目录
        /// </summary>
        /// <param name="FolderToZip">待压缩的文件夹，全路径格式</param>
        /// <param name="ZipedFile">压缩后的文件名，全路径格式</param>
        private bool ZipFileDirectory(string FolderToZip, string ZipedFile, int level, string pwd = null)
        {
            bool res;
            if (!Directory.Exists(FolderToZip))
            {
                return false;
            }
            ZipOutputStream s = new ZipOutputStream(File.Create(ZipedFile));
            if (!string.IsNullOrEmpty(pwd))
            {
                s.Password = pwd;
            }
            s.SetLevel(level);
            res = ZipFileDirectory(FolderToZip, s, "");
            s.Finish();
            s.Close();
            return res;
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="FileToZip">要进行压缩的文件名</param>
        /// <param name="ZipedFile">压缩后生成的压缩文件名</param>
        private bool ZipFile(string FileToZip, string ZipedFile, int level, string pwd = null)
        {
            if (!File.Exists(FileToZip))
            {
                throw new System.IO.FileNotFoundException("指定要压缩的文件: " + FileToZip + " 不存在!");
            }
            FileStream ZipFile = null;
            ZipOutputStream ZipStream = null;
            ZipEntry ZipEntry = null;
            bool res = true;
            try
            {
                ZipFile = File.OpenRead(FileToZip);
                byte[] buffer = new byte[ZipFile.Length];
                ZipFile.Read(buffer, 0, buffer.Length);
                ZipFile.Close();

                ZipFile = File.Create(ZipedFile);
                ZipStream = new ZipOutputStream(ZipFile);
                if (pwd != null)
                {
                    ZipStream.Password = pwd;
                }
                ZipEntry = new ZipEntry(Path.GetFileName(FileToZip));
                ZipStream.PutNextEntry(ZipEntry);
                ZipStream.SetLevel(level);

                ZipStream.Write(buffer, 0, buffer.Length);
            }
            catch
            {
                res = false;
            }
            finally
            {
                if (ZipEntry != null)
                {
                    ZipEntry = null;
                }
                if (ZipStream != null)
                {
                    ZipStream.Finish();
                    ZipStream.Close();
                }
                if (ZipFile != null)
                {
                    ZipFile.Close();
                    ZipFile = null;
                }
                GC.Collect();
                GC.Collect(1);
            }
            return res;
        }
        #endregion

        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="FileToZip">待压缩的文件目录</param>
        /// <param name="ZipedFile">生成的目标文件</param>
        /// <param name="level">6</param>
        public bool Zip(String FileToZip, String ZipedFile, int level, string pwd = null)
        {
            if (Directory.Exists(FileToZip))
            {
                return ZipFileDirectory(FileToZip, ZipedFile, level, pwd);
            }
            else if (File.Exists(FileToZip))
            {
                return ZipFile(FileToZip, ZipedFile, level, pwd);
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="FileToUpZip">待解压的文件</param>
        /// <param name="ZipedFolder">解压目标存放目录</param>
        public void UnZip(string FileToUpZip, string ZipedFolder, AyZipProgressReport report, string pwd = null)
        {
            if (report == null)
            {
                report = new AyZipProgressReport();
            }
            if (!File.Exists(FileToUpZip))
            {
                return;
            }
            if (!Directory.Exists(ZipedFolder))
            {
                Directory.CreateDirectory(ZipedFolder);
            }
            //FileInfo files=
            ZipInputStream s = null;
            ZipEntry theEntry = null;
            string fileName;



            FileStream streamWriter = null;
            try
            {
                var fs = File.OpenRead(FileToUpZip);
                report.CurrentSize = 0;
                report.TotalSize = fs.Length;
                s = new ZipInputStream(fs);

                //获得压缩包大小

                if (!string.IsNullOrEmpty(pwd))
                {
                    s.Password = pwd;
                }
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    if (theEntry.Name != String.Empty)
                    {
                        fileName = Path.Combine(ZipedFolder, theEntry.Name);
                        if (fileName.EndsWith("/") || fileName.EndsWith("\\"))
                        {
                            Directory.CreateDirectory(fileName);
                            continue;
                        }
                        streamWriter = File.Create(fileName);
                        //streamWriter = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                        int size = 2048;
                        byte[] data = new byte[2048];

                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            report.CurrentSize += size;
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                //fs.Close();
                //fs.Dispose();
            }
            finally
            {
                if (streamWriter != null)
                {
                    //streamWriter.Flush();
                    streamWriter.Close();
                    streamWriter = null;
                }
                if (theEntry != null)
                {
                    theEntry = null;
                }
                if (s != null)
                {
                    s.Close();
                    s = null;
                }

                GC.Collect();
                GC.Collect(1);
            }
        }

       
    }

}
