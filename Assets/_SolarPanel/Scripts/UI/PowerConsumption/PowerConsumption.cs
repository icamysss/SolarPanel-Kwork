using System;
using TMPro;
using UnityEngine;

namespace _SolarPanel.Scripts.UI.PowerConsumption
{
    public class PowerConsumption : CanvasGroupMenu
    {
        [SerializeField] private TMP_InputField dailyConsumption;

        private ConsumptionMode _consumptionMode;
        
        private void Start()
        {
            if (dailyConsumption == null) throw new NullReferenceException("PowerConsumption");
        }

        public override void Initialize()
        {
            base.Initialize();
            InitDailyConsumption();
            InputMode = ConsumptionMode.InputDailyConsumption;
        }

        private void InitDailyConsumption()
        {
            dailyConsumption.onEndEdit.AddListener(OnDailyConsumptionEnter);
            dailyConsumption.onEndEdit.Invoke(100f.ToString());
            dailyConsumption.text = 100f.ToString();
        }
        
        private void OnDailyConsumptionEnter(string value)
        {
            if (float.TryParse(value, out var result))
            {
                result = Mathf.Clamp(result, 0f, 200f);
                DataManager.Instance.DailyConsumption = result;
            }
            Debug.Log($"Суточное потребление: { DataManager.Instance.DailyConsumption}");
        }

        public ConsumptionMode InputMode
        {
            get => _consumptionMode;
            set
            {
                _consumptionMode = value;
                ShowConsumptionMode(_consumptionMode);
                DataManager.Instance.DailyConsumption = 0f;
            }
        }

        private void ShowConsumptionMode(ConsumptionMode mode)
        {
            if (mode == ConsumptionMode.SelectAppliance)
            {
                dailyConsumption.gameObject.SetActive(false);
            }
            if (mode == ConsumptionMode.InputDailyConsumption)
            {
                dailyConsumption.gameObject.SetActive(true);
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