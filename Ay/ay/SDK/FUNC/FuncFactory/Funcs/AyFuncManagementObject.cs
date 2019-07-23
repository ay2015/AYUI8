using System;
using System.Management;

namespace ay.FuncFactory
{
    public partial class AyFuncManagementObject:AyFuncBase
    {
        private static AyFuncManagementObject _Singleton = null;
        private static object _Lock = new object();
        internal static AyFuncManagementObject CreateInstance()
        {
            if (_Singleton == null) //双if +lock
            {
                lock (_Lock)
                {
                    if (_Singleton == null)
                    {
                        _Singleton = new AyFuncManagementObject();
                    }
                }
            }
            return _Singleton;
        }
        /// <summary>
        /// 对外操作实例
        /// </summary>
        public static AyFuncManagementObject Instance
        {
            get
            {
                return CreateInstance();
            }
        }
        //获取主板序列号
        public string GetBIOSSerialNumber()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_BIOS");
                string sBIOSSerialNumber = "";
                foreach (ManagementObject mo in searcher.Get())
                {
                    sBIOSSerialNumber = mo["SerialNumber"].ToString().Trim();
                }
                return sBIOSSerialNumber;
            }
            catch
            {
                return "";
            }
        }
        //获取CPU序列号
        public string GetCPUSerialNumber()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_Processor");
                string sCPUSerialNumber = "";
                foreach (ManagementObject mo in searcher.Get())
                {
                    sCPUSerialNumber = mo["ProcessorId"].ToString().Trim();
                }
                return sCPUSerialNumber;
            }
            catch
            {
                return "";
            }
        }
        //获取硬盘序列号
        public string GetHardDiskSerialNumber()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
                string sHardDiskSerialNumber = "";
                foreach (ManagementObject mo in searcher.Get())
                {
                    sHardDiskSerialNumber = mo["SerialNumber"].ToString().Trim();
                    break;
                }
                return sHardDiskSerialNumber;
            }
            catch
            {
                return "";
            }
        }
        //获取网卡地址
        public string GetNetCardMACAddress()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE ((MACAddress Is Not NULL) AND (Manufacturer <> 'Microsoft'))");
                string NetCardMACAddress = "";
                foreach (ManagementObject mo in searcher.Get())
                {
                    NetCardMACAddress = mo["MACAddress"].ToString().Trim();
                }
                return NetCardMACAddress;
            }
            catch
            {
                return "";
            }

        }

        /// <summary>
        /// 获得CPU编号
        /// </summary>
        /// <returns></returns>
        public string GetCPUID()
        {
            string cpuid = "";
            ManagementClass mc = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                cpuid = mo.Properties["ProcessorId"].Value.ToString();
            }
            return cpuid;
        }

        /// <summary>
        /// 获取硬盘序列号
        /// </summary>
        /// <returns></returns>
        public string GetDiskSerialNumber()
        {
            //这种模式在插入一个U盘后可能会有不同的结果，如插入我的手机时
            String HDid = "";
            ManagementClass mc = new ManagementClass("Win32_DiskDrive");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                HDid = (string)mo.Properties["Model"].Value;//SerialNumber
                break;//这名话解决有多个物理盘时产生的问题，只取第一个物理硬盘
            }
            return HDid;

            /*ManagementClass mc = new ManagementClass("Win32_PhysicalMedia");
            ManagementObjectCollection moc = mc.GetInstances();
            string str = "";
            foreach (ManagementObject mo in moc)
            {
                str = mo.Properties["SerialNumber"].Value.ToString();
                break;
            }
            return str;*/
        }

        /// <summary>
        /// 获取网卡硬件地址
        /// </summary>
        /// <returns></returns> 
        public string GetMacAddress()
        {
            string mac = "";
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                {
                    mac = mo["MacAddress"].ToString();
                    break;
                }
            }
            return mac;
        }

        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns></returns>
        public string GetIPAddress()
        {
            string st = "";
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                {
                    //st=mo["IpAddress"].ToString(); 
                    System.Array ar;
                    ar = (System.Array)(mo.Properties["IpAddress"].Value);
                    st = ar.GetValue(0).ToString();
                    break;
                }
            }
            return st;
        }

        /// <summary>
        /// 操作系统的登录用户名
        /// </summary>
        /// <returns></returns> 
        public string GetUserName()
        {
            return Environment.UserName;
        }


        /// <summary>
        /// 获取计算机名
        /// </summary>
        /// <returns></returns>
        public string GetComputerName()
        {
            return Environment.MachineName;
        }

        /// <summary>
        /// 操作系统类型
        /// </summary>
        /// <returns></returns> 
        public string GetSystemType()
        {
            string st = "";
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                st = mo["SystemType"].ToString();
            }
            return st;
        }

        /// <summary>
        /// 物理内存
        /// </summary>
        /// <returns></returns>
        public string GetPhysicalMemory()
        {
            string st = "";
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                st = mo["TotalPhysicalMemory"].ToString();
            }
            return st;
        }

        /// <summary>
        /// 显卡PNPDeviceID
        /// </summary>
        /// <returns></returns>
        public string GetVideoPNPID()
        {
            string st = "";
            ManagementObjectSearcher mos = new ManagementObjectSearcher("Select * from Win32_VideoController");
            foreach (ManagementObject mo in mos.Get())
            {
                st = mo["PNPDeviceID"].ToString();
            }
            return st;
        }

        /// <summary>
        /// 声卡PNPDeviceID
        /// </summary>
        /// <returns></returns>
        public string GetSoundPNPID()
        {
            string st = "";
            ManagementObjectSearcher mos = new ManagementObjectSearcher("Select * from Win32_SoundDevice");
            foreach (ManagementObject mo in mos.Get())
            {
                st = mo["PNPDeviceID"].ToString();
            }
            return st;
        }

        /// <summary>
        /// CPU版本信息
        /// </summary>
        /// <returns></returns>
        public string GetCPUVersion()
        {
            string st = "";
            ManagementObjectSearcher mos = new ManagementObjectSearcher("Select * from Win32_Processor");
            foreach (ManagementObject mo in mos.Get())
            {
                st = mo["Version"].ToString();
            }
            return st;
        }

        /// <summary>
        /// CPU名称信息
        /// </summary>
        /// <returns></returns>
        public string GetCPUName()
        {
            string st = "";
            ManagementObjectSearcher driveID = new ManagementObjectSearcher("Select * from Win32_Processor");
            foreach (ManagementObject mo in driveID.Get())
            {
                st = mo["Name"].ToString();
            }
            return st;
        }

        /// <summary>
        /// CPU制造厂商
        /// </summary>
        /// <returns></returns>
        public string GetCPUManufacturer()
        {
            string st = "";
            ManagementObjectSearcher mos = new ManagementObjectSearcher("Select * from Win32_Processor");
            foreach (ManagementObject mo in mos.Get())
            {
                st = mo["Manufacturer"].ToString();
            }
            return st;
        }

        /// <summary>
        /// 主板制造厂商
        /// </summary>
        /// <returns></returns>
        public string GetBoardManufacturer()
        {
            SelectQuery query = new SelectQuery("Select * from Win32_BaseBoard");
            ManagementObjectSearcher mos = new ManagementObjectSearcher(query);
            ManagementObjectCollection.ManagementObjectEnumerator data = mos.Get().GetEnumerator();
            data.MoveNext();
            ManagementBaseObject board = data.Current;
            return board.GetPropertyValue("Manufacturer").ToString();
        }

        /// <summary>
        /// 主板编号
        /// </summary>
        /// <returns></returns>
        public string GetBoardID()
        {
            string st = "";
            ManagementObjectSearcher mos = new ManagementObjectSearcher("Select * from Win32_BaseBoard");
            foreach (ManagementObject mo in mos.Get())
            {
                st = mo["SerialNumber"].ToString();
            }
            return st;
        }

        /// <summary>
        /// 主板型号
        /// </summary>
        /// <returns></returns>
        public string GetBoardType()
        {
            string st = "";
            ManagementObjectSearcher mos = new ManagementObjectSearcher("Select * from Win32_BaseBoard");
            foreach (ManagementObject mo in mos.Get())
            {
                st = mo["Product"].ToString();
            }
            return st;
        }
        //获得硬盘
        //SelectQuery selectQuery = new
        //SelectQuery("Win32_LogicalDisk");
        // ManagementObjectSearcher searcher =
        //     new ManagementObjectSearcher(selectQuery);

        // foreach (ManagementObject disk in searcher.Get())
        // {
        //     Console.WriteLine(disk.ToString());
        // }


        #region 放弃
        //List<string> dd = new List<string>();
        ////StringBuilder sb=new StringBuilder();
        //ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DesktopMonitor");
        //foreach (ManagementObject share in searcher.Get())
        //{
        //    StringBuilder sb = new StringBuilder();
        //    foreach (PropertyData pd in share.Properties)
        //    {
        //        sb.Append(pd.Name + " :: ");
        //        if (pd.Value != null)
        //        {
        //            sb.Append(pd.Value.ToString());
        //        }
        //        sb.AppendLine("");
        //    }
        //    dd.Add(sb.ToString());
        //    //break;

        //    //dd.Add(share["name"].ToString());
        //    //sb.Append(share["name"].ToString());
        //}
        //MessageBox.Show(string.Join(",",dd)); 
        #endregion
    }
}
