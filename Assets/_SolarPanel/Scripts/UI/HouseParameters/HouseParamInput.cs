using System;
using _SolarPanel.Scripts.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SolarPanel.Scripts.UI.HouseParameters
{
    public class HouseParamInput : CanvasGroupMenu
    {
        [Header("House")]
        [SerializeField] private TMP_InputField lengthHouse;
        [SerializeField] private TMP_InputField widthHouse;
        
        [Header("Roof")]
        [SerializeField] private TMP_Dropdown roofDropdown;
        [SerializeField] private TextMeshProUGUI angleHeader;
        [SerializeField] private Slider roofAngle;
        
        [Header("Town")]
        [SerializeField] private TMP_Dropdown cityDropdown;

        private void Start()
        {
            if (lengthHouse == null) throw new NullReferenceException("HouseParamInput");
            if (widthHouse == null) throw new NullReferenceException("HouseParamInput");
            if (roofDropdown == null) throw new NullReferenceException("HouseParamInput");
            if (roofAngle == null) throw new NullReferenceException("HouseParamInput");
            if (cityDropdown == null) throw new NullReferenceException("HouseParamInput");
        }

        public override void Initialize()
        {
            base.Initialize();
            lengthHouse.onEndEdit.AddListener(OnLengthHouseChanged);
            widthHouse.onEndEdit.AddListener(OnWidthHouseChanged);
            roofAngle.onValueChanged.AddListener(OnAngleChanged);
            roofAngle.onValueChanged.Invoke(roofAngle.value);
            InitCityDropdown();
            InitRoofDropdown();
            InitHouseParam();
        }
        
        private void InitHouseParam()
        {
            lengthHouse.onEndEdit.Invoke(9.5f.ToString());
            lengthHouse.text = 9.5f.ToString();
            widthHouse.onEndEdit.Invoke(5.5f.ToString());
            widthHouse.text = 5.5f.ToString();
        }

        private void InitRoofDropdown()
        {
            PopulateRoofTypeDropdown();
            roofDropdown.onValueChanged.AddListener(OnRoofTypeSelected);
            roofDropdown.onValueChanged.Invoke(roofDropdown.value);
        }
        private void PopulateRoofTypeDropdown() {
            // Получить все значения enum
            var roofTypes = Enum.GetValues(typeof(RoofType));
        
            // Очистить старые опции
            roofDropdown.ClearOptions();
        
            // Добавить новые опции
            foreach (RoofType roofType in roofTypes) {
                roofDropdown.options.Add(new TMP_Dropdown.OptionData(roofType.ToString()));
            }
            roofDropdown.value = 0;
            // Обновить отображение
            roofDropdown.RefreshShownValue();
        }
        private void OnRoofTypeSelected(int index)
        {
            // Получить выбранное значение
            string selectedRoofType = roofDropdown.options[index].text;
        
            // Преобразовать строку в enum
            var roofType = (RoofType)Enum.Parse(typeof(RoofType), selectedRoofType);
        
            // Сохранить в DataManager
            DataManager.Instance.HouseParam.Roof.RoofType = roofType;
            Debug.Log($"Выбрали тип кровли {selectedRoofType}");
        }
        private void OnAngleChanged(float value)
        {
            DataManager.Instance.HouseParam.Roof.Angle = (int)Mathf.Clamp(value, 0f, 60f);
            angleHeader.text = $"Угол наклона кровли: {value}º";
        }
        
        
        private void InitCityDropdown()
        {
            foreach (var cityName in DataManager.Instance.GetCityNames())
            {
                cityDropdown.options.Add(new TMP_Dropdown.OptionData(cityName));
            }
            cityDropdown.onValueChanged.AddListener(OnCitySelected);
            cityDropdown.value = 0;
            cityDropdown.RefreshShownValue();
            cityDropdown.onValueChanged.Invoke(cityDropdown.value);
        }
        
        private void OnCitySelected(int index)
        {
            var selectedCityName = cityDropdown.options[index].text;
            DataManager.Instance.SelectCity(selectedCityName);
            
            var insolation = DataManager.Instance.GetCityAverageInsolation();
            var angle = DataManager.Instance.GetAverageOptimalAngle();
            Debug.Log($"Средний угол: {angle} град");
            Debug.Log($"Средняя инсоляция: {insolation} кВт·ч/м²");
        }
        
        private void OnLengthHouseChanged(string value)
        {
            if (float.TryParse(value, out var result))
            {
                result = Mathf.Clamp(result, 3f, 30f );
                lengthHouse.text = result.ToString();
                DataManager.Instance.HouseParam.HouseLength = result;
                
            }
            Debug.Log($"House Length: {DataManager.Instance.HouseParam.HouseLength}");
        }
        
        private void OnWidthHouseChanged(string value)
        {
            if (float.TryParse(value, out var result))
            {
                result = Mathf.Clamp(result, 3f, 30f);
                widthHouse.text = result.ToString();
                DataManager.Instance.HouseParam.HouseWidth = result;
            }
            Debug.Log($"House Width: {DataManager.Instance.HouseParam.HouseWidth}");
        }
        
    }
}