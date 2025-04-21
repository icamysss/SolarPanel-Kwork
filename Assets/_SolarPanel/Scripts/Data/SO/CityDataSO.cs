using System;
using System.Collections.Generic;
using UnityEngine;

namespace _SolarPanel.Scripts.Data.SO
{
    [CreateAssetMenu(fileName = "CityDataSO", menuName = "SolarApp/CityData")]
    public class CityDataSO : ScriptableObject {
        public List<City> Cities = new ();

        public float GetAverageAnnualSolarInsolationForTown(string townName)
        {
           
            var town = Cities.Find(x => x.Name == townName);
            if (town == null) throw new NullReferenceException("Такой город не найден!");
            var insolation = 0f;
           
            foreach (var month in town.MonthlyData)
            {
                insolation += month.Insolation;
            }
            return insolation / town.MonthlyData.Count; ;
        }

        public float GetAverageOptimalAngleForTown(string townName)
        {
            var town = Cities.Find(x => x.Name == townName);
            if (town == null) throw new NullReferenceException("Такой город не найден!");
            var angle = 0f;
          
            foreach (var month in town.MonthlyData)
            {
                angle += month.OptimalTilt;
            }
            return angle / town.MonthlyData.Count;
        }
    }
}