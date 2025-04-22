using System;
using TMPro;
using UnityEngine;

namespace _SolarPanel.Scripts.UI.PowerConsumption
{
    public class PowerConsumption : CanvasGroupMenu
    {
        [SerializeField] private TMP_InputField dailyConsumption;

        private void Start()
        {
            if (dailyConsumption == null) throw new NullReferenceException("PowerConsumption");
        }

        public override void Initialize()
        {
           InitDailyConsumption();
        }

        private void InitDailyConsumption()
        {
            dailyConsumption.onEndEdit.AddListener(OnDailyConsumptionEnter);
            dailyConsumption.onEndEdit.Invoke(15f.ToString());
            dailyConsumption.text = 15f.ToString();
        }
        
        private void OnDailyConsumptionEnter(string value)
        {
            if (float.TryParse(value, out var result))
            {
                if (result <= 0f) return;
                DataManager.Instance.DailyConsumption = result;
            }
            Debug.Log($"Суточное потребление: { DataManager.Instance.DailyConsumption}");
        }
    }
}