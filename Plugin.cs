using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using SuperSynerge;


[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public static ConfigEntry<bool> isNoSuper;
    public static ConfigEntry<bool> isNoFull;
    public static ConfigEntry<bool> isNoOmen;
    public static ConfigEntry<bool> isOmen;
    private void Awake()
    {
        isNoSuper = Config.Bind<bool>("General", "각인 정상화", false, "각인을 평범하게 바꿉니다.");
        isNoFull = Config.Bind<bool>("General", "각인 강제 맥스 정상화", false, "각인을 평범하게 바꿉니다.");
        isNoOmen = Config.Bind<bool>("General", "흉조 추가불가 정상화", false, "각인을 평범하게 바꿉니다.");
        isOmen = Config.Bind<bool>("General", "3흉조화", false, "3흉조로 시작합니다.");
        Harmony.CreateAndPatchAll(typeof(SuperPath));
        Logger.LogInfo($"Mod {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void Update()
    {
        Config.Reload();
    }
}
