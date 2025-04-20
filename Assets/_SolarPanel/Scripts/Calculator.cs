using System.Linq;
using _SolarPanel.Scripts.Data;
using UnityEngine;

namespace _SolarPanel.Scripts
{
    public static class Calculator
    {
        public static int CalculateRequiredPower(float monthlyConsumption, City city)
        {
            float annualInsolation = city.MonthlyData.Average(m => m.Insolation);
            return Mathf.CeilToInt(monthlyConsumption / annualInsolation * 1000);
        }
    }
}