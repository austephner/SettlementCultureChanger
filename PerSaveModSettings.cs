using MCM.Abstractions.Attributes;
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
        
        [SettingPropertyGroup("General")]
        [SettingPropertyBool("Enabled")]
        public bool enabled { get; set; } = true;

        #endregion

        #region Automatic Conversion

        [SettingPropertyGroup("Automatic Conversion")]
        [SettingPropertyDropdown("Mode", HintText = "Determines how a settlement's culture is automatically converted over time.")]
        public DropdownDefault<string> automaticConversionMode { get; set; } = new DropdownDefault<string>(
            new string[]
            {
                "(Default) Use Governor's Culture, Fallback to Owner's Culture",
                "Governor's Culture Only",
                "Owner's Culture Only",
                "Disabled"
            },
            0);

        #endregion
    }
}