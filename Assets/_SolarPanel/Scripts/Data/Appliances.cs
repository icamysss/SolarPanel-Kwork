using System;

namespace _SolarPanel.Scripts.Data
{
    [Serializable]
    public class Appliances
    {
        public string Name;             // название прибора
        public float Power;             // мощность прибора в Вт
        public float WorkTime;          // время работы в часах
        public float DailyConsumption;  // дневное потребление в кВт * ч   
    }
}