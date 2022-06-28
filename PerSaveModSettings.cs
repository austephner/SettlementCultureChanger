﻿using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Dropdown;
using MCM.Abstractions.Settings.Base.PerSave;

namespace SettlementCultureChanger
{
    public class PerSaveModSettings : AttributePerSaveSettings<PerSaveModSettings>
    {
        public override string Id => "SettlementCultureChanger";
        
        public override string DisplayName => $"Settlement Culture Changer ({Constants.DISPLAY_VERSION})";

        #region GENERAL 
        
        [SettingPropertyGroup("General", GroupOrder = 1)]
        [SettingPropertyBool("Enabled", HintText = "When enabled, allows for this mod to control the conversion of a settlement's culture.", Order = 1)]
        public bool enabled { get; set; } = true;
        
        [SettingPropertyGroup("General", GroupOrder = 1)]
        [SettingPropertyBool("Debug Logging", HintText = "When enabled, this mod will regularly post debug messages in various ways.", Order = 10)]
        public bool debugLogging { get; set; } = true;

        #endregion

        #region Automatic Conversion

        [SettingPropertyGroup("Automatic Conversion", GroupOrder = 2)]
        [SettingPropertyBool("Enable Automatic Conversion", HintText = "When enabled, a settlement's culture will automatically convert to the \"Culture Source\" over time.", IsToggle = true, Order = 1)]
        public bool enableAutomaticConversion { get; set; } = true;

        [SettingPropertyGroup("Automatic Conversion", GroupOrder = 2)]
        [SettingPropertyDropdown("Culture Source", HintText = "Determines where the target culture comes from.", Order = 2)]
        public DropdownDefault<string> automaticConversionCultureSource { get; set; } = new DropdownDefault<string>(
            new string[]
            {
                "Try Use Governor, Else Use Owner",
                "Try Use Governor, Else Nothing",
                "Owner Only"
            },
            0);
        
        [SettingPropertyGroup("Automatic Conversion", GroupOrder = 2)]
        [SettingPropertyDropdown("Population Algorithm", HintText = "Determines what aspect of the population is used to calculate the cultural conversion.", Order = 3)]
        public DropdownDefault<string> automaticConversionPopulationMode { get; set; } = new DropdownDefault<string>(
            new string[]
            {
                "Random Conversion",
                "Constant Conversion"
            },
            0);

        [SettingPropertyGroup("Automatic Conversion", GroupOrder = 2)]
        [SettingPropertyInteger("Min Civilians Per Day", 1, 1000, Order = 4)]
        public int minCivilianConversionPerDay { get; set; } = 1;

        [SettingPropertyGroup("Automatic Conversion", GroupOrder = 2)]
        [SettingPropertyInteger("Max Civilians Per Day", 1, 1000, Order = 5)]
        public int maxCivilianConversionPerDay { get; set; } = 5;

        #endregion
    }
}