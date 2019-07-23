
using RDS.Adapter.Design;
using RDS.IAdapter;
using RDS.Models.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDS.Adapter.Design
{
    public class CommonAdapter: ICommonAdapter
    {
        public CommonReturnDTO<List<dicts>> GetDicts()
        {
            CommonReturnDTO<List<dicts>> c = new CommonReturnDTO<List<dicts>>();
            c.Type = 7;
            try
            {
                c.Result = DictsTable.Data.ToList();
                c.IsSuccess = true;
            }
            catch (Exception ex)
            {
                c.IsSuccess = false;
                c.Error = ex.Message;
            }
            return c;
        }


    }
}
