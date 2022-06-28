using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using HarmonyLib;
using SettlementCultureChanger.Behaviours;

namespace SettlementCultureChanger
{
    public class SettlementCultureChangerSubModule : MBSubModuleBase 
    {
        #region Properties

        public static CampaignGameStarter campaignGameStarter; 

        #endregion
        
        #region Game Events (inherited) 
        
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            try
            {
                new Harmony(typeof(SettlementCultureChangerSubModule).FullName).PatchAll();
            }
            catch { }
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);
            
            if (!(game.GameType is Campaign))
            {
                return;
            }
            
            campaignGameStarter = (CampaignGameStarter)gameStarterObject;
            AddCampaignBehaviours();
            
            if (PerSaveModSettings.Instance?.debugLogging == true)
            {
                InformationManager.DisplayMessage(new InformationMessage("SCC: Campaign Started"));
            }
        }
        
        #endregion

        #region Utilities

        private void AddCampaignBehaviours()
        {
            campaignGameStarter.AddBehavior(new SettlementCultureChangerBehaviour());
        }

        #endregion
    }
}