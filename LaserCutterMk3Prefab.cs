using UnityEngine;
using System.Collections.Generic;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using UWE;


namespace LaserCutterImprovements
{
     
    internal class LaserCutterMk3Prefab : Equipable
    {
        

        public LaserCutterMk3Prefab(string classId, string friendlyName, string description) : base("LaserCutterMk3", "Laser Cutter Mk 3", "MINING LASER")
           
        {
          
        }

        public override string AssetsFolder => base.AssetsFolder;

        public override string IconFileName => base.IconFileName;

        public override Vector2int SizeInInventory => base.SizeInInventory;

        public override List<SpawnLocation> CoordinatedSpawns => base.CoordinatedSpawns;

        public override List<LootDistributionData.BiomeData> BiomesToSpawnIn => base.BiomesToSpawnIn;

        public override WorldEntityInfo EntityInfo => base.EntityInfo;

        public override bool HasSprite => base.HasSprite;

        public override TechType RequiredForUnlock => TechType.LaserCutter;

        public override bool AddScannerEntry => base.AddScannerEntry;

       public override PDAEncyclopedia.EntryData EncyclopediaEntryData => base.EncyclopediaEntryData;

        public override TechGroup GroupForPDA => TechGroup.Personal;

        public override TechCategory CategoryForPDA => TechCategory.Tools;

        public override bool UnlockedAtStart => false;

        public override string DiscoverMessage => base.DiscoverMessage;

        public override float CraftingTime => 3f;

        public override EquipmentType EquipmentType => EquipmentType.Hand;

        

        protected override TechData GetBlueprintRecipe()
            {

            var lasercuttermk2 = TechTypeUtils.GetModTechType("LaserCutterMk2");

            return new TechData()
                {
                        craftAmount = 1,
                        Ingredients =
                        {
                        new Ingredient(lasercuttermk2, 1),
                        new Ingredient(TechType.ComputerChip, 1),
                        new Ingredient(TechType.Magnetite, 1),
                        new Ingredient(TechType.Sulphur, 1),
                        new Ingredient(TechType.AluminumOxide, 1),
                        new Ingredient(TechType.Diamond, 1),
                        }
                };
            }

       



        public override GameObject GetGameObject()
                {
                    GameObject LaserCutterMk3 = CraftData.GetPrefabForTechType(TechType.LaserCutter);
                    
                    var obj = GameObject.Instantiate(LaserCutterMk3);                  

                    GameObject.DestroyImmediate(obj.GetComponent<LaserCutter>());
                    
                    obj.EnsureComponent<EnergyMixin>();

                    var laser3 = obj.EnsureComponent<LaserCutterMk3>();
                    laser3.ikAimRightArm = true;
                    laser3.laserCutSound = obj.GetComponent<FMODASRPlayer>();
                                       
                    laser3.fxControl = obj.GetComponentInChildren<VFXController>();
                    laser3.fxLight = obj.GetComponentInChildren<Light>(true);
                    laser3.mainCollider = obj.GetComponent<CapsuleCollider>();                    
            
                    laser3.drawSound = ScriptableObject.CreateInstance<FMODAsset>();
                    laser3.drawSound.path = "event:/tools/lasercutter/deploy";
                    
                    laser3.firstUseSound = obj.GetComponent<FMOD_CustomEmitter>();
                    laser3.pickupable = obj.GetComponent<Pickupable>();

                           

            return obj;
                }

        protected override Atlas.Sprite GetItemSprite()
        {
            return SpriteManager.Get(TechType.LaserCutter);
        }
        public static Atlas.Sprite CustomSprite => SpriteManager.Get(TechType.LaserCutter);

        

        

            
            
    }

    
    
        
    
    
}






  


