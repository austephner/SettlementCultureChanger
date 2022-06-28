using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;

namespace SettlementCultureChanger.Extensions
{
    public static class SettlementExtensions
    {
        public static bool HasOwner(this Settlement settlement)
        {
            return settlement.OwnerClan != null && settlement.Owner != null;
        }

        public static bool MatchesOwner(this Settlement settlement, Hero owner)
        {
            return settlement.HasOwner() && settlement.Owner == owner;
        }
    }
}