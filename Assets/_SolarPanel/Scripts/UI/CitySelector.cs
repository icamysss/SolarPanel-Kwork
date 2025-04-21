using System;
using TMPro;
using UnityEngine;

namespace _SolarPanel.Scripts.UI
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class CitySelector : MonoBehaviour
    {
        private TMP_Dropdown _cityDropdown;

        private void Awake()
        {
            _cityDropdown = GetComponentInChildren<TMP_Dropdown>();
            if (_cityDropdown == null) throw new NullReferenceException();
        }

        private void Start()
        {
            foreach (var cityName in DataManager.Instance.GetCityNames())
            {
                _cityDropdown.options.Add(new TMP_Dropdown.OptionData(cityName));
            }
            _cityDropdown.onValueChanged.AddListener(OnCitySelected);
        }

        private void OnCitySelected(int index)
        {
            var selectedCityName = _cityDropdown.options[index].text;
            DataManager.Instance.SelectCity(selectedCityName);

            // Пример: Вывести среднюю инсоляцию
            var insolation = DataManager.Instance.GetCityAnnualInsolation();
            Debug.Log($"Средняя инсоляция: {insolation} кВт·ч/м²");
        }
    }
}