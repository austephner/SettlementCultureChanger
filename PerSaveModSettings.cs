using System;
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Dropdown;
using MCM.Abstractions.Settings.Base.PerSave;
using SettlementCultureChanger.Behaviours;

namespace SettlementCultureChanger
{
    public class PerSaveModSettings : AttributePerSaveSettings<PerSaveModSettings>
    {
        public override string Id => "SettlementCultureChanger";
        
        public override string DisplayName => $"Settlement Culture Changer ({Constants.DISPLAY_VERSION})";

        #region GENERAL 
        
        [SettingPropertyGroup("General", GroupOrder = 1)]
        [SettingPropertyBool("Enabled", HintText = "When enabled, allows for this mod to control the conversion of a settlement's culture.", Order = 1, RequireRestart = false)]
        public bool enabled { get; set; } = true;
        
        [SettingPropertyGroup("General", GroupOrder = 1)]
        [SettingPropertyBool("Debug Logging", HintText = "When enabled, this mod will regularly post debug messages in various ways.", Order = 10, RequireRestart = false)]
        public bool debugLogging { get; set; } = true;

        [SettingPropertyGroup("General", GroupOrder = 1)]
        [SettingPropertyBool("Notify when settlements' culture changes for player", HintText = "When enabled, the a message will be logged stating a settlement owned by a player has converted their culture.", Order = 11, RequireRestart = false)]
        public bool notifyWhenPlayerSettlementsChangeCulture { get; set; } = true;
        
        [SettingPropertyGroup("General", GroupOrder = 1)]
        [SettingPropertyBool("Only convert player settlements", HintText = "When enabled, only player settlements will be converted.", Order = 12, RequireRestart = false)]
        public bool onlyConvertPlayerSettlements { get; set; } = true;
        
        #endregion

        #region Automatic Conversion

        [SettingPropertyGroup("Automatic Conversion", GroupOrder = 2)]
        [SettingPropertyBool("Enable Automatic Conversion", HintText = "When enabled, a settlement's culture will automatically convert to the \"Culture Source\" over time.", IsToggle = true, Order = 1, RequireRestart = false)]
        public bool enableAutomaticConversion { get; set; } = true;

        [SettingPropertyGroup("Automatic Conversion", GroupOrder = 2)]
        [SettingPropertyDropdown("Culture Source", HintText = "Determines where the target culture comes from.", Order = 2, RequireRestart = false)]
        public DropdownDefault<string> automaticConversionCultureSource { get; set; } = new DropdownDefault<string>(
            new string[]
            {
                "Prefer Governor, Fallback to Owner",
                "Governor Only",
                "Owner Only"
            },
            0);
        
        [SettingPropertyGroup("Automatic Conversion", GroupOrder = 2)]
        [SettingPropertyDropdown("Conversion Mode", HintText = "Determines how settlement cultures are converted.", Order = 3, RequireRestart = false)]
        public DropdownDefault<string> automaticConversionMode { get; set; } = new DropdownDefault<string>(
            new string[]
            {
                Constants.TIMED_AUTOMATIC_CONVERSION_MODE,
                Constants.IMMEDIATE_AUTOMATIC_CONVERSION_MODE
            },
            0);

        [SettingPropertyGroup("Automatic Conversion", GroupOrder = 2)]
        [SettingPropertyInteger("Min Random Days", Constants.MIN_RANDOM_DAYS, Constants.MAX_RANDOM_DAYS, HintText = "The minimum amount of random days for time based culture conversion.", Order = 4, RequireRestart = false)]
        public int minRandomDays { get; set; } = 10;

        [SettingPropertyGroup("Automatic Conversion", GroupOrder = 2)]
        [SettingPropertyInteger("Max Random Days", Constants.MIN_RANDOM_DAYS, Constants.MAX_RANDOM_DAYS, HintText = "The maximum amount of random days for time based culture conversion.", Order = 5, RequireRestart = false)]
        public int maxRandomDays { get; set; } = 50;

        #endregion

        #region Tools

        [SettingPropertyGroup("Tools & Cheats", GroupOrder = 3)]
        [SettingPropertyButton("Convert All Settlements to Owner Culture", HintText = "Immediately converts all settlements to their owner's culture.", RequireRestart = false, Content = "Set")]
        public Action convertAllSettlementsToOwnerCulture { get; set; } = SettlementCultureChangerBehaviour.ConvertAllSettlementsToOwnerCulture;
        
        [SettingPropertyGroup("Tools & Cheats", GroupOrder = 3)]
        [SettingPropertyButton("Convert All Player Settlements to Owner Culture", HintText = "Immediately converts all player's settlements to their owner's culture.", RequireRestart = false, Content = "Set")]
        public Action convertAllPlayerSettlementsToOwnerCulture { get; set; } = SettlementCultureChangerBehaviour.ConvertAllPlayerSettlementsToOwnerCulture;
        
        [SettingPropertyGroup("Tools & Cheats", GroupOrder = 3)]
        [SettingPropertyButton("Convert All Settlements in Player Clan to Owner Culture", HintText = "Immediately converts all settlements in the player's clan to their owner's culture.", RequireRestart = false, Content = "Set")]
        public Action convertAllPlayerClanSettlementsToOwnerCulture { get; set; } = SettlementCultureChangerBehaviour.ConvertAllSettlementsInPlayersClanToOwnerCulture;

        [SettingPropertyGroup("Tools & Cheats", GroupOrder = 3)]
        [SettingPropertyButton("Convert All Settlements in Player Kingdom to Owner Culture", HintText = "Immediately converts all settlements in the player's kingdom to their owner's culture.", RequireRestart = false, Content = "Set")]
        public Action convertAllPlayerKingdomSettlementsToOwnerCulture { get; set; } = SettlementCultureChangerBehaviour.ConvertAllSettlementsInPlayersKingdomToOwnerCulture;

        #endregion
    }
}