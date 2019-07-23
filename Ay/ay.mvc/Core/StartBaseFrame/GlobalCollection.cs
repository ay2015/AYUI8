using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ay.MvcFramework
{
    public class GlobalCollection
    {
        private static ClientResourceDictionaryCollection _ResourceDictionaryCollection;
        public static ClientResourceDictionaryCollection ResourceDictionaryCollection
        {
            get
            {
                if (_ResourceDictionaryCollection == null)
                {
                    _ResourceDictionaryCollection = new ClientResourceDictionaryCollection();
                }
                return _ResourceDictionaryCollection;
            }
        }


        private static ClientFontsCollection _Fonts;
        public static ClientFontsCollection Fonts
        {
            get
            {
                if (_Fonts == null)
                {
                    _Fonts = new ClientFontsCollection();
                }
                return _Fonts;
            }
        }

        private static ClientLanuagesCollection _Lanuages;
        public static ClientLanuagesCollection Lanuages
        {
            get
            {
                if (_Lanuages == null)
                {
                    _Lanuages = new ClientLanuagesCollection();
                }
                return _Lanuages;
            }
        }

        //private static GlobalFilterCollection _filters;
        //internal static GlobalFilterCollection Filters
        //{
        //    get
        //    {
        //        if (_filters == null)
        //        {
        //            _filters = new GlobalFilterCollection();
        //        }
        //        return _filters;
        //    }
        //}


    }
}
