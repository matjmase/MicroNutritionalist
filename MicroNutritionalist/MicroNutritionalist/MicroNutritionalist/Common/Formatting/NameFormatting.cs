using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroNutritionalist.Common.Formatting
{
    public static class NameFormatting
    {
        public static string FormatName(string name)
        {
            return String.Join(" ", name.Trim().Split(' ').Where(e => e.Length > 0).Select(e =>
            {
                if (e.Length == 1)
                    return e.ToUpper();
                else
                    return e.Substring(0, 1).ToUpper() + e.Substring(1).ToLower();
            }));
        }
    }
}
