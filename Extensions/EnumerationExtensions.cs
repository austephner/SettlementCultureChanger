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

        public static string ToLocalizationKey(this ConversionMode conversionMode)
        {
            switch (conversionMode)
            {
                default:
                case ConversionMode.PlayerOnly: return Constants.PLAYER_ONLY_MODE;
                case ConversionMode.PlayerClanOnly: return Constants.PLAYER_CLAN_ONLY_MODE;
                case ConversionMode.PlayerKingdomOnly: return Constants.PLAYER_KINGDOM_ONLY_MODE;
                case ConversionMode.Everyone: return Constants.EVERYONE_MODE;
            }
        }
    }
}