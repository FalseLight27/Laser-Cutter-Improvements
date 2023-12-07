


namespace LaserCutterImprovements
{
    using UWE;
    using UnityEngine;    
    using HarmonyLib;
   
    
    using System;
    using System.IO;
    using System.Reflection;
    using System.Collections.Generic;

    using System.Text;
    
    
    using System.Collections;
    using System.Linq;
    using System.Threading.Tasks;
    
    
    


    [RequireComponent(typeof(EnergyMixin))]
    
    
         

    public class LaserCutterMk3 : LaserCutter
    {

        public override string animToolName => TechType.Welder.AsString(true);                        

        public VFXEventTypes vfxEventType;

        public static GameObject laserFX;
        
        public static GameObject Streak;        

        public Light fxlight;                 

        public float LaserRange = 3.5f;

        private float timeCanHarvestAgain;

        private float timeCanMineAgain;

        private const float kCooldownDuration = 0.3f;

        private const float jCooldownDuration = 0.1f;
                       
        public FMOD_CustomLoopingEmitter loopHit;

        public Vector3 vector = default(Vector3);

        public new float healthPerWeld = 35f;


        public override void Awake()
        {
            base.Awake();
            
        }

        public override void OnToolUseAnim(GUIHand hand)

        {
            // Variable energy consumption
            float LaserEnergyCost = 1f * Time.deltaTime / 3; // Base usage cost
            float vsBioEnergyCost = 1f * Time.deltaTime / 2; 
            float vsEnviEnergyCost = 1f * Time.deltaTime;
            float LaserDamage = 200f * Time.deltaTime;                                               
                                    
            GameObject gameObject = null;                        

            // Standard UWE hitscan

            UWE.Utils.TraceFPSTargetPosition(Player.main.gameObject, LaserRange, ref gameObject, ref vector, true);          

            float dist = Vector3.Distance(vector, Player.main.transform.position);
            
            // Target detection

            if (gameObject == null)
            {
                InteractionVolumeUser interactionVolume = Player.main.gameObject.GetComponent<InteractionVolumeUser>();

                if (interactionVolume != null && interactionVolume.GetMostRecent() != null)
                {
                    gameObject = interactionVolume.GetMostRecent().gameObject;
                }

                energyMixin.ConsumeEnergy(LaserEnergyCost);
            }

            if (gameObject)
            {
                LiveMixin liveMixin = gameObject.GetComponentInParent<LiveMixin>();

                Drillable drillable = gameObject.GetComponentInParent<Drillable>();                

                TechType techType = CraftData.GetTechType(gameObject);

                HarvestType harvestType = CraftData.GetHarvestTypeFromTech(techType);

                var exosuitdrill = gameObject.AddComponent<ExosuitDrillArm>();

                loopHit = exosuitdrill.loopHit;


                if (drillable)

                {
                    
                    if (Time.time >= timeCanMineAgain)
                    {
                        timeCanMineAgain = Time.time + jCooldownDuration;
                        var entityRoot = Utils.GetEntityRoot(gameObject) ?? gameObject;
                        entityRoot?.GetComponentInChildren<Drillable>()?.OnDrill(vector, null, out var _);
                        //ErrorMessage.AddMessage("ONDRILL");
                        //Logger.Log(Logger.Level.Debug, "ONDRILL");
                    }                    

                    energyMixin.ConsumeEnergy(vsEnviEnergyCost);                   
                    

                }
                
                if (liveMixin)
                {

                    energyMixin.ConsumeEnergy(vsBioEnergyCost);

                    bool wasAlive = liveMixin.IsAlive();
                    liveMixin.TakeDamage(LaserDamage, vector, type: DamageType.Heat, null);
                    GiveResourceOnDamage(gameObject, liveMixin.IsAlive(), wasAlive);

                    if (harvestType != HarvestType.None)
                    {
                        liveMixin.TakeDamage(20f * Time.deltaTime, vector, type: DamageType.Heat, null);
                        GiveResourceOnDamage(gameObject, liveMixin.IsAlive(), wasAlive);
                    }
                }

                

                else
                {

                    LaserCut();
                    energyMixin.ConsumeEnergy(vsEnviEnergyCost);
                }
                               

                gameObject.GetComponentInChildren<BreakableResource>()?.HitResource();                                           
               

                VFXSurface component2 = gameObject.GetComponent<VFXSurface>();
                Vector3 euler = MainCameraControl.main.transform.eulerAngles + new Vector3(100f, 90f, 0f);
                VFXSurfaceTypeManager.main.Play(component2, this.vfxEventType, vector, Quaternion.Euler(euler), Player.main.transform);
                
            }

            

            if (dist <= LaserRange)

            {
                StartLaserCuttingFX();                
            }

            if (dist > LaserRange)

            {
                if (this.playerIKTarget != null)
                {
                    this.playerIKTarget.enabled = true;
                }
                if (this.fxControl != null && this.fxIsPlaying)
                {
                    this.fxControl.StopAndDestroy(0f);
                    this.fxIsPlaying = false;
                    base.CancelInvoke("RandomizeIntensity");
                    this.fxLight.enabled = false;
                }
                if (PlatformUtils.isPS4Platform)
                {
                    PlatformUtils.ResetLightbarColor(0);
                }

            }           

            

            for (int i = 1; i < laserFX.transform.childCount; i++)
            {
                ParticleSystem component = laserFX.transform.GetChild(i).GetComponent<ParticleSystem>();
                if (component)
                {
                    var emission = component.emission;
                    emission.enabled = true;

                    if (!component.isPlaying)
                    {
                        component.Play();
                    }
                }
            }


        }

        

        void GiveResourceOnDamage(GameObject target, bool isAlive, bool wasAlive)
        {
            if (Time.time >= timeCanHarvestAgain)
            {
                TechType techType = CraftData.GetTechType(target);
                HarvestType harvestTypeFromTech = CraftData.GetHarvestTypeFromTech(techType);
                if (techType == TechType.Creepvine)
                {
                    GoalManager.main.OnCustomGoalEvent("Cut_Creepvine");
                }
                if ((harvestTypeFromTech == HarvestType.DamageAlive && wasAlive) || (harvestTypeFromTech == HarvestType.DamageDead && !isAlive))
                {
                    int num = 1;
                    if (harvestTypeFromTech == HarvestType.DamageAlive && !isAlive)
                    {
                        num += CraftData.GetHarvestFinalCutBonus(techType);
                    }
                    TechType harvestOutputData = CraftData.GetHarvestOutputData(techType);
                    if (harvestOutputData != TechType.None)
                    {
                        CraftData.AddToInventory(harvestOutputData, num, false, false);
                    }
                }

                timeCanHarvestAgain = Time.time + kCooldownDuration;

            }
        }

        [HarmonyPatch(typeof(LaserCutter))]
        [HarmonyPatch("RandomizeIntensity")]
        internal class RandomizeIntensityTweak
        {
            [HarmonyPostfix]
            public static void Postfix(LaserCutter __instance)
            {
                if (__instance is LaserCutterMk3 lasercutter)

                {
                    float laserIntensity = __instance.lightIntensity;
                    float newLaserIntensity = UnityEngine.Random.Range(0.7f, 1f);
                    __instance.lightIntensity = newLaserIntensity;
                }
                   


            }

        }
        [HarmonyPatch(typeof(LaserCutter))]
        [HarmonyPatch("UpdateLightbar")]
        internal class ChangeLight
        {
            [HarmonyPostfix]
            public static void Postfix(LaserCutter __instance)
            {
                if (__instance is LaserCutterMk3)

                {                    
                    Color newLaserLightColor = Color.blue;
                    __instance.lightbarColor = newLaserLightColor;
                }



            }

        }
      

        [HarmonyPatch(typeof(LaserCutter))]
        [HarmonyPatch("OnToolUseAnim")]
        internal class ChangeCutPowerCost
        {
            [HarmonyPostfix]
            public static void Postfix(LaserCutter __instance)
            {
                if (__instance is LaserCutterMk3)

                {
                    float laserEnergyCost = __instance.laserEnergyCost;                    
                    float newlaserEnergyCost = 0f;
                    __instance.laserEnergyCost = newlaserEnergyCost;
                }



            }

        }


    }


 }


















