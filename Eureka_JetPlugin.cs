using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UWE;

namespace Eureka_Jet
{

    [BepInPlugin(MyGUID, PluginName, VersionString)]
    [BepInDependency("com.Bobasaur.AircraftLib")]
    [BepInDependency("com.mikjaw.subnautica.vehicleframework.mod")]
    [BepInDependency("com.snmodding.nautilus")]
    public class Eureka_JetPlugin : BaseUnityPlugin
    {
        private const string MyGUID = "com.Bobasaur.Eureka_Jet";
        private const string PluginName = "Eureka_Jet";
        private const string VersionString = "1.0.0";

        private static readonly Harmony Harmony = new Harmony(MyGUID);
        public static ManualLogSource Log = new ManualLogSource(PluginName);

        private void Awake()
        {
            Logger.LogInfo($"Will load {PluginName} version {VersionString}.");
            Harmony.PatchAll();
            Logger.LogInfo($"{PluginName} version {VersionString} is loaded.");

            Log = Logger;

            CoroutineHost.StartCoroutine(Eureka.Register());
        }
    }
}
