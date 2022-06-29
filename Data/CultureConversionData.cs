using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.SaveSystem;

namespace SettlementCultureChanger.Data
{
    public class CultureConversionData
    {
        [SaveableField(1)]
        public int remainingDaysUntilConversion;

        public CultureConversionData(int remainingDaysUntilConversion)
        {
            this.remainingDaysUntilConversion = remainingDaysUntilConversion;
        }
    }

    public class CultureConversionDataTypeDefiner : CampaignBehaviorBase.SaveableCampaignBehaviorTypeDefiner
    {
        public CultureConversionDataTypeDefiner() : base(Constants.CULTURE_CONVERSION_DATA_TYPE_DEFINER_ID) { }
        
        protected override void DefineClassTypes()
        {
            AddClassDefinition(typeof(CultureConversionData), 1);
        }

        protected override void DefineContainerDefinitions()
        {
            ConstructContainerDefinition(typeof(Dictionary<Settlement, CultureConversionData>));
        }
    }
}