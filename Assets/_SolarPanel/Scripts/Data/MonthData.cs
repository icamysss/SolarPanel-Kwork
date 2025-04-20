namespace _SolarPanel.Scripts.Data
{
    [System.Serializable]
    public class MonthData {
        public string MonthName; // Название месяца (например, "Январь")
        public float Insolation;  // Солнечная инсоляция (кВт·ч/м²)
        public int OptimalTilt;   // Оптимальный угол наклона (°)

        // Конструктор для удобного заполнения данных
        public MonthData(string monthName, float insolation, int optimalTilt) {
            MonthName = monthName;
            Insolation = insolation;
            OptimalTilt = optimalTilt;
        }
    }
}