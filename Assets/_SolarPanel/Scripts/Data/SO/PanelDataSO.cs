using System.Collections.Generic;
using UnityEngine;

namespace _SolarPanel.Scripts.Data.SO
{
    [CreateAssetMenu(fileName = "PanelDataSO", menuName = "SolarApp/PanelData")]
    public class PanelDataSO : ScriptableObject {
        public List<SolarPanel> Panels = new List<SolarPanel>();
    }
}