namespace LaserCutterImprovements
{
    using UWE;
    using UnityEngine;            
    using System.Collections.Generic;
    using BepInEx;
    using Nautilus;
    using Nautilus.Handlers;
    using Nautilus.Assets.PrefabTemplates;
    using Nautilus.Assets;
    using Nautilus.Crafting;
    using Nautilus.Assets.Gadgets;
    using Ingredient = CraftData.Ingredient;

    //using SMLHelper.V2.Assets;
    //using SMLHelper.V2.Utility;
    //using SMLHelper.V2.Crafting;
    //using SMLHelper.V2.Handlers;






    public class LaserCutterMk3Prefab
    {
        
        /*
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

        */

        public static void Patch()
        {
            /*
            var laserCutterMk3 = new CustomPrefab(
                    "LaserCutterMk3",
                    "Laser Cutter Mk3",
                    "Increases laser power output to allow it to cut a variety of materials");

            */

            Atlas.Sprite sprite = SpriteManager.Get(TechType.LaserCutter);

            TechType customTech = EnumHandler.AddEntry<TechType>("LaserCutterMk3").WithPdaInfo("Laser Cutter Mk3", "Increases laser power output to allow it to cut a variety of materials.").WithIcon(SpriteManager.Get(TechType.LaserCutter));

            PrefabInfo laserCutterMk3 = PrefabInfo.WithTechType("LaserCutterMk3", "Laser Cutter Mk3", "Increases laser power output to allow it to cut a variety of materials.")
                    .WithIcon(sprite)
                    .WithSizeInInventory(new Vector2int(1, 1));                    

            var cloneTemplate = new CloneTemplate(laserCutterMk3, TechType.LaserCutter);

            cloneTemplate.ModifyPrefab += (gameObject) =>
            {
                /*
                Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
                foreach (Renderer rend in renderers)
                {
                    rend.material.color = new Color(55 / 255f, 178 / 255f, 212 / 255f);
                }
                */
                gameObject.EnsureComponent<LaserCutter>();

                /*
                Main_Plugin.logger.LogInfo("Attaching Storage");
                var coi = child.EnsureComponent<ChildObjectIdentifier>();
                if (coi)
                {

                    coi.classId = "EnhancedGravTrapStorage";
                    var storageContainer = coi.gameObject.EnsureComponent<StorageContainer>();
                    storageContainer.prefabRoot = gameObject;
                    storageContainer.storageRoot = coi;

                    storageContainer.width = Main_Plugin.GravTrapStorageWidth.Value;
                    storageContainer.height = Main_Plugin.GravTrapStorageHeight.Value;
                    storageContainer.storageLabel = "Grav trap";
                    storageContainer.errorSound = null;
                    child.SetActive(true);
                }
                else
                {
                    Main_Plugin.logger.LogInfo("Failed to add COI. Unable to attach storage!");
                }

                PickupableStorage pickupableStorage = coi.gameObject.EnsureComponent<PickupableStorage>();
                pickupableStorage.pickupable = gameObject.GetComponent<Pickupable>();
                pickupableStorage.storageContainer = coi.GetComponent<StorageContainer>();
                */
            };

            var lasercuttermk2 = TechTypeUtils.GetModTechType("LaserCutterMk2");

            var recipe = new RecipeData()
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
    },
            };

            var prefab = new CustomPrefab(laserCutterMk3);

            prefab.SetGameObject(cloneTemplate);
            prefab.SetUnlock(TechType.LaserCutter);
            prefab.SetEquipment(EquipmentType.Hand);
            prefab.SetPdaGroupCategory(TechGroup.Personal, TechCategory.Tools);

            prefab.SetRecipe(recipe)
                .WithFabricatorType(CraftTree.Type.Workbench)
                .WithStepsToFabricatorTab("Tools")
                .WithCraftingTime(5f);

            

            

            /*

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
        */

            // Add a recipe for our item, as well as add it to the Moonpool fabricator and Seamoth modules tab
            prefab.SetRecipe(recipe)
        .WithFabricatorType(CraftTree.Type.Workbench)
        .WithStepsToFabricatorTab("Tools")
        .WithCraftingTime(3f);

            //public override float CraftingTime => 3f;

            // Register our item to the game
            prefab.Register();


        }

        /*

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

        

        */

            
            
    }

    
    
        
    
    
}






  


