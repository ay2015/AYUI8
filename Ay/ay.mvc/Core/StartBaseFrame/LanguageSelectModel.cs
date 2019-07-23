

namespace Ay.MvcFramework
{
  public class LanguageSelectModel : Model
    {
        private string _LanuageName;

        /// <summary>
        /// 语言 别名，用于皮肤设置中 下拉选项的文本
        /// </summary>
        public string LanuageName
        {
            get { return _LanuageName; }
            set { Set(ref _LanuageName, value); }
        }

        private string _ResourceName;

        /// <summary>
        /// 语言包    文件名，例如 zh-cn.xaml 就填写 zh-cn
        /// </summary>
        public string ResourceName
        {
            get { return _ResourceName; }
            set { Set(ref _ResourceName, value); }
        }
    }
}
