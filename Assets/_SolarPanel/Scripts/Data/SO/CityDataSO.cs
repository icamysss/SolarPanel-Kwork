using System.Collections.Generic;
using UnityEngine;

namespace _SolarPanel.Scripts.Data.SO
{
    [CreateAssetMenu]
    public class CityDataSO : ScriptableObject {
        public List<City> Cities = new List<City>();
    }
}