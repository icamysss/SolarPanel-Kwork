using System.Linq;
using _SolarPanel.Scripts.Data;
using _SolarPanel.Scripts.SM.States;
using UnityEngine;

namespace _SolarPanel.Scripts
{
    public class Calculator
    {
        // Коэффициент потерь из-за отклонения угла наклона от оптимального (5% на 10 градусов)
        private const float AngleLossPerDegree = 0.005f;

        public CalculationResult Calculate()
        {
            // // Получаем данные из DataManager
            // DataManager data = DataManager.Instance;
            // if (data.SelectedCity == null || data.SelectedPanel == null)
            // {
            //     Debug.LogError("Город или панель не выбраны!");
            //     return null;
            // }
            //
            // // 1. Расчёт эффективности угла наклона
            // float optimalTilt = data.SelectedCity.GetOptimalTiltForCurrentMonth();
            // float tiltEfficiency = CalculateTiltEfficiency(data.RoofTiltAngle, optimalTilt);
            //
            // // 2. Расчёт энергии, вырабатываемой одной панелью в месяц
            // float panelEnergyPerMonth = CalculatePanelEnergy(
            //     data.SelectedPanel.NominalPower,
            //     data.SelectedCity.GetMonthlyInsolation(),
            //     tiltEfficiency
            //     );
            //
            // // 3. Расчёт количества панелей
            // int panelCount = Mathf.CeilToInt(data.MonthlyConsumption / panelEnergyPerMonth);
            //
            // // 4. Проверка вместимости на крыше
            // bool fitsOnRoof = CheckRoofSpace(
            //     data.HouseLength,
            //     data.HouseWidth,
            //     data.SelectedPanel.Dimensions,
            //     panelCount
            //     );
            //
            return new CalculationResult
            {
                RequiredPanels = 3 , //panelCount,
                EnergyPerPanel = 400, //panelEnergyPerMonth,
                FitsOnRoof = true
            };
        }

        // Формула: Учёт отклонения угла крыши от оптимального
        private float CalculateTiltEfficiency(float currentTilt, float optimalTilt)
        {
            float angleDifference = Mathf.Abs(currentTilt - optimalTilt);
            return 1 - (angleDifference * AngleLossPerDegree);
        }

        // Формула: Энергия панели = Мощность * Инсоляция * Эффективность угла / 1000
        private float CalculatePanelEnergy(float panelPower, float insolation, float tiltEfficiency)
        {
            return panelPower * insolation * tiltEfficiency / 1000; // в кВт·ч
        }

        // Проверка площади крыши
        private bool CheckRoofSpace(float houseLength, float houseWidth, Vector3 panelSize, int panelCount)
        {
            float roofArea = houseLength * houseWidth; // м²
            float panelArea = (panelSize.x * panelSize.y) / 1_000_000f; // мм² → м²
            return (panelArea * panelCount) <= roofArea;
        }
    }

    public class CalculationResult
    {
        public int RequiredPanels { get; set; }
        public float EnergyPerPanel { get; set; }
        public bool FitsOnRoof { get; set; }
    }
}