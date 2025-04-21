namespace _SolarPanel.Scripts.Data
{
    [System.Serializable]
    public class MonthData {
        public MonthType MonthName; // Название месяца
        public float Insolation;  // Солнечная инсоляция (кВт·ч/м²)
        public int OptimalTilt;   // Оптимальный угол наклона (°)

        // Конструктор для удобного заполнения данных
        public MonthData(MonthType monthName, float insolation, int optimalTilt) {
            MonthName = monthName;
            Insolation = insolation;
            OptimalTilt = optimalTilt;
        }
    }

    public enum MonthType
    {
        Январь, Февраль, Март, Апрель, Май, Июнь, Июль, Август, Сентябрь, Октябрь, Ноябрь, Декабрь
    }
}