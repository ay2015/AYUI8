namespace ay.contents
{
    public class DicItem 
    {
        private string _key = null;
        public string Key
        {
            get { return _key; }
            set
            {
                if (_key != value)
                {
                    _key = value;
                }
            }
        }
        private string _val = null;
        public string TargetValue
        {
            get { return _val; }
            set
            {
                if (_val != value)
                {
                    _val = value;
                }
            }
        }

        private string _sample = null;
        public string SampleValue
        {
            get { return _sample; }
            set
            {
                if (_sample != value)
                {
                    _sample = value;
                }
            }
        }


        private string _namePrex = null;
        /// <summary>
        /// 前缀，用于批量转换文件使用
        /// </summary>
        public string NamePrex
        {
            get { return _namePrex; }
            set
            {
                if (_namePrex != value)
                {
                    _namePrex = value;
                }
            }
        }


    }
}
