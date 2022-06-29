using System;
using System.Collections.Generic;
using SettlementCultureChanger.Data;
using SettlementCultureChanger.Extensions;
using SettlementCultureChanger.Utilities;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.TwoDimension;

namespace SettlementCultureChanger.Behaviours
{
    // todo: convert hard coded strings to localization keys
    // todo: automatic conversion culture source defaults to owner, it needs to take governor into account
    public class SettlementCultureChangerBehaviour : CampaignBehaviorBase
    {
        #region Private

        private IDictionary<Settlement, CultureConversionData> _cultureConversionData = new Dictionary<Settlement, CultureConversionData>();

        private Random _random = new Random();
        
        #endregion

        #region Properties

        public static bool enabled => PerSaveModSettings.Instance?.enabled == true;

        public static bool debugLogging => PerSaveModSettings.Instance?.debugLogging == true;

        public static bool enableAutomaticConversion => PerSaveModSettings.Instance?.enableAutomaticConversion == true;

        public static AutomaticConversionMode automaticConversionMode => 
            PerSaveModSettings.Instance?.automaticConversionMode?.SelectedValue?.ToAutomaticConversionMode() 
            ?? AutomaticConversionMode.Timed;

        public static ConversionMode conversionMode =>
            PerSaveModSettings.Instance?.conversionMode?.SelectedValue?.ToConversionMode()
            ?? ConversionMode.PlayerOnly;

        public static ConversionMode conversionNotificationMode =>
            PerSaveModSettings.Instance?.conversionNotificationMode?.SelectedValue?.ToConversionMode()
            ?? ConversionMode.PlayerOnly;

        #endregion
        
        #region Inherited 
        
        public override void RegisterEvents()
        {
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, OnDailyTick);
            CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, OnSettlementOwnerChanged);
        }

        public override void SyncData(IDataStore dataStore)
        {
            if (_cultureConversionData == null)
            {
                RecreateCultureConversionDataDictionary();
            }

            dataStore.SyncData(Constants.CULTURE_CONVERSION_DATA_KEY, ref _cultureConversionData);
        }
        
        #endregion

        #region Events

        private void OnDailyTick()
        {
            if (!enabled)
            {
                return;
            }
            
            if (debugLogging)
            {
                InformationManager.DisplayMessage(new InformationMessage($"{Constants.DEBUG_PREFIX} Doing daily culture conversion calculations."));
            }

            if (enableAutomaticConversion && automaticConversionMode == AutomaticConversionMode.Timed)
            {
                foreach (var settlement in Settlement.All)
                {
                    DailySettlementConversionUpdate(settlement);
                }
            }
        }
        
        private void OnSettlementOwnerChanged(
            Settlement settlement, 
            bool openToClaim, 
            Hero newOwner, 
            Hero oldOwner, 
            Hero capturedBy, 
            ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail ownershipChangeDetails)
        {
            if (automaticConversionMode == AutomaticConversionMode.Immediate)
            {
                ConvertSettlementCultureToOwnerCulture(settlement);
            }
        }

        #endregion

        #region Utilities

        private int GetRandomInt(int min, int max)
        {
            lock (this)
            {
                return _random.Next(min, max);
            }
        }

        private float GetRandomFloat(float min, float max)
        {
            lock (this)
            {
                return Mathf.Lerp(min, max, _random.NextFloat());
            }
        }

        private void RecreateCultureConversionDataDictionary()
        {
            _cultureConversionData = new Dictionary<Settlement, CultureConversionData>();

            foreach (var settlement in Settlement.All)
            {
                if (!settlement.HasOwner())
                {
                    continue;
                }
                
                _cultureConversionData.Add(
                    settlement,
                    CreateCultureConversionData(settlement));
            }
        }

        private CultureConversionData CreateCultureConversionData(Settlement settlement)
        {
            var min = PerSaveModSettings.Instance?.minRandomDays ?? 0;
            var max = PerSaveModSettings.Instance?.maxRandomDays ?? 0;

            min = Mathf.Clamp(min, Constants.MIN_RANDOM_DAYS, Constants.MAX_RANDOM_DAYS);
            max = Mathf.Clamp(max, Constants.MIN_RANDOM_DAYS, Constants.MAX_RANDOM_DAYS);
            
            if (min > max) max = min;

            return new CultureConversionData()
            {
                settlement = settlement,
                conversionStartTime = Campaign.CurrentTime,
                remainingDaysUntilConversion = min == max ? 0 : GetRandomInt(min,max)
            }; 
        }

        private bool IsSettlementValidForDailyConversion(Settlement settlement)
        {
            if (!settlement.HasOwner() || settlement.CurrentCultureMatchesOwnerCulture())
            {
                return false; 
            }

            switch (conversionMode)
            {
                case ConversionMode.PlayerOnly: return settlement.IsPlayerSettlement();
                case ConversionMode.PlayerClanOnly: return settlement.ClanMatchesPlayerClan();
                case ConversionMode.PlayerKingdomOnly: return settlement.ClanMatchesPlayerKingdom();
            }

            return true;
        }

        private void DailySettlementConversionUpdate(Settlement settlement)
        {
            if (!IsSettlementValidForDailyConversion(settlement))
            {
                return;
            }
            
            if (!_cultureConversionData.ContainsKey(settlement))
            {
                _cultureConversionData.Add(settlement, CreateCultureConversionData(settlement));
                return;
            }

            var cultureConversionData = _cultureConversionData[settlement];

            cultureConversionData.remainingDaysUntilConversion--;

            if (cultureConversionData.remainingDaysUntilConversion < 0)
            {
                ConvertSettlementCultureToOwnerCulture(settlement);
                _cultureConversionData.Remove(settlement);
                return;
            }

            _cultureConversionData[settlement] = cultureConversionData;
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

                    if (debugLogging)
                    {
                        InformationManager.DisplayMessage(new InformationMessage($"{Constants.DEBUG_PREFIX} {settlement.Name} culture changed to {settlement.Owner.Name}'s culture, {settlement.Culture.Name}"));
                    }
                }
            }
        }
        
        public static void ConvertAllPlayerSettlementsToOwnerCulture()
        {
            var hero = Hero.MainHero;

            if (hero == null)
            {
                if (debugLogging)
                {
                    InformationManager.DisplayMessage(new InformationMessage($"{Constants.DEBUG_PREFIX} There is currently no main hero."));
                }

                return;
            }
            
            foreach (var settlement in Settlement.All)
            {
                if (!settlement.HeroMatchesOwner(hero))
                {
                    continue;
                }
                
                if (settlement.Culture == null || settlement.Culture != hero.Culture)
                {
                    settlement.Culture = hero.Culture;

                    if (debugLogging)
                    {
                        InformationManager.DisplayMessage(new InformationMessage($"{Constants.DEBUG_PREFIX} {settlement.Name} culture changed to {hero.Name}'s culture, {settlement.Culture.Name}"));
                    }
                }
            }
        }

        public static void ConvertAllSettlementsInPlayersClanToOwnerCulture()
        {
            var hero = Hero.MainHero;

            if (hero == null)
            {
                if (debugLogging)
                {
                    InformationManager.DisplayMessage(new InformationMessage($"{Constants.DEBUG_PREFIX} There is currently no main hero."));
                }

                return;
            }

            var clan = hero.Clan;
            
            foreach (var settlement in Settlement.All)
            {
                if (!settlement.HasOwner() || settlement.OwnerClan != clan)
                {
                    continue;
                }
                
                if (settlement.Culture == null || settlement.Culture != settlement.Owner.Culture)
                {
                    if (debugLogging)
                    {
                        InformationManager.DisplayMessage(new InformationMessage($"{Constants.DEBUG_PREFIX} {settlement.Name} culture changed to {settlement.Owner.Name}'s culture, {settlement.Culture.Name}"));
                    }
                    
                    settlement.Culture = settlement.Owner.Culture;
                }
            }
        }
        
        public static void ConvertAllSettlementsInPlayersKingdomToOwnerCulture()
        {
            var hero = Hero.MainHero;

            if (hero == null)
            {
                if (debugLogging)
                {
                    InformationManager.DisplayMessage(new InformationMessage($"{Constants.DEBUG_PREFIX} There is currently no main hero."));
                }
                
                return;
            }

            var clan = hero.Clan;

            if (clan == null)
            {
                if (debugLogging)
                {
                    InformationManager.DisplayMessage(new InformationMessage($"{Constants.DEBUG_PREFIX} The main hero is not in a clan."));
                }

                return;
            }

            var kingdom = clan.Kingdom;

            if (kingdom == null)
            {
                if (debugLogging)
                {
                    InformationManager.DisplayMessage(new InformationMessage($"{Constants.DEBUG_PREFIX} The main hero is not in a kingdom."));
                }

                return;
            }
            
            foreach (var settlement in Settlement.All)
            {
                if (!settlement.HasOwner() || !settlement.HasKingdom() || settlement.OwnerClan.Kingdom != kingdom)
                {
                    continue;
                }
                
                if (settlement.Culture == null || settlement.Culture != settlement.Owner.Culture)
                {
                    if (debugLogging)
                    {
                        InformationManager.DisplayMessage(new InformationMessage($"{Constants.DEBUG_PREFIX} {settlement.Name} culture changed to {settlement.Owner.Name}'s culture, {settlement.Culture.Name}"));
                    }
                    
                    settlement.Culture = settlement.Owner.Culture;
                }
            }
        }

        public static void ConvertSettlementCultureToOwnerCulture(Settlement settlement)
        {
            if (!settlement.HasOwner() || settlement.Owner.Culture == settlement.Culture)
            {
                return;
            }

            var showNotificationMessage = false;
            
            switch (conversionNotificationMode)
            {
                case ConversionMode.PlayerOnly:
                    showNotificationMessage = Hero.MainHero != null && Hero.MainHero == settlement.Owner;
                    break;
                case ConversionMode.PlayerClanOnly:
                    showNotificationMessage = Hero.MainHero != null && HeroComparisonUtils.CheckSameClan(Hero.MainHero, settlement.Owner);
                    break;
                case ConversionMode.PlayerKingdomOnly:
                    showNotificationMessage = Hero.MainHero != null && HeroComparisonUtils.CheckSameKingdom(Hero.MainHero, settlement.Owner);
                    break;
                case ConversionMode.Everyone:
                    showNotificationMessage = true;
                    break;
            }

            if (showNotificationMessage)
            {
                InformationManager.DisplayMessage(new InformationMessage($"{settlement.Name} has been cultured by the ways of their owner, {settlement.Owner.Name}"));
            }

            settlement.Culture = settlement.Owner.Culture;
        }

        #endregion
    }
}