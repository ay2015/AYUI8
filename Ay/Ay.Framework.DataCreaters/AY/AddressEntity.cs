using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ay.Framework.DataCreaters
{
    /// <summary>
    /// 地址实体类
    /// </summary>
    internal class AddressData
    {
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 地区 县
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// 具体地址
        /// </summary>
        public string Address { get; set; }
    }

   
    internal class AddressJsonData
    {
        
        public string name { get; set; }
        
        public string address { get; set; }

        
        public string street_id { get; set; }

        
        public string telephone { get; set; }
        
        public int detail { get; set; }
        
        public string uid { get; set; }

        
        public Location location { get; set; }
    }

   
    internal class Location
    {
        
        public string lat { get; set; }

        
        public string lng { get; set; }
    }
   
    internal class ResultJsonData
    {
        
        public int status { get; set; }
        
        public string message { get; set; }
        
        public int total { get; set; }
        
        public List<AddressJsonData> results { get; set; }
    }
}
