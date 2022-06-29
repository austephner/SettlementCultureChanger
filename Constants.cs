namespace SettlementCultureChanger
{
    public static class Constants
    {
        #region Utility 
        
        public const string 
            DISPLAY_VERSION = "1.0.2",
            DEBUG_PREFIX = "SCC-Debug:",
            CULTURE_CONVERSION_DATA_KEY = "SCC:CultureConversionData";
        
        #endregion

        #region Calculations

        public const int 
            MIN_RANDOM_DAYS = 0,
            MAX_RANDOM_DAYS = 1000;

        #endregion

        #region Localization

        public const string
            UNKNOWN_STRING = "???",
            TIMED_AUTOMATIC_CONVERSION_MODE = "Convert After Random Amount of Days",
            IMMEDIATE_AUTOMATIC_CONVERSION_MODE = "Convert Immediately Upon Ownership Change",
            PLAYER_ONLY_MODE = "Player Only",
            PLAYER_CLAN_ONLY_MODE = "Player Clan Only",
            PLAYER_KINGDOM_ONLY_MODE = "Player Kingdom Only",
            EVERYONE_MODE = "Everyone";

        #endregion
    }
}