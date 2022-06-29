using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.SaveSystem;

namespace SettlementCultureChanger.Data
{
    public struct CultureConversionData
    {
        [SaveableField(1)]
        public Settlement settlement;
        
        [SaveableField(2)]
        public float conversionStartTime;
        
        [SaveableField(3)]
        public int remainingDaysUntilConversion;
    }
}