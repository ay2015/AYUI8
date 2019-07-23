using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ay.contentcore
{
    public class DesignDevSupport
    {
        private string _ContentFile;

        /// <summary>
        /// Content目录，仅wpf设计时有效果
        /// </summary>
        public string ContentDirectory
        {
            get
            {
                return _ContentFile;
            }
            set { _ContentFile = value; }
        }
    }
   
}
