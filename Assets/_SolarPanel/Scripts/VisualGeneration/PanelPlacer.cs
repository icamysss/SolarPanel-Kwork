using _SolarPanel.Scripts.Data;
using UnityEngine;

namespace _SolarPanel.Scripts.VisualGeneration
{
    public class PanelPlacer
    {
        private HouseParam houseParam;
        private SolarPanel panel;
        private int panelCount;

        public PanelPlacer(HouseParam houseParam, SolarPanel panel, int panelCount)
        {
            this.houseParam = houseParam;
            this.panel = panel;
            this.panelCount = panelCount;
        }

        public GameObject Place()
        {
            return null;
        }
    }
}