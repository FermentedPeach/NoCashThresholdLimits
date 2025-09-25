using BTD_Mod_Helper;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Rounds;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using MelonLoader;
using NoCashThresholdLimits;

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
}