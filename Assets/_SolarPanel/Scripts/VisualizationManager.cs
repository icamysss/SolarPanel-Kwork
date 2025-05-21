using _SolarPanel.Scripts.Data;
using _SolarPanel.Scripts.VisualGeneration;
using UnityEngine;

namespace _SolarPanel.Scripts
{ 
    // ДЛИНА ПО ОСИ Z ШИРИНА ПО ОСИ X
    public class VisualizationManager : MonoBehaviour
    {
        public static VisualizationManager Instance;
        public Texture2D wallTexture;
        public Texture2D panelTexture;

        private HouseParam houseParam;
        private SolarPanel panel;
        private int panelCount;

        private GameObject roof;
        private GameObject house;
        private GameObject panels;
        
        public Vector3 StartPosition;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }

        public void Generate()
        {
            GetData();
            // Дом
            Destroy(house);
            var houseGenerator = new HouseGenerator(houseParam, transform, wallTexture);
            house = houseGenerator.GenerateHouse();
            // кровля
            Destroy(roof);
            var roofGenerator = new RoofGenerator(houseParam, transform);
            roof = roofGenerator.GenerateRoof();
            // Панели
            Destroy(panels);
            var panelPlacer = new PanelPlacer(transform, out StartPosition, panelTexture);
            panels = panelPlacer.Place();
        }

        private void GetData()
        {
            // Получение данных из DataManager
            houseParam = DataManager.Instance.HouseParam;
            panel = DataManager.Instance.SelectedPanel;
        }
    }
}