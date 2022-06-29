using SettlementCultureChanger.Data;

namespace SettlementCultureChanger.Extensions
{
    public static class AutomaticConversionModeExtensions
    {
        public static string ToLocalizationKey(this AutomaticConversionMode automaticConversionMode)
        {
            switch (automaticConversionMode)
            {
                case AutomaticConversionMode.Immediate: return Constants.IMMEDIATE_AUTOMATIC_CONVERSION_MODE;
                case AutomaticConversionMode.Timed: return Constants.TIMED_AUTOMATIC_CONVERSION_MODE;
                default: return Constants.UNKNOWN_STRING;
            }
        }
    }
}