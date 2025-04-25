using System;
using System.Collections.Generic;
using _SolarPanel.Scripts.Data;
using _SolarPanel.Scripts.Data.SO;

using UnityEngine;

namespace _SolarPanel.Scripts
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance { get; private set; }

        // ScriptableObjects с данными
        [SerializeField] private CityDataSO _cityDataSO;
        [SerializeField] private PanelDataSO _panelDataSO;
        [SerializeField] private ApplianceDataSO _applianceDataSO;

        // Текущие данные, введённые пользователем
        
        public HouseParam HouseParam = new ();
        public City SelectedCity { get; private set; }
        public SolarPanel SelectedPanel { get; private set; }
        public float RequiredPower { get; set; }
        public float DailyConsumption { get; set; }
        public List<SelectedAppliance> SelectedAppliances { get; private set; }

        
        private List<SelectedAppliance> _selectedAppliances = new();

        // Кэшированные данные
        private Dictionary<string, City> _citiesCache;
        private Dictionary<string, SolarPanel> _panelsCache;
        private Dictionary<string, Appliances> _appliancesCache;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeData();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Инициализация данных из SO
        private void InitializeData()
        {
            _citiesCache = new Dictionary<string, City>();
            foreach (var city in _cityDataSO.Cities)
            {
                _citiesCache[city.Name] = city;
            }

            _panelsCache = new Dictionary<string, SolarPanel>();
            foreach (var panel in _panelDataSO.Panels)
            {
                _panelsCache[panel.PanelName] = panel;
            }

            _appliancesCache = new Dictionary<string, Appliances>();
            foreach (var appliance in _applianceDataSO.Appliances)
            {
                _appliancesCache[appliance.Name] = appliance;
            }
            
            HouseParam.HouseHeight = Constants.HOUSE_HEIGHT;
        }

        // Методы для работы с данными
        public void SelectCity(string cityName)
        {
            if (_citiesCache.TryGetValue(cityName, out var city))
            {
                SelectedCity = city;
            }
        }

        public void SelectPanel(string panelName)
        {
            if (_panelsCache.TryGetValue(panelName, out var panel))
            {
                SelectedPanel = panel;
            }
        }
        
        public int GetPanelCount()
        {
            return 
                Mathf.Clamp(RequiredPower <= 0 ? 0 : 
                    Mathf.RoundToInt(RequiredPower * 1000 / SelectedPanel.NominalPower),1, 100);
        }
        
        public float CalculateRequiredPower()
        {
            RequiredPower = DailyConsumption / GetCityAverageInsolation() * Constants.PANELS_KPD;
            return RequiredPower; 
        }

        // Пример: Получить среднюю инсоляцию за год
        public float GetCityAverageInsolation()
        {
            if (SelectedCity == null) return 0;
            float sum = 0;
           
            foreach (var month in SelectedCity.MonthlyData)
            {
                sum += month.Insolation;
            }
            return sum / SelectedCity.MonthlyData.Count;
        }

        public IEnumerable<string> GetCityNames()
        {
            return _citiesCache.Keys;
        }

        public IEnumerable<string> GetAllPanels()
        {
           return _panelsCache.Keys;
        }

        public float GetAverageOptimalAngle()
        {
            if (SelectedCity == null) return 0;
            float sum = 0;
           
            foreach (var month in SelectedCity.MonthlyData)
            {
                sum += month.OptimalTilt;
            }
            return sum / SelectedCity.MonthlyData.Count;
        }
    }

    [Serializable]
    public class HouseParam
    {
        public float HouseLength { get; set; } = 9f;
        public float HouseWidth { get; set; } = 7f;
        public float HouseHeight { get; set; } = 3f;
        public RoofParam Roof { get; set; } = new();
    }

    [Serializable]
    public class RoofParam
    {
        public RoofType RoofType { get; set; } = RoofType.Односкатная;
        public int Angle { get; set; } = 12;
    }

    [Serializable]
    public class SelectedAppliance
    {
        public Appliances Appliance;
        public int Count;

        public SelectedAppliance(Appliances appliance, int count)
        {
            Appliance = appliance;
            Count = count;
        }
    }
}