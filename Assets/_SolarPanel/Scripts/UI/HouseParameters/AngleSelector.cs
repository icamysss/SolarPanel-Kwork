using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SolarPanel.Scripts.UI.HouseParameters
{
    public class AngleSelector: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI header;
        [SerializeField] private Slider slider;

        private void Awake()
        {
            slider.onValueChanged.AddListener(OnAngleChanged);
        }

        private void OnAngleChanged(float value)
        {
            header.text = $"Угол наклона кровли: {value}º";
            DataManager.Instance.HouseParam.Roof.Angle = (int)value;
        }
    }
}