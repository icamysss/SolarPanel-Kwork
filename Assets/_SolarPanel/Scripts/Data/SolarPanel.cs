using UnityEngine;

namespace _SolarPanel.Scripts.Data
{
    [System.Serializable]
    public class SolarPanel {
        public string PanelName;       // Название модели (например, "Восток ФСМ 150Вт")
        public int NominalPower;       // Номинальная мощность (Вт)
        public Vector3 Dimensions;     // Габариты в мм (X=длина, Z=ширина, Y=высота)
        public string Voltage;         // Напряжение (например, "12В")
        public string Technology;      // Технология (например, "PERC M10")

        public SolarPanel(string panelName, int nominalPower, Vector3 dimensions, string voltage, string technology)
        {
            PanelName = panelName;
            NominalPower = nominalPower;
            Dimensions = dimensions;
            Voltage = voltage;
            Technology = technology;
        }
    }
}