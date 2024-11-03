using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;
using UWE;
using VehicleFramework.Engines;
using VehicleFramework.VehicleParts;
using VehicleFramework;
using VehicleFramework.VehicleTypes;
using static Oculus.Platform.Models.Product.Offer;
using System.IO;
using AircraftLib;
using AircraftLib.Engines;
using AircraftLib.VehicleTypes;
using AircraftLib.Managers;

namespace Eureka_Jet
{
    public class Eureka : PlaneVehicle
    {
        public static GameObject model;

        public static Atlas.Sprite pingSprite;

        public static Atlas.Sprite crafterSprite;

        protected float _maxSpeed = 60f;
        public override float maxSpeed
        {
            get
            {
                return _maxSpeed;
            }
            set
            {
                _maxSpeed = value;
            }
        }

        protected float _takeoffSpeed = 20f;
        public override float takeoffSpeed
        {
            get
            {
                return _takeoffSpeed;
            }
            set
            {
                _takeoffSpeed = value;
            }
        }

        public override void Update()
        {
            base.Update();

            FlightManager.CheckLandingGear(this);
        }

        public static void GetAssets()
        {
            string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            AssetBundle assetBundle = AssetBundle.LoadFromFile(Path.Combine(directory, "Assets/eureka"));
            bool flag = assetBundle == null;
            if (flag)
            {
                VehicleFramework.Logger.Log("Failure loading assetbundle");
            }
            else
            {
                object[] array = assetBundle.LoadAllAssets();
                object[] array2 = array;
                foreach (object obj in array2)
                {
                    bool flag2 = obj.ToString().Contains("SpriteAtlas");
                    if (flag2)
                    {
                        SpriteAtlas spriteAtlas = (SpriteAtlas)obj;
                        Sprite sprite = spriteAtlas.GetSprite("EurekaPingSprite");
                        Eureka.pingSprite = new Atlas.Sprite(sprite, false);
                        Sprite sprite2 = spriteAtlas.GetSprite("EurekaCrafterSprite");
                        Eureka.crafterSprite = new Atlas.Sprite(sprite2, false);
                    }
                    else
                    {
                        bool flag3 = obj.ToString().Contains("vehicle");
                        if (flag3)
                        {
                            Eureka.model = (GameObject)obj;
                        }
                    }
                }
            }
        }
        public static IEnumerator Register()
        {
            GetAssets();
            ModVehicle Eureka = Radical.EnsureComponent<Eureka>(model);
            Eureka.name = "Eureka";
            yield return CoroutineHost.StartCoroutine(VehicleRegistrar.RegisterVehicle(Eureka));
            yield break;
        }

        public override Dictionary<TechType, int> Recipe
        {
            get
            {
                return new Dictionary<TechType, int>
                {
                    {
                        TechType.PlasteelIngot, 2
                    },
                    {
                        TechType.AdvancedWiringKit, 3
                    },
                    {
                        TechType.Lubricant, 1
                    },
                    {
                        TechType.EnameledGlass, 2
                    },
                    {
                        TechType.PowerCell, 2
                    },
                    {
                        TechType.AramidFibers, 4
                    },
                    {
                        TechType.Aerogel, 4
                    }
                };
            }
        }

        public override string vehicleDefaultName
        {
            get
            {
                Language main = Language.main;
                bool flag = !(main != null);
                string result;
                if (flag)
                {
                    result = "Eureka";
                }
                else
                {
                    result = main.Get("EurekaDefaultName");
                }
                return result;
            }
        }

        public override string Description
        {
            get
            {
                return "Sleek private jet to travel in style";
            }
        }

        public override string EncyclopediaEntry
        {
            get
            {
                string str = "The Alterra Eureka is a sleek modern looking private jet that is able to cut across continents at mach 4.";
                str += "It is most well known for it\'s popularity amongst Blizzard Corp employees.\n";
                str += "\nIt features:\n";
                str += "- Cutting edge flight controls\n";
                str += "- Luxurious customizable interior\n";
                str += "- Variable airfoil to ensure stability in any fluid\n";
                str += "- Automatic landing gear functionality\n";
                str += "- Onboard advanced AI\n";
                str += "\nRatings:\n";
                str += "- Top Speed: 60m/s\n";
                str += "- Max Acceleration: 6m/s/s\n";
                str += "- Power: Two replaceable power cells in each wing\n";
                str += "- Dimensions: 21.4m x 24.7m x 6.5m\n";
                str += "- Passenger capacity: 8\n";
                return str + "\n\'The Alterra Eureka: The most stylish way to travel.\'\n";
            }
        }

        public override GameObject VehicleModel
        {
            get
            {
                return Eureka.model;
            }
        }

        public override GameObject StorageRootObject
        {
            get
            {
                return base.transform.Find("StorageRoot").gameObject;
            }
        }

        public override GameObject ModulesRootObject
        {
            get
            {
                return base.transform.Find("ModulesRoot").gameObject;
            }
        }

        public override VehiclePilotSeat PilotSeat
        {
            get
            {
                VehiclePilotSeat result = default(VehiclePilotSeat);
                Transform transform = base.transform.Find("Pilotseat");
                result.Seat = transform.gameObject;
                result.SitLocation = transform.Find("SitLocation").gameObject;
                result.LeftHandLocation = transform;
                result.RightHandLocation = transform;
                return result;
            }
        }

        public override List<VehicleHatchStruct> Hatches
        {
            get
            {
                List<VehicleHatchStruct> list = new List<VehicleHatchStruct>();
                VehicleHatchStruct vehicleHatchStruct = default(VehicleHatchStruct);
                Transform transform = base.transform.Find("Hatch");
                vehicleHatchStruct.Hatch = transform.gameObject;
                vehicleHatchStruct.ExitLocation = transform.Find("ExitPosition");
                vehicleHatchStruct.SurfaceExitLocation = transform.Find("ExitPosition");
                list.Add(vehicleHatchStruct);
                VehicleHatchStruct item = default(VehicleHatchStruct);
                item.Hatch = base.transform.Find("Collider").gameObject;
                item.ExitLocation = vehicleHatchStruct.ExitLocation;
                item.SurfaceExitLocation = vehicleHatchStruct.ExitLocation;
                list.Add(item);
                return list;
            }
        }

        public override List<VehicleStorage> InnateStorages
        {
            get
            {
                return new List<VehicleStorage>();
            }
        }

        public override List<VehicleUpgrades> Upgrades
        {
            get
            {
                List<VehicleUpgrades> list = new List<VehicleUpgrades>();
                VehicleUpgrades vehicleUpgrades = default(VehicleUpgrades);
                vehicleUpgrades.Interface = base.transform.Find("Upgrades").gameObject;
                vehicleUpgrades.Flap = vehicleUpgrades.Interface;
                list.Add(vehicleUpgrades);
                return list;
            }
        }

        public override List<VehicleBattery> Batteries
        {
            get
            {
                List<VehicleBattery> list = new List<VehicleBattery>();
                VehicleBattery item = default(VehicleBattery);
                item.BatterySlot = base.transform.Find("Batteries/Battery1").gameObject;
                list.Add(item);
                VehicleBattery item2 = default(VehicleBattery);
                item2.BatterySlot = base.transform.Find("Batteries/Battery2").gameObject;
                list.Add(item2);
                return list;
            }
        }

        public override List<VehicleBattery> BackupBatteries
        {
            get
            {
                return null;
            }
        }

        public List<VehicleFloodLight> FloodLights
        {
            get
            {
                return null;
            }
        }

        public override List<VehicleFloodLight> HeadLights
        {
            get
            {
                List<VehicleFloodLight> list = new List<VehicleFloodLight>();
                List<VehicleFloodLight> list2 = list;
                VehicleFloodLight item = default(VehicleFloodLight);
                item.Light = base.transform.Find("lights_parent/headlights/L").gameObject;
                item.Angle = 70f;
                item.Color = Color.white;
                item.Intensity = 0.6f;
                item.Range = 400f;
                list2.Add(item);
                List<VehicleFloodLight> list3 = list;
                item = default(VehicleFloodLight);
                item.Light = base.transform.Find("lights_parent/headlights/R").gameObject;
                item.Angle = 70f;
                item.Color = Color.white;
                item.Intensity = 0.6f;
                item.Range = 400f;
                list3.Add(item);
                return list;
            }
        }

        public override List<GameObject> WaterClipProxies
        {
            get
            {
                List<GameObject> list = new List<GameObject>();
                foreach (object obj in transform.Find("WaterClipProxies"))
                {
                    Transform transform = (Transform)obj;
                    list.Add(transform.gameObject);
                }
                return list;
            }
        }

        public override List<GameObject> CanopyWindows
        {
            get
            {
                return new List<GameObject>
                {
                    base.transform.Find("Model/CanopyInner").gameObject,
                    base.transform.Find("Model/CanopyOuter").gameObject,
                };
            }
        }

        public override GameObject BoundingBox
        {
            get
            {
                return base.transform.Find("BoundingBox").gameObject;
            }
        }

        public override GameObject CollisionModel
        {
            get
            {
                return base.transform.Find("Collider").gameObject;
            }
        }

        public override ModVehicleEngine Engine
        {
            get
            {
                return Radical.EnsureComponent<PlaneRCJetEngine>(base.gameObject);
            }
        }

        public override Atlas.Sprite PingSprite
        {
            get
            {
                return Eureka.pingSprite;
            }
        }

        public override int BaseCrushDepth
        {
            get
            {
                return 10;
            }
        }

        public override int CrushDepthUpgrade1
        {
            get
            {
                return 20;
            }
        }

        public override int CrushDepthUpgrade2
        {
            get
            {
                return 50;
            }
        }

        public override int CrushDepthUpgrade3
        {
            get
            {
                return 100;
            }
        }

        public override int MaxHealth
        {
            get
            {
                return 2000;
            }
        }

        public override int Mass
        {
            get
            {
                return 7000;
            }
        }

        public override int NumModules
        {
            get
            {
                return 8;
            }
        }

        public override bool HasArms
        {
            get
            {
                return false;
            }
        }

        public override Atlas.Sprite CraftingSprite
        {
            get
            {
                return Eureka.crafterSprite;
            }
        }

        public override List<VehicleStorage> ModularStorages
        {
            get
            {
                return null;
            }
        }
    }
}
