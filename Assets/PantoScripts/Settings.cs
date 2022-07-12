using UnityEngine;

namespace DualPantoFramework
{

    public enum SpeedControlStrategy
    {
        MAX_SPEED,
        EXPLORATION,
        LEASH
    }

    public class Settings : PantoBehaviour
    {
        public bool tetheringEnabled;
        public float tetherFactor;
        public float innerRadius;
        public float outerRadius;
        public bool pockEnabled;
        public IntroContourStrategy introContourOption;
        public SpeedControlStrategy speedControlOption;
        public bool visualizeMePath;
        public bool visualizeItPath;
        void UpdateSettings()
        {
            if (!pantoSync.debug)
            {
                pantoSync.SetSpeedControl(tetheringEnabled, tetherFactor, innerRadius, outerRadius, speedControlOption, pockEnabled);
                //SyncSettings(tetherFactor, innerRadius, outerRadius, speedControlOption);
            }
        }

        void CreateTrails()
        {
            if (visualizeItPath)
            {
                GameObject x = GameObject.Find("MeHandleGodObject");
                GameObject go = Instantiate(Resources.Load("ItVisualizationParticles"), x.transform) as GameObject;
            }
            if (visualizeMePath)
            {
                GameObject x = GameObject.Find("ItHandleGodObject");
                GameObject go = Instantiate(Resources.Load("MeVisualizationParticles"), x.transform) as GameObject;
            }
        }

        void Start()
        {
            UpdateSettings();
            CreateTrails();
        }
    }
}