using System;
using System.Collections.Generic;
using System.Linq;
using SettlementCultureChanger.Data;
using SettlementCultureChanger.Extensions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.TwoDimension;

namespace SettlementCultureChanger.Behaviours
{
    public class SettlementCultureChangerBehaviour : CampaignBehaviorBase
    {
        #region Private

        private IDictionary<Settlement, CultureConversionData> _cultureConversionData = new Dictionary<Settlement, CultureConversionData>();

        private Random _random = new Random();
        
        #endregion

        #region Properties

        public static bool debugLogging => PerSaveModSettings.Instance?.debugLogging ?? false;

        #endregion
        
        #region Inherited 
        
        public override void RegisterEvents()
        {
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, DailyTick);
        }

        public override void SyncData(IDataStore dataStore)
        {
            if (_cultureConversionData == null)
            {
                RecreateCultureConversionDataDictionary();
            }
            
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
            
            if (debugLogging)
            {
                InformationManager.DisplayMessage(new InformationMessage($"{Constants.DEBUG_PREFIX} Doing daily culture conversion calculations."));
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
            if (!settlement.HasOwner() || settlement.CurrentCultureMatchesOwnerCulture() ||
                (PerSaveModSettings.Instance?.onlyConvertPlayerSettlements == true && !settlement.IsPlayerSettlement()))
            {
                return false;
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

            if (PerSaveModSettings.Instance?.notifyWhenPlayerSettlementsChangeCulture == true && 
                Hero.MainHero != null && Hero.MainHero == settlement.Owner)
            {
                InformationManager.DisplayMessage(new InformationMessage($"{settlement.Name} has been cultured by the ways of their owner, {settlement.Owner.Name}"));
            }

            settlement.Culture = settlement.Owner.Culture;
        }

        #endregion
    }
}