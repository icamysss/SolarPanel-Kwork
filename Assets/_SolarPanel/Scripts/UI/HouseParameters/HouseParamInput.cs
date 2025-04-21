using TMPro;
using UnityEngine;

namespace _SolarPanel.Scripts.UI.HouseParameters
{
    public class HouseParamInput : MonoBehaviour
    {
        [SerializeField] private TMP_InputField lengthHouse;
        [SerializeField] private TMP_InputField widthHouse;

        private void Awake()
        {
            lengthHouse.onEndEdit.AddListener(OnLengthHouseChanged);
            widthHouse.onEndEdit.AddListener(OnWidthHouseChanged);
        }

        private void OnLengthHouseChanged(string value)
        {
            if (int.TryParse(value, out var result))
            {
                DataManager.Instance.HouseParam.HouseLength = result;
                Debug.Log($"House Length: {DataManager.Instance.HouseParam.HouseLength}");
            };
           
        }
        
        private void OnWidthHouseChanged(string value)
        {
            var w = int.Parse(value);
            DataManager.Instance.HouseParam.HouseWidth = w;
            Debug.Log($"House Width: {DataManager.Instance.HouseParam.HouseWidth}");
        }
        
    }
}