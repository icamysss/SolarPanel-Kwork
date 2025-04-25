using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace _SolarPanel.Scripts.UI.PowerConsumption
{
    public class PowerConsumption : CanvasGroupMenu
    {
        [SerializeField] private TMP_InputField dailyConsumption;
        [SerializeField] private Transform applianceItemContainer;
        [SerializeField] private ApplianceItem applianceItem;
        [SerializeField] private CanvasGroup applianceSelecting;
       
        
        private ConsumptionMode _consumptionMode;
        private List<ApplianceItem> _applianceItems = new();

        private float enteredConsumption;
        private void Start()
        {
            if (dailyConsumption == null) throw new NullReferenceException("PowerConsumption");
        }

        public override void Initialize()
        {
            base.Initialize();
            InitDailyConsumption();
            
        }

        private void InitDailyConsumption()
        {
            InputMode = ConsumptionMode.InputDailyConsumption;
            dailyConsumption.onEndEdit.AddListener(OnDailyConsumptionEnter);
            dailyConsumption.onEndEdit.Invoke(100f.ToString());
            dailyConsumption.text = 100f.ToString();

            foreach (var ap in DataManager.Instance.GetAppliancesNames())
            {
                var item = Instantiate(applianceItem, applianceItemContainer);
                item.Initialize(ap);
                _applianceItems.Add(item);
            }
        }

        /// <summary>
        /// Отправлет данные в DataManager
        /// </summary>
        public void CalculateDailyConsumption()
        {
            DataManager.Instance.DailyConsumption = 0;
            if (InputMode == ConsumptionMode.InputDailyConsumption)
            {
                DataManager.Instance.DailyConsumption = enteredConsumption;
            }
            if (InputMode == ConsumptionMode.SelectAppliance)
            {
                foreach (var item in _applianceItems)
                {
                    if (item.IsSelected)
                    {
                        DataManager.Instance.SelectAppliance(item.Name, item.Amount);
                    }
                }
            }
            Debug.Log($"Суточное потребление: { DataManager.Instance.DailyConsumption}");
        }
        
        private void OnDailyConsumptionEnter(string value)
        {
            if (!float.TryParse(value, out var result)) return;
            
            result = Mathf.Clamp(result, 0f, 200f);
            enteredConsumption = result;
        }

        public ConsumptionMode InputMode
        {
            get => _consumptionMode;
            set
            {
                _consumptionMode = value;
                ShowConsumptionMode(_consumptionMode);
            }
        }

        private void ShowConsumptionMode(ConsumptionMode mode)
        {
            if (mode == ConsumptionMode.SelectAppliance)
            {
                dailyConsumption.gameObject.SetActive(false);
                UIManager.Show(applianceSelecting);
            }
            if (mode == ConsumptionMode.InputDailyConsumption)
            {
                dailyConsumption.gameObject.SetActive(true);
                UIManager.Show(applianceSelecting, false);
            }
        }

        public void SwitchConsumptionMode()
        {
            InputMode = InputMode == ConsumptionMode.SelectAppliance ? 
                ConsumptionMode.InputDailyConsumption : ConsumptionMode.SelectAppliance;
        }
    }

    public enum ConsumptionMode
    {
        SelectAppliance, InputDailyConsumption,
    }
}