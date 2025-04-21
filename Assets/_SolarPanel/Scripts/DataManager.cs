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

        // Текущие данные, введённые пользователем
        public float HouseLength { get; set; }
        public float HouseWidth { get; set; }
        public RoofType RoofType { get; set; }
        public City SelectedCity { get; private set; }
        public SolarPanel SelectedPanel { get; private set; }

        // Кэшированные данные
        private Dictionary<string, City> _citiesCache;
        private Dictionary<string, SolarPanel> _panelsCache;

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

        // Пример: Получить среднюю инсоляцию за год
        public float GetCityAnnualInsolation()
        {
            if (SelectedCity == null) return 0;
            float sum = 0;
            foreach (var month in SelectedCity.MonthlyData)
            {
                sum += month.Insolation;
            }
            return sum / 12;
        }

        public IEnumerable<string> GetCityNames()
        {
            return _citiesCache.Keys;
        }
    }
}