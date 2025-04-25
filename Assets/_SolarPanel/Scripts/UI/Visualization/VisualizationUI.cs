using TMPro;
using UnityEngine;

namespace _SolarPanel.Scripts.UI.Visualization
{
    public class VisualizationUI : CanvasGroupMenu
    {
        [SerializeField] private TextMeshProUGUI optimalAngle;
        [SerializeField] private TextMeshProUGUI installAngle;

        public void UpdateText()
        {
            optimalAngle.text = $"Отимальный угол :  {GetOptimalAngle()} º";
            installAngle.text = $"Угол установки      :  {GetInstallAngle()} º";
        }

        private string GetInstallAngle()
        {
            var optimalAngle = DataManager.Instance.GetAverageOptimalAngle();
            var roofAngle = DataManager.Instance.HouseParam.Roof.Angle;
            
            return Mathf.Abs(roofAngle - optimalAngle).ToString("0.00");
        }

        private string GetOptimalAngle()
        {
           return DataManager.Instance.GetAverageOptimalAngle().ToString("0.00");
        }
    }
}