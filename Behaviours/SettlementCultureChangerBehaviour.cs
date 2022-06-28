using System.Linq;
using SettlementCultureChanger.Extensions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace SettlementCultureChanger.Behaviours
{
    public class SettlementCultureChangerBehaviour : CampaignBehaviorBase
    {
        #region Inherited 
        
        public override void RegisterEvents()
        {
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, DailyTick);
        }

        public override void SyncData(IDataStore dataStore)
        {
            // todo: data synchronization
        }
        
        #endregion

        #region Events

        private void DailyTick()
        {
            if (PerSaveModSettings.Instance?.enabled == false)
            {
                return;
            }
            
            if (PerSaveModSettings.Instance?.debugLogging == true)
            {
                InformationManager.DisplayMessage(new InformationMessage("SCC: Doing daily culture conversion calculations."));
            }

            if (PerSaveModSettings.Instance?.enableAutomaticConversion == true)
            {
                foreach (var settlement in Settlement.All.Where(s => s.HasOwner()))
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

        public static void ConvertAllSettlementsToOwnerCulture()
        {
            foreach (var settlement in Settlement.All)
            {
                if (!settlement.HasOwner())
                {
                    continue;
                }
                
                if (settlement.Culture == null || settlement.Culture != settlement.Owner.Culture)
                {
                    settlement.Culture = settlement.Owner.Culture;
                    InformationManager.DisplayMessage(new InformationMessage($"SCC: {settlement.Name} culture changed to Owner ({settlement.Owner.Name}), {settlement.Culture.Name}"));
                }
            }
        }
        
        public static void ConvertAllPlayerSettlementsToOwnerCulture()
        {
            var hero = Hero.MainHero;

            if (hero == null)
            {
                InformationManager.DisplayMessage(new InformationMessage("SCC: There is currently no main hero."));
                return;
            }
            
            foreach (var settlement in Settlement.All)
            {
                if (!settlement.MatchesOwner(hero))
                {
                    continue;
                }
                
                if (settlement.Culture == null || settlement.Culture != hero.Culture)
                {
                    settlement.Culture = hero.Culture;
                    InformationManager.DisplayMessage(new InformationMessage($"SCC: {settlement.Name} culture changed to Owner ({hero.Name}), {settlement.Culture.Name}"));
                }
            }
        }

        #endregion
    }
}