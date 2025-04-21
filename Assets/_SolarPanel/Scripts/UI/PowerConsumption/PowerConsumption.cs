using System;
using TMPro;
using UnityEngine;

namespace _SolarPanel.Scripts.UI.PowerConsumption
{
    public class PowerConsumption : MonoBehaviour
    {
        [SerializeField] private TMP_InputField dailyConsumption;

        private void Awake()
        {
            if (dailyConsumption == null) throw new NullReferenceException("PowerConsumption");
            dailyConsumption.onEndEdit.AddListener(OnDailyConsumptionEnter);
        }

        private void OnDailyConsumptionEnter(string value)
        {
            DataManager.Instance.DailyConsumption = float.Parse(value);
            Debug.Log($"Суточное потребление: { DataManager.Instance.DailyConsumption}");
        }
    }
}