using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ay.MvcFramework
{
    public class ExceptionContext
    {
        //private static object _lock = new object();
        //private static ExceptionContext instance;
        //public static ExceptionContext Instance()
        //{
        //    if (instance == null)
        //    {
        //        lock (_lock)
        //        {
        //            if (instance == null)
        //            {
        //                instance = new ExceptionContext();
        //            }
        //        }
        //    }
        //    return instance;
        //}

        private Exception exception;

        public Exception Exception
        {
            get { return exception; }
            set { exception = value; }
        }


        private string errorText;

        public string ErrorText
        {
            get { return errorText; }
            set
            {
                if (errorText != value)
                {
                    errorText = value;
                }
            }
        }



        private string caption = "用户反馈";
        /// <summary>
        /// 错误标题
        /// </summary>
        public string Caption
        {
            get { return caption; }
            set
            {
                if (caption != value)
                {
                    caption = value;
                }
            }
        }

        private string stackError;
        /// <summary>
        /// 堆栈信息
        /// </summary>
        public string StackError
        {
            get { return stackError; }
            set
            {
                if (stackError != value)
                {
                    stackError = value;
                }
            }
        }

    }
}
