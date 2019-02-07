using System;
using System.Collections.Generic;
using System.Text;

namespace Samdoss.CommonLayer
{
    public class Common
    {
        public static string CheckNull(object obj)
        {
            if (obj != null)
                return obj.ToString();
            else
                return string.Empty;
        }

        public static int CheckIntNull(object obj)
        {
            if (!string.IsNullOrEmpty(obj.ToString()))
                return Convert.ToInt32(obj, null);
            else
                return 0;
        }
    }
}
