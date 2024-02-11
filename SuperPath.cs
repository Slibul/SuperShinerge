using HarmonyLib;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Player;
using Characters.Gear.Synergy;
using Characters.Gear.Items;
using Characters.Actions;
using UnityEngine.AddressableAssets;
using Characters.Gear.Synergy.Inscriptions.FairyTaleSummon;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using Characters;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using Level;
using Services;
using Singletons;
using GameResources;
using UI.TestingTool;
using System.Collections;
using System;
using Characters.Gear;

namespace SuperSynerge;

public class SuperPath
{
    
    [HarmonyPostfix]
    [HarmonyPatch((typeof(Inscription)), nameof(Inscription.isSuper),MethodType.Getter)]
    static void GetSuperIsTrue(ref bool __result,ref Inscription __instance)
    {
        if (Plugin.isNoSuper.Value) { return; }
        if(__instance.key != Inscription.Key.Masterpiece && __instance.key != Inscription.Key.SunAndMoon && __instance.key != Inscription.Key.Omen && __instance.key != Inscription.Key.Sin)
            __result = true;
    }

    [HarmonyPrefix]
    [HarmonyPatch((typeof(Inventory)), nameof(Inventory.UpdateSynergy))]
    static bool InscriptionFull(ref Synergy ___synergy,ref System.Action ___onUpdatedKeywordCounts,ref ItemInventory ___item)
    {
        if(Plugin.isNoFull.Value) 
        {
            return true;
        }

        foreach (Inscription inscription in ___synergy.inscriptions)
        {
            if (Plugin.isOmen.Value == false)
            {
                if (inscription.key != Inscription.Key.Omen)
                    inscription.count = inscription.maxStep;
            }else
            {
                if (inscription.key == Inscription.Key.Omen)
                    inscription.count = inscription.maxStep;
            }
        }
        foreach (Item item in ___item.items)
        {
            if (!(item == null))
            {
                if(item.keyword1 != Inscription.Key.Omen && Plugin.isNoOmen.Value == false)
                    ___synergy.inscriptions[item.keyword1].count++;
                else if (Plugin.isNoOmen.Value == true)
                    ___synergy.inscriptions[item.keyword1].count++;


                if (item.keyword2 != Inscription.Key.Omen && Plugin.isNoOmen.Value == false)
                    ___synergy.inscriptions[item.keyword2].count++;
                else if (Plugin.isNoOmen.Value == true)
                    ___synergy.inscriptions[item.keyword2].count++;
            }
        }
        foreach (Item item2 in ___item.items)
        {
            if (!(item2 == null))
            {
                foreach (Item.BonusKeyword bonusKeyword2 in item2.bonusKeyword)
                {
                    bonusKeyword2.Evaluate();
                    EnumArray<Inscription.Key, int> values = bonusKeyword2.Values;
                    foreach (Inscription.Key key in Inscription.keys)
                    {
                        if(key != Inscription.Key.Omen && Plugin.isNoOmen.Value == false)
                            ___synergy.inscriptions[key].count += values[key];
                        else if(Plugin.isNoOmen.Value == true)
                            ___synergy.inscriptions[key].count += values[key];
                    }
                }
            }
        }
        foreach (Inscription.Key key2 in Inscription.keys)
        {
            ___synergy.inscriptions[key2].count += ___synergy.inscriptions[key2].bonusCount;
        }
        System.Action action = ___onUpdatedKeywordCounts;
        if (action != null)
        {
            action();
        }
        
        ___synergy.UpdateBonus();

        return false;
    }
}


