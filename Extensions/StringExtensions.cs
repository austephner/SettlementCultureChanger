using JetBrains.Annotations;
using SettlementCultureChanger.Data;

namespace SettlementCultureChanger.Extensions
{
    public static class StringExtensions
    {
        public static AutomaticConversionMode ToAutomaticConversionMode([CanBeNull] this string str)
        {
            if (str == null)
            {
                return AutomaticConversionMode.Timed;
            }
            
            switch (str)
            {
                default:
                case Constants.TIMED_AUTOMATIC_CONVERSION_MODE: return AutomaticConversionMode.Timed;
                case Constants.IMMEDIATE_AUTOMATIC_CONVERSION_MODE: return AutomaticConversionMode.Immediate;
            }
        }
    }
}