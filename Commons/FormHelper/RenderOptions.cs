using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bOS.Commons.FormHelper
{
    public enum Mode
    {
        READ,
        WRITE
    }

    public enum ControlPosition
    {
        ABSOLUTE,
        RELATIVE,
        FIXED
    }

    static class ControlPositionString
    {

        public static String GetString(this ControlPosition s1)
        {
            switch (s1)
            {
                case ControlPosition.ABSOLUTE:
                    return "absolute";
                case ControlPosition.FIXED:
                    return "fixed";
                case ControlPosition.RELATIVE:
                    return "relative";
                default:
                    return "relative";
            }
        }
    }


}