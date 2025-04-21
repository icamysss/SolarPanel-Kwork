using System;
using _SolarPanel.Scripts.Data;
using TMPro;
using UnityEngine;

namespace _SolarPanel.Scripts.UI.HouseParameters
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class RoofSelector : MonoBehaviour
    {
        private TMP_Dropdown _roofTypeDropdown;

        private void Awake()
        {
            _roofTypeDropdown = GetComponentInChildren<TMP_Dropdown>();
            if (_roofTypeDropdown == null) throw new NullReferenceException();
        }

        private void Start()
        {
            // Заполнить dropdown значениями из enum RoofType
            PopulateRoofTypeDropdown();
        
            // Подписаться на изменение выбранного значения
            _roofTypeDropdown.onValueChanged.AddListener(OnRoofTypeSelected);
        }
        
        private void PopulateRoofTypeDropdown() {
            // Получить все значения enum
            var roofTypes = Enum.GetValues(typeof(RoofType));
        
            // Очистить старые опции
            _roofTypeDropdown.ClearOptions();
        
            // Добавить новые опции
            foreach (RoofType roofType in roofTypes) {
                _roofTypeDropdown.options.Add(new TMP_Dropdown.OptionData(roofType.ToString()));
            }
            // Обновить отображение
            _roofTypeDropdown.RefreshShownValue();
        }
        private void OnRoofTypeSelected(int index)
        {
            // Получить выбранное значение
            string selectedRoofType = _roofTypeDropdown.options[index].text;
        
            // Преобразовать строку в enum
            var roofType = (RoofType)Enum.Parse(typeof(RoofType), selectedRoofType);
        
            // Сохранить в DataManager
            DataManager.Instance.HouseParam.Roof.RoofType = roofType;
            Debug.Log($"Выбрали тип кровли {selectedRoofType}");
        }
    }
}