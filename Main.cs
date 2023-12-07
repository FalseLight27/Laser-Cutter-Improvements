using System.Reflection;
using HarmonyLib;
using BepInEx;
using Nautilus;
using UWE;
using UnityEngine;
using System;
using System.IO;


namespace LaserCutterImprovements



{



    [BepInPlugin(myGUID, pluginName, versionString)]
    public class Main_Plugin : BaseUnityPlugin

    {
        private const string myGUID = "FalseLight.LaserCutterImprovements";
        private const string pluginName = "Laser Cutter Improvements";
        private const string versionString = "0.2b";

        //private static readonly string ConfigFilePath = Path.Combine(Path.GetDirectoryName(Paths.BepInExConfigPath), "LaserCutterImprovements.json");

        private static readonly Harmony harmony = new Harmony(myGUID);

        private void Awake()
        {

            /*
            var lasercuttermk2 = new LaserCutterMk2Prefab("LaserCutterMk2", "Laser Cutter Mk 2", "Removes built-in safety features, allowing the Laser Cutter to be used on organic targets");
            lasercuttermk2.Patch();
            var lasercuttermk2tech = lasercuttermk2.TechType;

            var lasercuttermk3 = new LaserCutterMk3Prefab("LaserCutterMk3", "Laser Cutter Mk 3", "MINING LASER");
            lasercuttermk3.Patch();
            var lasercuttermk3tech = lasercuttermk3.TechType;

            //CraftTreeHandler.AddTabNode(CraftTree.Type.Workbench, "LaserCutterMods", "Laser Cutter Upgrades", SpriteManager.Get(TechType.LaserCutter));
            //CraftTreeHandler.AddCraftingNode(CraftTree.Type.Workbench, lasercuttermk2tech, "LaserCutterMods", "Laser Cutter Mk2");
            //CraftTreeHandler.AddCraftingNode(CraftTree.Type.Workbench, lasercuttermk3tech, "LaserCutterMods", "Laser Cutter Mk3");

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "com.falselight.subnautica.lasercutterimprovements.mod");
                Logger.Log(Logger.Level.Info, "Patched successfully.");
            */

            LaserCutterMk2Prefab.Patch();
            LaserCutterMk3Prefab.Patch();

            harmony.PatchAll();

            Logger.LogInfo($"{pluginName} {versionString} Loaded.");

        }
        }
    }


