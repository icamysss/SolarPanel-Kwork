using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SolarPanel.Scripts.UI.PowerConsumption
{
    public class ApplianceItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI name;
        [SerializeField] private TMP_InputField count;
        [SerializeField] private Toggle select;
        
        public bool IsSelected => select.isOn;
        public string Name => name.text;
        public int Amount => int.Parse(count.text);
        
        public void Initialize(string ApplianceName, int count = 1, bool selected = false)
        {
            
            name.text = ApplianceName;
            this.count.text = count.ToString();
            this.select.isOn = selected;
            this.count.onEndEdit.AddListener(OnCountChanged);
        }

        private void OnCountChanged(string value)
        {
            if (!float.TryParse(value, out var result)) return;
            
            result = result < 0 ? 1 : result;
            this.count.text = result.ToString();
        }
    }
}