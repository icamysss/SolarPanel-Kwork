using UnityEngine;

namespace _SolarPanel.Scripts
{
    public static class Constants
    {
        public const string NEXT_BUTTON_TEXT = "Далее";
        public const string BACK_BUTTON_TEXT = "Назад";
        public const string RESTART_BUTTON_TEXT = "Заново";
        public const string CALCULATE_BUTTON_TEXT = "Рассчёт";
        public const string VISUALIZE_BUTTON_TEXT = "Показать";
        
        public const string HOUSE_PARAMETERS_HEADER = "Ввод параметров дома";
        public const string POWER_CONSUMPTION_HEADER = "Ввод параметров потребления";
        public const string CALCULATION_RESULT_HEADER = "Выбор солнечных панелей";
        public const string VISUALIZATION_HEADER = "Визуализация";

        public const float PANELS_KPD = 0.2f;                   // кпд
      
        /// <summary>
        /// зазор между панелями, рядами
        /// </summary>
        public const float PANELS_SPACING = 0.05f;              // зазор между панелями, рядами
        /// <summary>
        /// зазор между панелями и кровлей
        /// </summary>
        public const float PANELS_SPACE_FROM_ROOF = 0.05f;       // зазор между панелями и кровлей
        /// <summary>
        /// расстояние до края крыши
        /// </summary>
        public const float PANELS_SPACING_FROM_END_ROOF = 0.5f;  // расстояние до края крыши
        /// <summary>
        /// Свес кровли
        /// </summary>
        public const float ROOF_OVERHANG = 0.5f;                 // Свес кровли
    }
}