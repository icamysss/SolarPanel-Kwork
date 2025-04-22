using _SolarPanel.Scripts.Data;
using _SolarPanel.Scripts.VisualGeneration;
using UnityEngine;

namespace _SolarPanel.Scripts
{
    public class VisualizationManager : MonoBehaviour
    {
        public static VisualizationManager Instance;

        private HouseGenerator _houseGenerator;
        private RoofGenerator _roofGenerator;
        private PanelPlacer _panelPlacer;

        private float length;
        private float width;
        private SolarPanel panel;
        private int panelCount;


        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
            DontDestroyOnLoad(gameObject);

            _houseGenerator = new HouseGenerator();
            _roofGenerator = new RoofGenerator();
            _panelPlacer = new PanelPlacer();
        }

        public void Generate()
        {
            GetData();
            // Генерация
            var house = _houseGenerator.GenerateHouse(length, width, 3f); // Высота = 3 м
            _roofGenerator.GenerateRoof(house, length, width, DataManager.Instance.HouseParam.Roof.RoofType);
            _panelPlacer.PlacePanels(house, panel, panelCount);
        }

        private void GetData()
        {
            // Получение данных из DataManager
            length = DataManager.Instance.HouseParam.HouseLength;
            width = DataManager.Instance.HouseParam.HouseWidth;
            panel = DataManager.Instance.SelectedPanel;
            panelCount = DataManager.Instance.GetPanelCount();
        }
    }
}