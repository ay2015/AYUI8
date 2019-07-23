using System;
using System.Collections.Generic;

namespace ay.Controls
{
  public class AyTableViewColumnEventArgs : EventArgs
  {
    public AyTableViewColumnEventArgs(AyTableViewColumn column)
    {
      Column = column;
    }
    public AyTableViewColumn Column { get; private set; }
  }

    public class AyTableViewColumnHeaderEventArgs : EventArgs
    {
        public AyTableViewColumnHeaderEventArgs(AyTableViewColumnHeader columnHeader)
        {
            ColumnHeader = columnHeader;
        }
        public AyTableViewColumnHeader ColumnHeader { get; private set; }
    }

    public class AyTableViewRowEventArgs : EventArgs
    {
        public AyTableViewRowEventArgs(object data)
        {
            Data = data;
        }
        /// <summary>
        /// 行对象
        /// </summary>
        public object Data { get; private set; }
    }
    public class AyTableViewRowsEventArgs : EventArgs
    {
        public AyTableViewRowsEventArgs(List<object> datas)
        {
            Datas = datas;
        }
        /// <summary>
        /// 行对象列表
        /// </summary>
        public List<object> Datas { get; private set; }
    }

    public class AyTableViewCellEventArgs : EventArgs
    {
        public AyTableViewCellEventArgs(object data,string field)
        {
            Data = data;
            Field = field;
        }
        /// <summary>
        /// 行对象
        /// </summary>
        public object Data { get; private set; }
        public string Field { get; private set; }

    }
}
