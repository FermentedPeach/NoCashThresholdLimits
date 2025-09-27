using BTD_Mod_Helper;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2CppAssets.Scripts.Models.Rounds;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Unity.UI_New.InGame.Stats;
using Il2CppAssets.Scripts.Unity.UI_New.Popups;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppTMPro;
using MelonLoader;
using NoCashThresholdLimits;
using System;
using UnityEngine;
using UnityEngine.UI;

[assembly: MelonInfo(typeof(NoCashThresholdLimits.NoCashThresholdLimits), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace NoCashThresholdLimits;

public class NoCashThresholdLimits : BloonsTD6Mod
{
    public override void OnInGameLoaded(InGame game)
    {
        Il2CppStructArray<RoundThresholdMultiplier> thresholds = new Il2CppStructArray<RoundThresholdMultiplier>(1);

        var threshold = new RoundThresholdMultiplier();
        threshold.threshold = 1;
        threshold.multiplier = 1.0f;
        thresholds[0] = threshold;

        game.GetGameModel().incomeSet = new IncomeSetModel("_NewIncomeSet", thresholds, 1.0f);

    }

    public override void OnApplicationStart()
    {
        ModHelper.Msg<NoCashThresholdLimits>("NoCashThresholdLimits loaded!");
    }

    static public void Log(ConsoleColor color, object message)
    {
        if (message != null)
            MelonLogger.Msg(color, message);
    }

    //Fix the cash popup to give above 9 999 999 cash :)
    [HarmonyPatch(typeof(Popup), nameof(Popup.ShowPopup))]
    internal static class Popup_ShowPopup
    {
        [HarmonyPrefix]
        private static void Prefix(Popup __instance)
        {

            if (__instance.gameObject.name.Contains("SetValuePopup") && __instance.title.m_text.Contains("Set Cash"))
            {
                var inputField = __instance.GetComponentInChildren<TMP_InputField>();
                var button = __instance.GetComponentInChildren<Button>();

                inputField.characterLimit = 19; //assume signed long (64 bit)
                button.AddOnClick(() =>
                {
                    double outVal = 0.0;
                    double.TryParse(inputField.m_Text.Replace(",", "").Replace("$", ""), out outVal);
                    InGame.instance.bridge.SetCash(outVal);
                });
            }
        }
    }

    //Fix the cash display to display above 9 999 999 cash
    [HarmonyPatch(typeof(CashDisplay), nameof(CashDisplay.OnCashChanged))]
    internal static class CashDisplay_OnCashChanged
    {
        [HarmonyPostfix]
        private static void Postfix(CashDisplay __instance)
        {
            __instance.text.SetText("$" + (InGame.instance.bridge.GetCash()).ToString("N0"));
        }
    }

}