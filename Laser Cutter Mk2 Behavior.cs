



namespace LaserCutterImprovements
{
    using System.Linq;
    using UWE;
    using UnityEngine;
    using Logger = QModManager.Utility.Logger;
    using HarmonyLib;


    [RequireComponent(typeof(EnergyMixin))]
    
    
         

    public class LaserCutterMk2 : LaserCutter
    {

        public override string animToolName => TechType.Welder.AsString(true);     
                      
        public VFXEventTypes vfxEventType;

        public GameObject laserCutStreak;

        public VFXController fxcontrol;

        public Light fxlight;

        public GameObject laserCutFX;        

        public float LaserRange = 2.5f;

        private float timeCanUseAgain;

        private const float kCooldownDuration = 0.5f;

        public new float healthPerWeld = 28f;
              


    public override void OnToolUseAnim(GUIHand hand)

        {
            
            float LaserEnergyCost = 1f * Time.deltaTime / 2;
            float LaserDamage = 30f * Time.deltaTime;            

            energyMixin.ConsumeEnergy(LaserEnergyCost);            

            Vector3 vector = default(Vector3);
            GameObject gameObject = null;
            UWE.Utils.TraceFPSTargetPosition(Player.main.gameObject, LaserRange, ref gameObject, ref vector, true);

            float dist = Vector3.Distance(vector, Player.main.transform.position);
                        

            if (gameObject == null)
            {
                InteractionVolumeUser interactionVolume = Player.main.gameObject.GetComponent<InteractionVolumeUser>();

                if (interactionVolume != null && interactionVolume.GetMostRecent() != null)
                {
                    gameObject = interactionVolume.GetMostRecent().gameObject;
                }
            }

            if (gameObject)
            {
                LiveMixin liveMixin = gameObject.GetComponentInParent<LiveMixin>();                

                TechType techType = CraftData.GetTechType(gameObject);
                HarvestType harvestType = CraftData.GetHarvestTypeFromTech(techType);


                if (liveMixin)
                {
                    bool wasAlive = liveMixin.IsAlive();
                    liveMixin.TakeDamage(LaserDamage, vector, type: DamageType.Heat, null);
                    GiveResourceOnDamage(gameObject, liveMixin.IsAlive(), wasAlive);

                    if (harvestType != HarvestType.None)
                    {
                        liveMixin.TakeDamage(15f * Time.deltaTime, vector, type: DamageType.Heat, null);
                        GiveResourceOnDamage(gameObject, liveMixin.IsAlive(), wasAlive);
                    }


                }               


                else
                {
                    LaserCut();
                }

                gameObject.GetComponentInChildren<BreakableResource>()?.HitResource();

                VFXSurface component2 = gameObject.GetComponent<VFXSurface>();
                Vector3 euler = MainCameraControl.main.transform.eulerAngles + new Vector3(100f, 90f, 0f);
                VFXSurfaceTypeManager.main.Play(component2, VFXEventTypes.heatBlade, vector, Quaternion.Euler(euler), Player.main.transform);

            }

            if (dist <= LaserRange)

            {
                StartLaserCuttingFX();
                Instantiate(laserCutStreak, vector, Quaternion.identity);
                Instantiate(laserCutFX, vector, Quaternion.identity);
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



            ParticleSystem component = this.laserCutFX.transform.GetComponent<ParticleSystem>();
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

        

        void GiveResourceOnDamage(GameObject target, bool isAlive, bool wasAlive)
        {
            if (Time.time >= timeCanUseAgain)
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

                timeCanUseAgain = Time.time + kCooldownDuration;

            }
        }

        [HarmonyPatch(typeof(LaserCutter))]
        [HarmonyPatch("RandomizeIntensity")]
        internal class RandomizeIntensityTweak
        {
            [HarmonyPostfix]
            public static void Postfix(LaserCutter __instance)
            {
                if (__instance is LaserCutterMk2 lasercutter)

                {
                    float laserIntensity = __instance.lightIntensity;
                    float newLaserIntensity = UnityEngine.Random.Range(0f, 1.8f);
                    __instance.lightIntensity = newLaserIntensity;
                }
                   


            }

        }
    }


 }


















