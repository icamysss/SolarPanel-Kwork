using System.Collections.Generic;

namespace _SolarPanel.Scripts.Data
{
    [System.Serializable]
    public class City {
        public string Name;
        public List<MonthData> MonthlyData;
    }
}