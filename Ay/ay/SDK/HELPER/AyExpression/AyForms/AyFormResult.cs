using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ay.AyExpression
{
    /// <summary>
    /// 用户关心的结果
    /// </summary>
    public class AyFormResult
    {
        /// <summary>
        /// 验证示例
        /// </summary>
        private string example;

        public string Example
        {
            get { return example; }
            set { example = value; }
        }


        private string error = "格式不正确";
        /// <summary>
        /// 错误详细信息
        /// </summary>
        public string Error
        {
            get
            {
                return error;
            }

            set
            {
                error = value;
            }
        }

        private bool result=true;
        /// <summary>
        /// 验证结果
        /// </summary>
        public bool Result
        {
            get { return result; }
            set { result = value; }
        }

        /// <summary>
        /// 验证类型
        /// </summary>
        public Type ValidatorType { get; set; }

    }
}
