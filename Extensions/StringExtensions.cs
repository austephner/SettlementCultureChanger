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
        
        public static ConversionMode ToConversionMode([CanBeNull] this string str)
        {
            if (str == null)
            {
                return ConversionMode.PlayerOnly;
            }
            
            switch (str)
            {
                default:
                case Constants.PLAYER_ONLY_MODE: return ConversionMode.PlayerOnly;
                case Constants.PLAYER_CLAN_ONLY_MODE: return ConversionMode.PlayerClanOnly;
                case Constants.PLAYER_KINGDOM_ONLY_MODE: return ConversionMode.PlayerKingdomOnly;
                case Constants.EVERYONE_MODE: return ConversionMode.Everyone;
            }
        }
    }
}