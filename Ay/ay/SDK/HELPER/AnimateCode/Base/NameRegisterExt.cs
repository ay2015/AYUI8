/**----------------------------------------------- 
 *  ====================www.ayjs.net       杨洋    wpfui.com        ayui      ay  aaronyang======使用代码请注意侵权========= 
 * 
 * 作者：ay 
 * 联系QQ：875556003 
 * 时间2016-6-24 15:27:39 
 * 最后修改时间：2019-06-14 增加名字记录集合和取消注册，因为注册的名字会很多，如果n个，则外侧可能需要声明N个字符串去存储
 * -----------------------------------------*/

namespace ay.Animate
{
    public static class NameRegisterExt
    {
        /// <summary>
        /// 注册资源
        /// </summary>
        /// <param name="nameRegister">命名服务注册器</param>
        /// <param name="name">资源名</param>
        /// <param name="value">资源</param>
        public static void RegisterResourceByName(this NameRegister nameRegister,string name,object value)
        {
            if (!nameRegister.Resources.Contains(name))
            {
                nameRegister.Resources.Add(name, value);
            }
        }
        /// <summary>
        /// 注册资源
        /// </summary>
        /// <param name="nameRegister">命名服务注册器</param>
        /// <param name="value">资源</param>
        public static void RegisterResource(this NameRegister nameRegister, object value)
        {
            string _1 = nameRegister.namePrix + value.GetHashCode().ToString();
            if (!nameRegister.Resources.Contains(_1))
            {
                nameRegister.Resources.Add(_1, value);
            }
        }
        /// <summary>
        /// 取消注册资源
        /// </summary>
        /// <param name="nameRegister">命名服务注册器</param>
        /// <param name="name">资源名</param>
        public static void UnRegisterResource(this NameRegister nameRegister, object value)
        {
            string _1 = nameRegister.namePrix + value.GetHashCode().ToString();
            if (nameRegister.Resources.Contains(_1))
            {
                nameRegister.Resources.Remove(_1);
            }
        }

        /// <summary>
        /// 取消注册资源
        /// </summary>
        /// <param name="nameRegister">命名服务注册器</param>
        /// <param name="name">资源名</param>
        public static void UnRegisterResourceByName(this NameRegister nameRegister, string name)
        {
            if (nameRegister.Resources.Contains(name))
            {
                nameRegister.Resources.Remove(name);
            }
        }
    }

}
