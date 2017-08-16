using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace bOS.Commons
{
    public class StringHelper
    {
        private static String SpecialCharacters = "\\/:*?\"<>|";

        public static Boolean ContainsSpecialCharacters(String value)
        {
            for (int i = 0; i < SpecialCharacters.Length; i++)
            {
                if (value.Contains(SpecialCharacters[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public static String ToCamelCase(CultureInfo culture, String value)
        {
            if (culture == null)
                return ToCamelCase("it-IT", value);

            return culture.TextInfo.ToTitleCase(value);
        }

        //culture: it-IT
        public static String ToCamelCase(String culture, String value)
        {
            TextInfo textInfo = new CultureInfo(culture, false).TextInfo;
            return textInfo.ToTitleCase(value);
        }

    }
}
