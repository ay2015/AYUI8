using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ay.MARKUP.ResponsiveSupport
{
    public class ResourceSetter : IDataSetter
    {
        /// <summary>
        /// 指定名字
        /// </summary>
        public string TargetName { get; set; }
        /// <summary>
        /// 资源范围
        /// </summary>
        public ResourceSetterScope Scope { get; set; } = ResourceSetterScope.FindInWindowResource;
        /// <summary>
        /// 资源值
        /// </summary>
        public object ResourceValue { get; set; }

        private Type _ResourceType;

        /// <summary>
        /// 资源类型
        /// </summary>
        public Type ResourceType
        {
            get { return _ResourceType; }
            set
            {
                _ResourceType = value;

            }
        }   
    }
}
