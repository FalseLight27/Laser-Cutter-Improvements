


namespace LaserCutterImprovements
{
    using UWE;
    using UnityEngine;    
    using HarmonyLib;
   


    using BepInEx;
    using Nautilus.Assets.PrefabTemplates;
    using Nautilus.Assets;
    using Nautilus.Crafting;
    using Nautilus.Assets.Gadgets;
    using Ingredient = CraftData.Ingredient;
    using Nautilus.Utility;
    using Nautilus.Handlers;
    using Nautilus.Json;
    using Nautilus.Commands;
    using Nautilus.Options;
    using Nautilus.Extensions;
    using Nautilus.FMod;
    
    // using SMLHelper.V2.Assets;
    //using SMLHelper.V2.Utility;
    //using SMLHelper.V2.Crafting;



    public class LaserCutterMk2Prefab
    {
        /*
       public LaserCutterMk2Prefab(string classId, string friendlyName, string description) : base("LaserCutterMk2", "Laser Cutter Mk 2", "Removes built-in safety features, allowing the Laser Cutter to be used on organic targets")
            
        {
            //CRITICAL
            OnFinishedPatching += () =>
            {                
                TechTypeUtils.AddModTechType(this.TechType);
            };

        }

        */

        

        /*
        public override string AssetsFolder => base.AssetsFolder;

        public override string IconFileName => base.IconFileName;

        public override Vector2int SizeInInventory => base.SizeInInventory;

        public override List<SpawnLocation> CoordinatedSpawns => base.CoordinatedSpawns;

        public override List<LootDistributionData.BiomeData> BiomesToSpawnIn => base.BiomesToSpawnIn;

        public override WorldEntityInfo EntityInfo => base.EntityInfo;

        public override bool HasSprite => base.HasSprite;        

        public override bool AddScannerEntry => base.AddScannerEntry;

       public override PDAEncyclopedia.EntryData EncyclopediaEntryData => base.EncyclopediaEntryData;

        
                      
        

        public override bool UnlockedAtStart => false;

        public override string DiscoverMessage => base.DiscoverMessage;

        */




        public static TechType techType;

        

        public static void Patch()
        {
            /*
            var laserCutterMk2 = new CustomPrefab(
                    "LaserCutterMk2",
                    "Laser Cutter Mk2",
                   "Removes built-in safety features, allowing the Laser Cutter to be used on organic targets.");
            */

            TechType customTech = EnumHandler.AddEntry<TechType>("LaserCutterMk2").WithPdaInfo("Laser Cutter Mk 2", "Removes built-in safety features, allowing the Laser Cutter to be used on organic targets.").WithIcon(SpriteManager.Get(TechType.LaserCutter));

            Atlas.Sprite sprite = SpriteManager.Get(TechType.LaserCutter);


            PrefabInfo laserCutterMk2 = PrefabInfo.WithTechType("LaserCutterMk2", "Laser Cutter Mk2", "Removes built-in safety features, allowing the Laser Cutter to be used on organic targets.")
                    .WithIcon(sprite)
                    .WithSizeInInventory(new Vector2int(1, 1));

            var cloneTemplate = new CloneTemplate(laserCutterMk2, TechType.LaserCutter);

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

            var recipe = new RecipeData()
            {
                craftAmount = 1,
                Ingredients =
    {
                new Ingredient(TechType.LaserCutter, 1),
                new Ingredient(TechType.WiringKit, 1),
    },
            };

            var prefab = new CustomPrefab(laserCutterMk2);

            prefab.SetGameObject(cloneTemplate);
            prefab.SetUnlock(TechType.LaserCutter);
            prefab.SetEquipment(EquipmentType.Hand);
            prefab.SetPdaGroupCategory(TechGroup.Personal, TechCategory.Tools);

            prefab.SetRecipe(recipe)
                .WithFabricatorType(CraftTree.Type.Workbench)
                .WithStepsToFabricatorTab("Tools")
                .WithCraftingTime(3f);


            // Set our prefab to a clone of the Seamoth electrical defense module
            ////laserCutterMk2.SetGameObject(new CloneTemplate(laserCutterMk2.Info, TechType.LaserCutter));

            // Make our item compatible with the seamoth module slot
            ////laserCutterMk2.SetEquipment(EquipmentType.Hand).WithQuickSlotType(QuickSlotType.Selectable);

            //public override EquipmentType EquipmentType => EquipmentType.Hand;

            // Make the Vehicle upgrade console a requirement for our item's blueprint
            ////ScanningGadget scanning = laserCutterMk2.SetUnlock(TechType.LaserCutter);

            //public override TechType RequiredForUnlock => TechType.LaserCutter;

            // Add our item to the Vehicle upgrades category
            ////scanning.WithPdaGroupCategory(TechGroup.Personal, TechCategory.Tools);

            //public override TechGroup GroupForPDA => TechGroup.Personal;
            //public override TechCategory CategoryForPDA => TechCategory.Tools;


            

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
            /*
            laserCutterMk2.SetRecipe(recipe)
        .WithFabricatorType(CraftTree.Type.Workbench)
        .WithStepsToFabricatorTab("Tools")
        .WithCraftingTime(3f);
            */

            //public override float CraftingTime => 3f;

            // Register our item to the game
            prefab.Register();


        }



        
       
        /*


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

       */        


    }

    
    
        
    
    
}






  


