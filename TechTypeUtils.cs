using UnityEngine;
using System.Collections.Generic;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using UWE;
using Logger = QModManager.Utility.Logger;

namespace LaserCutterImprovements
{
    public class TechTypeUtils
    {
        public static Dictionary<string, TechType> ModTechTypes = new Dictionary<string, TechType>();

        public static Dictionary<string, GameObject> ModPrefabs = new Dictionary<string, GameObject>();

        internal static void AddModTechType(TechType techtype, GameObject prefab = null)
        {
            Logger.Log(Logger.Level.Info, "Adding mod TechType" + techtype.AsString());
            string key = techtype.AsString(true);
            if (!ModTechTypes.ContainsKey(key))
            {
                ModTechTypes.Add(key, techtype);
            }
            if (prefab != null)
            {
                ModPrefabs[key] = prefab;
            }
        }

        public static bool TryGetModTechType(string key, out TechType techType)
        {
            techType = GetModTechType(key);
            return (techType != TechType.None);
        }

        public static TechType GetTechType(string value)
        {
            if (string.IsNullOrEmpty(value))
                return TechType.None;

            // Look for a known TechType
            if (TechTypeExtensions.FromString(value, out TechType tType, true))
                return tType;

            //  Not one of the known TechTypes - is it registered with SMLHelper?
            if (TryGetModTechType(value, out TechType custom))
                return custom;
            
            return TechType.None;
        }

        public static TechType GetModTechType(string key)
        {
            string lowerKey = key.ToLower();
            TechType tt;
            if (ModTechTypes.TryGetValue(lowerKey, out tt))
                return tt;

            return GetTechType(key);
        }
    }
}
