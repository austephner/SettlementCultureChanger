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
                new TextObject("Message", null),
                9990,
                () => { InformationManager.DisplayMessage(new InformationMessage("Hello World!")); },
                () => (false, null)));
        }
    }
}