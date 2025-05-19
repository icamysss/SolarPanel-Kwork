using System;
using TMPro;
using UnityEngine;

namespace _SolarPanel.Scripts.UI.CalculationResults
{
    public class Results : CanvasGroupMenu
    {
        [SerializeField] private TextMeshProUGUI requiredPower;
        [SerializeField] private TextMeshProUGUI selectedPanel;
        [SerializeField] private TMP_Dropdown panelDropdown;
        
        public override void Initialize()
        {
            base.Initialize();
            if (panelDropdown == null) throw new NullReferenceException("panelDropdown");
            if (requiredPower == null) throw new NullReferenceException("requiredPower");
            if (selectedPanel == null) throw new NullReferenceException("selectedPanel");
            
            foreach (var panelName in DataManager.Instance.GetAllPanels())
            {
                panelDropdown.options.Add(new TMP_Dropdown.OptionData(panelName));
            }
            panelDropdown.onValueChanged.AddListener(OnPanelSelected);
            panelDropdown.value = 0;
            panelDropdown.RefreshShownValue();
            panelDropdown.onValueChanged.Invoke(0);
            UpdateSelectedPanelText();
        }

        public void UpdateRequiredPowerText(float power)
        {
            requiredPower.text = $"Требуемая мощность панелей: \n {power} кВт";
        }
        
        private void OnPanelSelected(int index)
        {
            var selectedPanelName = panelDropdown.options[index].text;
            DataManager.Instance.SelectPanel(selectedPanelName);
            
            UpdateSelectedPanelText();
            // Пример: Вывести среднюю инсоляцию
            Debug.Log($"Выбрана панель: {DataManager.Instance.SelectedPanel.PanelName}");
        }

        public void UpdateSelectedPanelText()
        {
            selectedPanel.text = $"Необходимое кол-во панелей: {DataManager.Instance.GetPanelCount()} шт. " +
                                 $"\nМощность панели: {DataManager.Instance.SelectedPanel.NominalPower} Вт" +
                                 $"\nДлина панели: {DataManager.Instance.SelectedPanel.Dimensions.x} мм" +
                                 $"\nШирина панели: {DataManager.Instance.SelectedPanel.Dimensions.z} мм";
        }
    }
}