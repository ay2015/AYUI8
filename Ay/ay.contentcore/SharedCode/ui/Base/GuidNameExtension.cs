
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Media;

namespace ay.Controls
{
    public class GuidNameExtension : MarkupExtension
    {
        readonly string _qian;
        public GuidNameExtension(string qian)
        {
            this._qian = qian;
        }



        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Guid.NewGuid().ToGuidStringNoSplit(_qian);
        }
    }


}
