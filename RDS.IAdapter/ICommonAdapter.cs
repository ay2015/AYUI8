using RDS.Models.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDS.IAdapter
{
    public interface ICommonAdapter
    {
        CommonReturnDTO<List<dicts>> GetDicts();

    }
}
