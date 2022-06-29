using TaleWorlds.CampaignSystem;

namespace SettlementCultureChanger.Utilities
{
    public static class HeroComparisonUtils
    {
        public static bool CheckSameClan(Hero a, Hero b)
        {
            return a.Clan != null && b.Clan != null && 
                   a.Clan == b.Clan;
        }

        public static bool CheckSameKingdom(Hero a, Hero b)
        {
            return a != null && b != null && 
                   a.Clan?.Kingdom != null && b.Clan?.Kingdom != null && 
                   a.Clan.Kingdom == b.Clan.Kingdom;
        }
    }
}