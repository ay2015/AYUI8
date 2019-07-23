using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;

namespace ay.Controls
{
    public class RandomExtension : MarkupExtension
    {
        readonly int _from, _to;

        public RandomExtension(int from, int to)
        {
            _from = from;
            _to = to;
        }

        public RandomExtension(int to) : this(0, to)
        {
        }

        static readonly Random _rdn = new Random();


        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return (double)_rdn.Next(_from, _to);
        }
    }
}
