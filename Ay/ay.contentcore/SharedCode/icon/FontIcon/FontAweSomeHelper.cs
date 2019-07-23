using System;

namespace ay.contentcore.Mgr
{
    public class FontAweSomeHelper
    {
        public static string GetUnicode(string key)
        {
            AyFontAweSomeEnum d1;
            if (Enum.TryParse<AyFontAweSomeEnum>(key, true, out d1))
            {
                return d1.GetDescription();
            }
            else
            {
                return "";
            }
        }
    }



}
