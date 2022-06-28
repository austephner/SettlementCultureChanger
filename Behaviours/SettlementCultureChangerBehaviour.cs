using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace SettlementCultureChanger.Behaviours
{
    public class SettlementCultureChangerBehaviour : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, DailyTick);
        }

        public override void SyncData(IDataStore dataStore)
        {
            // todo: data synchronization
        }

        #region Events

        private void DailyTick()
        {
            if (PerSaveModSettings.Instance?.debugLogging == true)
            {
                InformationManager.DisplayMessage(new InformationMessage("SCC: Doing daily culture conversion"));
            }

            if (PerSaveModSettings.Instance?.enableAutomaticConversion == true)
            {
                foreach (var settlement in Settlement.All)
                {
                    DailySettlementConversionUpdate(settlement);
                }
            }
        }

        #endregion

        #region Utilities

        private void DailySettlementConversionUpdate(Settlement settlement)
        {
            
        }

        #endregion
    }
}