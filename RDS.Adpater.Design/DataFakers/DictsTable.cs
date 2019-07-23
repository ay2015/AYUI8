using Ay.Framework.DataCreaters;
using RDS.Models.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDS.Adapter.Design
{
    internal class DictsTable
    {
        static DictsTable()
        {
            Data.Add(new dicts { Id = "1", Field = "sex", Name = "男", Value = "1", SystemName = "rds" });
            Data.Add(new dicts { Id = "2", Field = "sex", Name = "女", Value = "2", SystemName = "rds" });
        }
        internal static List<dicts> Data = new List<dicts>();
    }
}
