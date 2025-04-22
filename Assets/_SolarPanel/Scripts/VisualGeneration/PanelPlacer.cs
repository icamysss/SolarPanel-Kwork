using _SolarPanel.Scripts.Data;
using UnityEngine;

namespace _SolarPanel.Scripts.VisualGeneration
{
    public class PanelPlacer
    {
        public void PlacePanels(GameObject roof, SolarPanel panel, int panelCount) {
            // Размеры панели (в метрах, из PanelDataSO)
            var panelSize = panel.Dimensions / 1000f; // мм → м

            // Начальная позиция (нижний левый край крыши)
            var startPos = roof.transform.position - new Vector3(roof.transform.localScale.x / 2 - panelSize.x / 2, 0, roof.transform.localScale.z / 2 - panelSize.z / 2);

            // Расстояние между панелями (зазор 0.1 м)
            var gap = 0.1f;
        
            // Расчёт рядов
            var panelsPerRow = Mathf.FloorToInt(roof.transform.localScale.x / (panelSize.x + gap));
            var rows = Mathf.CeilToInt(panelCount / (float)panelsPerRow);

            for (var i = 0; i < panelCount; i++) {
                var row = i / panelsPerRow;
                var col = i % panelsPerRow;

                var position = new Vector3(
                    startPos.x + col * (panelSize.x + gap),
                    startPos.y + panelSize.y / 2, // Поднять над крышей
                    startPos.z + row * (panelSize.z + gap)
                    );

                var panelObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                panelObj.transform.parent = roof.transform;
                panelObj.transform.localPosition = position;
                panelObj.transform.localScale = panelSize;
                panelObj.name = "SolarPanel";
            }
        }
    }
}