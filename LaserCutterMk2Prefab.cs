using UnityEngine;
using System.Collections.Generic;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using UWE;


namespace LaserCutterImprovements
{

     
    internal class LaserCutterMk2Prefab : Equipable
    {

       public LaserCutterMk2Prefab(string classId, string friendlyName, string description) : base("LaserCutterMk2", "Laser Cutter Mk 2", "Removes built-in safety features, allowing the Laser Cutter to be used on organic targets")
            
        {

            OnFinishedPatching += () =>
            {                
                TechTypeUtils.AddModTechType(this.TechType);
            };

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
                return new TechData()
                {
                        craftAmount = 1,
                        Ingredients =
                        {
                        new Ingredient(TechType.LaserCutter, 1),
                        new Ingredient(TechType.WiringKit, 1),
                        }
                };
            }

       



        public override GameObject GetGameObject()
                {
                    GameObject LaserCutterMk2 = CraftData.GetPrefabForTechType(TechType.LaserCutter);
                    var obj = GameObject.Instantiate(LaserCutterMk2);
                    GameObject.DestroyImmediate(obj.GetComponent<LaserCutter>());
                    
                    obj.EnsureComponent<EnergyMixin>();

                    var laser = obj.EnsureComponent<LaserCutterMk2>();
                    laser.ikAimRightArm = true;
                    laser.laserCutSound = obj.GetComponent<FMODASRPlayer>();
                    laser.fxControl = obj.GetComponentInChildren<VFXController>();
                    laser.fxLight = obj.GetComponentInChildren<Light>(true);
                    laser.mainCollider = obj.GetComponent<CapsuleCollider>();

                    laser.drawSound = ScriptableObject.CreateInstance<FMODAsset>();
                    laser.drawSound.path = "event:/tools/lasercutter/deploy";

                    laser.firstUseSound = obj.GetComponent<FMOD_CustomEmitter>();
                    laser.pickupable = obj.GetComponent<Pickupable>();

           

                    
                    
                    
            return obj;
                }

        protected override Atlas.Sprite GetItemSprite()
        {
            return SpriteManager.Get(TechType.LaserCutter);
        }
        public static Atlas.Sprite CustomSprite => SpriteManager.Get(TechType.LaserCutter);

               


    }

    
    
        
    
    
}






  


