using System.Collections.Generic;
using UnityEngine;

namespace _SolarPanel.Scripts.Data.SO
{
    [CreateAssetMenu(fileName = "ApplianceDataSO", menuName = "SolarApp/ApplianceData")]
    public class ApplianceDataSO : ScriptableObject
    {
        public List<Appliances> Appliances = new List<Appliances>();
    }
}