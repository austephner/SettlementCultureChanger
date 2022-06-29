using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;

namespace SettlementCultureChanger.Extensions
{
    public static class SettlementExtensions
    {
        public static bool HasClan(this Settlement settlement)
        {
            return settlement.OwnerClan != null;
        }

        public static bool HasKingdom(this Settlement settlement)
        {
            return settlement.HasClan() && settlement.OwnerClan.Kingdom != null;
        }
        
        public static bool HasOwner(this Settlement settlement)
        {
            return settlement.HasClan() && settlement.Owner != null;
        }

        public static bool IsPlayerSettlement(this Settlement settlement)
        {
            return settlement.HasOwner() && Hero.MainHero != null && Hero.MainHero == settlement.Owner; 
        }

        public static bool HeroMatchesOwner(this Settlement settlement, Hero owner)
        {
            return settlement.HasOwner() && settlement.Owner == owner;
        }

        public static bool CurrentCultureMatchesOwnerCulture(this Settlement settlement)
        {
            return settlement.HasOwner() && settlement.Culture == settlement.Owner.Culture;
        }
    }
}