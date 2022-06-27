using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SettlementCultureChanger
{
    public class SettlementCultureChangerSubModule : MBSubModuleBase 
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            
            Module.CurrentModule.AddInitialStateOption(new InitialStateOption("Message",
                new TextObject("Test", null),
                9990,
                () => { InformationManager.DisplayMessage(new InformationMessage("Test completed")); },
                () => (false, null)));
        }
    }
}